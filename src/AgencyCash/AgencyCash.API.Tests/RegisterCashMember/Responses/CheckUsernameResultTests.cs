using AgencyCash.API.RegisterCashMember.Responses;

namespace AgencyCash.API.Tests.RegisterCashMember.Responses;

public class CheckUsernameResultTests
{
    [Fact]
    public void Constructor_SetsAvailable()
    {
        var result = new CheckUsernameResult(true);
        Assert.True(result.Available);
        var result2 = new CheckUsernameResult(false);
        Assert.False(result2.Available);
    }
}