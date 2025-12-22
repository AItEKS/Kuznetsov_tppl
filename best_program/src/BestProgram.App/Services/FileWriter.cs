namespace BestProgram.Services;

using System.Threading.Channels;

using BestProgram.Domain;

public class FileWriter
{
    private readonly string _filePath;
    private readonly ChannelReader<DataModel> _reader;

    public FileWriter(string filePath, ChannelReader<DataModel> reader)
    {
        this._filePath = filePath;
        this._reader = reader;
    }

    public async Task RunAsync(CancellationToken token)
    {
        using (StreamWriter writer = new StreamWriter(_filePath, true))
        {
            await foreach (var item in _reader.ReadAllAsync(token))
            {
                var resultString = $"[{item.Timestamp:T}] {item.Values}";
                await writer.WriteLineAsync(resultString);
            }
        }
    }
}