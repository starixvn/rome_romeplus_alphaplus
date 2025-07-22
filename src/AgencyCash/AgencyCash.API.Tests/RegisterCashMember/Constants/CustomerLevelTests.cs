using AgencyCash.API.RegisterCashMember.Constants;

namespace AgencyCash.API.Tests.RegisterCashMember.Constants;

public class CustomerLevelTests
{
    [Fact]
    public void Constants_HaveExpectedValues()
    {
        Assert.Equal((byte)1, CustomerLevel.Member);
        Assert.Equal((byte)2, CustomerLevel.Agent);
        Assert.Equal((byte)3, CustomerLevel.Master);
        Assert.Equal((byte)4, CustomerLevel.Super);
    }
}