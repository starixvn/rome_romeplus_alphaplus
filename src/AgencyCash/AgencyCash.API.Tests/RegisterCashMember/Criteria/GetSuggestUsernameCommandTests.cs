using AgencyCash.API.RegisterCashMember.Criteria;

namespace AgencyCash.API.Tests.RegisterCashMember.Criteria;

public class GetSuggestUsernameCommandTests
{
    [Fact]
    public void Constructor_SetsProperties()
    {
        var cmd = new GetSuggestUsernameCommand("agent", 1);
        Assert.Equal("agent", cmd.ActorName);
        Assert.Equal((byte)1, cmd.RoleId);
        Assert.Equal("Age_CustomerSelSuggestUsernameNew", cmd.GetSettingKey());
        Assert.True(cmd.IsValid());
    }
}