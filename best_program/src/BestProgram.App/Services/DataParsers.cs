namespace BestProgram.Services;

using System.Buffers.Binary;

using BestProgram.Domain;

public static class DataParser
{
    private static DateTime ParseTimestamp(ReadOnlySpan<byte> timeSlice)
    {
        long microseconds = BinaryPrimitives.ReadInt64BigEndian(timeSlice);
        
        long ticks = microseconds * 10;
        
        return DateTime.UnixEpoch.AddTicks(ticks);
    }

    public static DataModel ParseWeather(ReadOnlySpan<byte> data)
    {
        DateTime timestamp = ParseTimestamp(data.Slice(0, 8));

        float temperature = BinaryPrimitives.ReadSingleBigEndian(data.Slice(8, 4));
        short pressure = BinaryPrimitives.ReadInt16BigEndian(data.Slice(12, 2));
        
        string source = "Sensor Weather";
        string values = $"Temp: {temperature:F2} | Press: {pressure}";
        
        return new DataModel(timestamp, source, values);
    }

    public static DataModel ParseCoords(ReadOnlySpan<byte> data)
    {
        DateTime timestamp = ParseTimestamp(data.Slice(0, 8));

        long x = BinaryPrimitives.ReadInt32BigEndian(data.Slice(8, 4));
        long y = BinaryPrimitives.ReadInt32BigEndian(data.Slice(12, 4));
        long z = BinaryPrimitives.ReadInt32BigEndian(data.Slice(16, 4));
        
        string source = "Sensor Coords";
        string values = $"X: {x} | Y: {y} | Z: {z}";
        
        return new DataModel(timestamp, source, values);
    }
}