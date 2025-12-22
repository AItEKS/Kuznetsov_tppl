namespace BestProgram.Utils;

public static class ByteUtils
{
    public static bool ValidateChecksum(ReadOnlySpan<byte> data)
    {
        if (data.Length == 0)
        {
            return false;
        }

        int sum = 0;
        foreach (byte b in data[..^1])
        {
            sum += b;
        }

        return (sum % 256) == data[^1];
    }
} 