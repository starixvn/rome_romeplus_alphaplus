using AgencyCash.API.RegisterCashMember.Criteria;

namespace AgencyCash.API.Tests.RegisterCashMember.Criteria;

public class RegisterCustomerBeforeAddNewCommandTests
{
    [Fact]
    public void Constructor_SetsProperties()
    {
        var cmd = new RegisterCustomerBeforeAddNewCommand("user1", "user2", 1, 2);
        Assert.Equal("user1", cmd.Username);
        Assert.Equal("user2", cmd.Username2);
        Assert.Equal((byte)1, cmd.RoleId);
        Assert.Equal(2, cmd.UplineId);
        Assert.Equal("API_CustomerIns_CashMember_Register", cmd.GetSettingKey());
        Assert.True(cmd.IsValid());
    }
}