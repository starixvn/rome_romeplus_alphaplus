using AgencyCash.API.RegisterCashMember.Criteria;

namespace AgencyCash.API.Tests.RegisterCashMember.Criteria;

public class CheckUsernameCommandTests
{
    [Fact]
    public void Constructor_SetsProperties()
    {
        var cmd = new CheckUsernameCommand("user");
        Assert.Equal("user", cmd.Username);
        Assert.Equal("API_CustomerInsMember_CashMember_CheckUsername", cmd.GetSettingKey());
        Assert.True(cmd.IsValid());
    }
}