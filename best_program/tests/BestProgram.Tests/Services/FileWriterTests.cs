namespace BestProgram.Tests;

using Xunit;
using System.IO;

using System.Threading.Channels;
using BestProgram.Services;
using BestProgram.Domain;

public class FileWriterTests : IDisposable
{
    private readonly string _tempFilePath;

    public FileWriterTests()
    {
        _tempFilePath = Path.GetTempFileName();
    }

    [Fact]
    public async Task RunAsync_ShouldWriteDataToFile_WhenChannelHasItems()
    {
        var channel = Channel.CreateUnbounded<DataModel>();
        
        var fileWriter = new FileWriter(_tempFilePath, channel.Reader);

        var testData = new DataModel(new DateTime(2023, 10, 10, 12, 0, 0, DateTimeKind.Utc), "Source", "10, 20, 30");

        await channel.Writer.WriteAsync(testData);
        channel.Writer.Complete(); 

        await fileWriter.RunAsync(CancellationToken.None);

        var fileContent = await File.ReadAllLinesAsync(_tempFilePath);

        Assert.NotEmpty(fileContent);
        Assert.Contains("10, 20, 30", fileContent[0]);
        Assert.Contains("12:00:00", fileContent[0]);
    }

    public void Dispose()
    {
        if (File.Exists(_tempFilePath))
        {
            File.Delete(_tempFilePath);
        }
    }
}