namespace BestProgram.Tests;

using System.Net;
using System.Net.Sockets;
using System.Threading.Channels;

using BestProgram.Services;
using BestProgram.Domain;

public class ResilientTcpClientTests
{
    [Fact]
    public async Task RunAsync_ShouldConnectAndReceiveData()
    {
        int port = 55555;
        int packetSize = 15;
        var channel = Channel.CreateUnbounded<DataModel>();
        
        var cts = new CancellationTokenSource(); 
        cts.CancelAfter(TimeSpan.FromSeconds(5));

        byte[] validPacket = new byte[15]
        {
            0x00, 0x06, 0x46, 0x99, 0x8A, 0x08, 0x3C, 0x00,
            0x41, 0xC3, 0xD7, 0x0A,
            0x03, 0xF5,
            0x90
        };

        var serverTask = Task.Run(async () => 
        {
            var listener = new TcpListener(IPAddress.Loopback, port);
            listener.Start();
            
            using var client = await listener.AcceptTcpClientAsync(cts.Token);
            using var stream = client.GetStream();

            byte[] buffer = new byte[1024]; 

            await stream.ReadAsync(buffer, cts.Token);
            await stream.WriteAsync(new byte[256], cts.Token);
            await stream.ReadAsync(buffer, cts.Token);
            await stream.WriteAsync(validPacket, cts.Token);
            
            listener.Stop();
        });

        var service = new ResilientTcpClient(
            "127.0.0.1", 
            port, 
            packetSize, 
            bytes => new DataModel(new DateTime(2025, 12, 23, 7, 32, 0, DateTimeKind.Utc), "Sensor Weather", "Temp: 24,48 | Press: 1013"), 
            channel.Writer
        );

        var clientTask = service.RunAsync(cts.Token);

        try 
        {
            var result = await channel.Reader.ReadAsync(cts.Token);
            
            Assert.NotNull(result);
            Assert.Equal("Temp: 24,48 | Press: 1013", result.Values);
        }
        finally
        {
            cts.Cancel();
            try { await clientTask; } catch (OperationCanceledException) {}
        }
    }

    [Fact]
    public async Task RunAsync_ShouldReconnect_WhenChecksumIsInvalid()
    {
        int port = 55557;
        int packetSize = 15;
        var channel = Channel.CreateUnbounded<DataModel>();
        
        var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));

        byte[] invalidPacket = new byte[15]
        {
            0x00, 0x06, 0x46, 0x99, 0x8A, 0x08, 0x3C, 0x00,
            0x41, 0xC3, 0xD7, 0x0A,
            0x03, 0xF5,
            0x91
        };

        var serverTask = Task.Run(async () => 
        {
            var listener = new TcpListener(IPAddress.Loopback, port);
            listener.Start();
            
            {
                using var client1 = await listener.AcceptTcpClientAsync(cts.Token);
                using var stream1 = client1.GetStream();
                byte[] buf = new byte[1024];

                await stream1.ReadAsync(buf, cts.Token);
                await stream1.WriteAsync(new byte[256], cts.Token);
                await stream1.ReadAsync(buf, cts.Token);

                await stream1.WriteAsync(invalidPacket, cts.Token);
            }

            try 
            {
                using var client2 = await listener.AcceptTcpClientAsync(cts.Token);
            }
            catch(OperationCanceledException)
            {
                throw new Exception("Client did not reconnect after Invalid Checksum exception");
            }
            
            listener.Stop();
        });

        var service = new ResilientTcpClient(
            "127.0.0.1", port, packetSize, 
            bytes => new DataModel(DateTime.UtcNow, "Ok", "Ok"), 
            channel.Writer
        );

        var clientTask = service.RunAsync(cts.Token);

        await Task.WhenAny(serverTask, Task.Delay(6000));
        
        cts.Cancel();
        
        try { await clientTask; } catch (OperationCanceledException) {}
        
        Assert.True(serverTask.IsCompletedSuccessfully, "Server did not accept the second connection (reconnect failed)");
        
        Assert.Equal(0, channel.Reader.Count);
    }

    [Fact]
    public async Task RunAsync_ShouldReconnect_WhenConnectionClosedPrematurely()
    {
        int port = 55558;
        int packetSize = 15;
        var channel = Channel.CreateUnbounded<DataModel>();
        var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));

        var serverTask = Task.Run(async () => 
        {
            var listener = new TcpListener(IPAddress.Loopback, port);
            listener.Start();
            
            {
                using var client1 = await listener.AcceptTcpClientAsync(cts.Token);
                using var stream1 = client1.GetStream();
                byte[] buf = new byte[1024];

                await stream1.ReadAsync(buf, cts.Token);
                await stream1.WriteAsync(new byte[256], cts.Token);
                await stream1.ReadAsync(buf, cts.Token);

                byte[] partialData = new byte[] { 0x01, 0x02, 0x03, 0x04, 0x05 };
                await stream1.WriteAsync(partialData, cts.Token);
                
                client1.Close(); 
            }

            try 
            {
                using var client2 = await listener.AcceptTcpClientAsync(cts.Token);
            }
            catch(OperationCanceledException)
            {
                throw new Exception("Client did not reconnect after Connection Closed exception");
            }
            
            listener.Stop();
        });

        var service = new ResilientTcpClient(
            "127.0.0.1", port, packetSize, 
            bytes => new DataModel(DateTime.UtcNow, "Ok", "Ok"), 
            channel.Writer
        );

        var clientTask = service.RunAsync(cts.Token);
        await Task.WhenAny(serverTask, Task.Delay(6000));
        
        cts.Cancel();
        try { await clientTask; } catch (OperationCanceledException) {}

        Assert.True(serverTask.IsCompletedSuccessfully, "Server did not accept reconnect after premature close");
        Assert.Equal(0, channel.Reader.Count);
    }
}