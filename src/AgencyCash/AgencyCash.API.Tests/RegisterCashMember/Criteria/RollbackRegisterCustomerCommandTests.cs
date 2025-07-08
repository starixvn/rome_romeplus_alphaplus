using AgencyCash.API.RegisterCashMember.Criteria;

namespace AgencyCash.API.Tests.RegisterCashMember.Criteria;

public class RollbackRegisterCustomerCommandTests
{
    [Fact]
    public void Constructor_SetsProperties()
    {
        var cmd = new RollbackRegisterCustomerCommand(123);
        Assert.Equal(123, cmd.CustId);
        Assert.Equal("Acc_CustomerIns_Rollback", cmd.GetSettingKey());
        Assert.True(cmd.IsValid());
    }
}