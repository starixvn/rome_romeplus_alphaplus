using Fanex.Alpha.Before.Extensions;

namespace Fanex.Alpha.Before.UnitTests.Extensions;

public class EncryptExtensionTests
{
    [Fact]
    public void RandomNumberString_LengthAndDigitsOnly()
    {
        var s = EncryptExtension.RandomNumberString(32);
        Assert.Equal(32, s.Length);
        Assert.Matches("^[0-9]+$", s);
    }

    [Fact]
    public void CfsEnCode_DeterministicForSameInput()
    {
        var a = EncryptExtension.CfsEnCode("abc");
        var b = EncryptExtension.CfsEnCode("abc");
        Assert.Equal(a, b);
        Assert.NotEmpty(a);
    }
}


