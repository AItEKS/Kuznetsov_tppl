namespace BestProgram.Tests;

using BestProgram.Utils;

public class ByteUtilsTests
{
    [Fact]
    public void ValidateChecksum_CorrectByteData_ShouldReturnTrue()
    {
        byte[] data = { 0x01, 0x02, 0x03, 0x06 };
        ReadOnlySpan<byte> span = data;

        bool result = ByteUtils.ValidateChecksum(span);

        Assert.True(result);
    }

    [Fact]
    public void ValidateChecksum_InvalidByteData_ShouldReturnFalse()
    {
        byte[] data = { 0x01, 0x02, 0x03, 0x07 };
        ReadOnlySpan<byte> span = data;

        bool result = ByteUtils.ValidateChecksum(span);

        Assert.False(result);
    }

    [Fact]
    public void ValidateChecksum_EmptyByteData_ShouldReturnFalse()
    {
        byte[] data = { };
        ReadOnlySpan<byte> span = data;

        bool result = ByteUtils.ValidateChecksum(span);

        Assert.False(result);
    }
}