namespace BestProgram.Tests;

using System.Buffers.Binary;
using BestProgram.Domain;
using BestProgram.Services;

public class DataParserTests
{
    [Fact]
    public void ParseTimestamp_ParseMicroSecs_CorrectDateTime()
    {
        byte[] timeSlice = new byte[8];
        long microseconds = 0;
        BinaryPrimitives.WriteInt64BigEndian(timeSlice, microseconds);
        
        var result = DataParser.ParseTimestamp(timeSlice.AsSpan());

        Assert.Equal(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc), result);
    }

    [Fact]
    public void ParseWeather_ParseByteData_CorrectDataModel()
    {
        byte[] testData = new byte[14]
        {
            // Timestamp: 23.12.2025 07:32:00
            0x00, 0x06, 0x46, 0x99, 0x8A, 0x08, 0x3C, 0x00,
            
            // Temperature: 23.5
            0x41, 0xC3, 0xD7, 0x0A,
            
            // Pressure: 1013
            0x03, 0xF5
        };

        var result = DataParser.ParseWeather(testData.AsSpan());

        DateTime expectedTimestamp = new DateTime(2025, 12, 23, 7, 32, 0, DateTimeKind.Utc);
        string expectedSource = "Sensor Weather";
        string expectedValues = "Temp: 24,48 | Press: 1013";

        Assert.Equal(expectedTimestamp, result.Timestamp);
        Assert.Equal(expectedSource, result.Source);
        Assert.Equal(expectedValues, result.Values);
    }

    [Fact]
    public void ParseCoords_ParseByteData_CorrectDataModel()
    {
        byte[] testData = new byte[20]
        {
            // Timestamp: 23.12.2025 07:32:00
            0x00, 0x06, 0x46, 0x99, 0x8A, 0x08, 0x3C, 0x00,
            
            // X = 12345
            0x00, 0x00, 0x30, 0x39,

            // Y = -10100
            0xFF, 0xFF, 0xD8, 0x8C,

            // Z = 5000
            0x00, 0x00, 0x13, 0x88
        };

        var result = DataParser.ParseCoords(testData.AsSpan());

        DateTime expectedTimestamp = new DateTime(2025, 12, 23, 7, 32, 0, DateTimeKind.Utc);
        string expectedSource = "Sensor Coords";
        string expectedValues = "X: 12345 | Y: -10100 | Z: 5000";

        Assert.Equal(expectedTimestamp, result.Timestamp);
        Assert.Equal(expectedSource, result.Source);
        Assert.Equal(expectedValues, result.Values);
    }
}