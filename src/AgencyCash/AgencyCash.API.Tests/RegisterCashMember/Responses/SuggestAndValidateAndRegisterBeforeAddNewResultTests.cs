using AgencyCash.API.RegisterCashMember.Responses;

namespace AgencyCash.API.Tests.RegisterCashMember.Responses;

public class SuggestUsernameResultTests
{
    [Fact]
    public void Constructor_SetsProperties()
    {
        var result = new SuggestUsernameResult(0, "ok", "newuser");
        Assert.Equal(0, result.Code);
        Assert.Equal("ok", result.Message);
        Assert.Equal("newuser", result.SeqName);
    }
}

public class ValidateCashMemberResultTests
{
    [Fact]
    public void Constructor_SetsProperties()
    {
        var result = new ValidateCashMemberResult(0, "ok", "agent");
        Assert.Equal(0, result.Code);
        Assert.Equal("ok", result.Message);
        Assert.Equal("agent", result.AgentUsername);
    }
}

public class RegisterBeforeAddNewResultTests
{
    [Fact]
    public void Constructor_SetsProperties()
    {
        var result = new RegisterBeforeAddNewResult(0, "ok", 123);
        Assert.Equal(0, result.Code);
        Assert.Equal("ok", result.Message);
        Assert.Equal(123, result.CustId);
    }
}