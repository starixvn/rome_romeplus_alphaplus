using Xunit;
using AgencyCash.API.Shared.Helpers;

namespace AgencyCash.API.Tests.Shared.Helpers
{
    public class SPNameHelperTests
    {
        [Fact]
        public void CustDBPrefix_WithGroupId_ReturnsPrefixed()
        {
            var result = SPNameHelper.CustDBPrefix("sp", 5);
            Assert.Equal("CustDB5_sp", result);
        }

        [Fact]
        public void CustDBPrefix_WithoutGroupId_ReturnsSpName()
        {
            var result = SPNameHelper.CustDBPrefix("sp", 0);
            Assert.Equal("sp", result);
        }

        [Fact]
        public void SiteDBPrefix_WithGroupId_ReturnsPrefixed()
        {
            var result = SPNameHelper.SiteDBPrefix("sp", 3);
            Assert.Equal("SiteDB3_sp", result);
        }
    }
}
