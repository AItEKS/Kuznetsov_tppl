using System.Threading.Channels;

using BestProgram.Domain;
using BestProgram.Services;
using BestProgram.Config;

var channel = Channel.CreateBounded<DataModel>(new BoundedChannelOptions(500)
{
    FullMode = BoundedChannelFullMode.Wait
});

var cts = new CancellationTokenSource();
Console.CancelKeyPress += (sender, args) =>
{
    Console.WriteLine("\nОстанавливаем систему...");
    args.Cancel = true;
    cts.Cancel();
};

var fileWriter = new FileWriter("output.txt", channel.Reader);

var weatherClient = new ResilientTcpClient(
    Config.Ip,
    Config.WeatherPort,
    15,
    DataParser.ParseWeather,
    channel.Writer
);

var coordClient = new ResilientTcpClient(
    Config.Ip,
    Config.CoordPort,
    21,
    DataParser.ParseCoords,
    channel.Writer
);

Task t1 = fileWriter.RunAsync(cts.Token);
Task t2 = weatherClient.RunAsync(cts.Token);
Task t3 = coordClient.RunAsync(cts.Token);

try
{
    await Task.WhenAll(t1, t2, t3);
}
catch (OperationCanceledException)
{
    Console.WriteLine("Отмена выполнена корректно");
}
catch (Exception ex)
{
    Console.WriteLine($"Критическая ошибка: {ex.Message}");
}

Console.WriteLine("Система остановлена");