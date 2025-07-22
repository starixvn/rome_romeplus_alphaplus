using AgencyCash.API.RegisterCashMember.Criteria;

namespace AgencyCash.API.Tests.RegisterCashMember.Criteria;

public class ValidateSettingCommandTests
{
    [Fact]
    public void Constructor_SetsProperties()
    {
        var cmd = new ValidateSettingCommand("user", "domain", 1);
        Assert.Equal("user", cmd.Username);
        Assert.Equal("domain", cmd.Domain);
        Assert.Equal(1, cmd.AgentId);
        Assert.Equal("API_CustomerInsMember_CashMember_Validate", cmd.GetSettingKey());
        Assert.True(cmd.IsValid());
    }
}