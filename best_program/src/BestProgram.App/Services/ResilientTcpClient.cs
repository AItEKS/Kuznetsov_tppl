namespace BestProgram.Services;

using System.Threading.Channels;
using System.Net.Sockets;

using BestProgram.Domain;
using BestProgram.Config;
using BestProgram.Utils;

public class ResilientTcpClient
{   
    private readonly string _ip;
    private readonly int _port;
    private readonly int _packetSize;
    private readonly Func<ReadOnlySpan<byte>, DataModel> _parser;

    private readonly ChannelWriter<DataModel> _writer;

    public ResilientTcpClient(string ip, int port, int packetSize, Func<ReadOnlySpan<byte>, DataModel> parserStrategy, ChannelWriter<DataModel> writer)
    {
        this._ip = ip;
        this._port = port;
        this._packetSize = packetSize;
        this._parser = parserStrategy;
        this._writer = writer;
    }

    public async Task RunAsync(CancellationToken token)
    {
        while(!token.IsCancellationRequested) 
        {
            try
            {
                byte[] buffer = new byte[_packetSize];

                using var client = new TcpClient();
                client.ReceiveTimeout = 3000;
                
                await client.ConnectAsync(_ip, _port);
                
                using var stream = client.GetStream();
                
                await stream.WriteAsync(System.Text.Encoding.UTF8.GetBytes(Config.SecretKey));
                
                byte[] authBuffer = new byte[256];
                int authBytesRead = await stream.ReadAsync(authBuffer.AsMemory(), token); 

                while (client.Connected && !token.IsCancellationRequested)
                {
                    await stream.WriteAsync(System.Text.Encoding.UTF8.GetBytes(Config.GetKey));

                    int totalRecivedBytes = 0;
                    while(totalRecivedBytes != _packetSize)
                    {
                        int data = await stream.ReadAsync(buffer, totalRecivedBytes, _packetSize - totalRecivedBytes, token);
                        totalRecivedBytes += data;

                        if (data == 0)
                        {
                            throw new Exception("Connection closed");
                        }
                    }

                    if (!ByteUtils.ValidateChecksum(buffer))
                    {
                        throw new Exception("Invalid checksum");
                    }

                    DataModel dataModel = _parser(buffer);

                    await _writer.WriteAsync(dataModel, token);
                }
            } 
            catch (Exception ex)
            {
                Console.WriteLine($"Something went wrong: {ex.Message}. Reconnecting in 2 sec...");
                await Task.Delay(2000, token);
            }
        }
    }
}