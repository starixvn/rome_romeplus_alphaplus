using AgencyCash.API.RegisterCashMember.Requests;

namespace AgencyCash.API.Tests.RegisterCashMember.Requests;

public class CheckUsernameRequestTests
{
    [Fact]
    public void Can_Set_Username()
    {
        var req = new CheckUsernameRequest { Username = "user" };
        Assert.Equal("user", req.Username);
    }
}