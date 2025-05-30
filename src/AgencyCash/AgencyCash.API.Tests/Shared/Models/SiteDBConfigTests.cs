using AgencyCash.API.Shared.Models;

namespace AgencyCash.API.Tests.Shared.Models
{
    public class SiteDBConfigTests
    {
        [Fact]
        public void Can_Set_Properties()
        {
            var config = new SiteDBConfig { CustDBGroupId = 1, SiteDBGroupId = 2 };
            Assert.Equal(1, config.CustDBGroupId);
            Assert.Equal(2, config.SiteDBGroupId);
        }
    }
}
