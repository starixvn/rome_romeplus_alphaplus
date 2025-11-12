using AlphaPlus.Features.Messages.API.Models.Customer;
using Xunit;

namespace AlphaPlus.Features.Messages.API.UnitTests.Models.Customer
{
    public class CustomerTests
    {
        [Fact]
        public void DefaultConstructor_InitializesWithDefaults()
        {
            // Act
            var customer = new AlphaPlus.Features.Messages.API.Models.Customer.Customer();

            // Assert
            Assert.Equal(0, customer.CustId);
            Assert.Equal(string.Empty, customer.UserName);
            Assert.Equal(0, customer.RoleId);
            Assert.Equal(0, customer.CurrencyId);
            Assert.Equal(0, customer.ARecommendId);
            Assert.Equal(0, customer.MRecommendId);
            Assert.Equal(0, customer.SRecommendId);
            Assert.Equal(string.Empty, customer.Site);
            Assert.False(customer.IsAllDayBalTransfer);
            Assert.False(customer.Enabled);
            Assert.Equal(string.Empty, customer.SiteLabel);
        }

        [Fact]
        public void CustId_CanBeSetAndRetrieved()
        {
            // Arrange
            var customer = new AlphaPlus.Features.Messages.API.Models.Customer.Customer();
            var custId = 12345;

            // Act
            customer.CustId = custId;

            // Assert
            Assert.Equal(custId, customer.CustId);
        }

        [Fact]
        public void UserName_CanBeSetAndRetrieved()
        {
            // Arrange
            var customer = new AlphaPlus.Features.Messages.API.Models.Customer.Customer();
            var userName = "testuser";

            // Act
            customer.UserName = userName;

            // Assert
            Assert.Equal(userName, customer.UserName);
        }

        [Fact]
        public void RoleId_CanBeSetAndRetrieved()
        {
            // Arrange
            var customer = new AlphaPlus.Features.Messages.API.Models.Customer.Customer();
            byte roleId = 5;

            // Act
            customer.RoleId = roleId;

            // Assert
            Assert.Equal(roleId, customer.RoleId);
        }

        [Fact]
        public void CurrencyId_CanBeSetAndRetrieved()
        {
            // Arrange
            var customer = new AlphaPlus.Features.Messages.API.Models.Customer.Customer();
            byte currencyId = 3;

            // Act
            customer.CurrencyId = currencyId;

            // Assert
            Assert.Equal(currencyId, customer.CurrencyId);
        }

        [Fact]
        public void RecommendIds_CanBeSetAndRetrieved()
        {
            // Arrange
            var customer = new AlphaPlus.Features.Messages.API.Models.Customer.Customer();
            var aRecommendId = 100;
            var mRecommendId = 200;
            var sRecommendId = 300;

            // Act
            customer.ARecommendId = aRecommendId;
            customer.MRecommendId = mRecommendId;
            customer.SRecommendId = sRecommendId;

            // Assert
            Assert.Equal(aRecommendId, customer.ARecommendId);
            Assert.Equal(mRecommendId, customer.MRecommendId);
            Assert.Equal(sRecommendId, customer.SRecommendId);
        }

        [Fact]
        public void Site_CanBeSetAndRetrieved()
        {
            // Arrange
            var customer = new AlphaPlus.Features.Messages.API.Models.Customer.Customer();
            var site = "www.example.com";

            // Act
            customer.Site = site;

            // Assert
            Assert.Equal(site, customer.Site);
        }

        [Fact]
        public void IsIMBC_ReturnsTrueWhenSiteLabelIsMBC()
        {
            // Arrange
            var customer = new AlphaPlus.Features.Messages.API.Models.Customer.Customer();
            customer.SiteLabel = "MBC";

            // Act
            var result = customer.IsIMBC;

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsIMBC_ReturnsFalseWhenSiteLabelIsNotMBC()
        {
            // Arrange
            var customer = new AlphaPlus.Features.Messages.API.Models.Customer.Customer();
            customer.SiteLabel = "OTHER";

            // Act
            var result = customer.IsIMBC;

            // Assert
            Assert.False(result);
        }

        [Theory]
        [InlineData("MBC", true)]
        [InlineData("mbc", false)]
        [InlineData("ABC", false)]
        [InlineData("", false)]
        public void IsIMBC_WithDifferentSiteLabels_ReturnsExpectedResult(string siteLabel, bool expected)
        {
            // Arrange
            var customer = new AlphaPlus.Features.Messages.API.Models.Customer.Customer();
            customer.SiteLabel = siteLabel;

            // Act
            var result = customer.IsIMBC;

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void IsIMBC_WithNullSiteLabel_ThrowsNullReferenceException()
        {
            // Arrange
            var customer = new AlphaPlus.Features.Messages.API.Models.Customer.Customer();
            customer.SiteLabel = null;

            // Act & Assert
            Assert.Throws<NullReferenceException>(() => customer.IsIMBC);
        }

        [Fact]
        public void IsAllDayBalTransfer_CanBeSetAndRetrieved()
        {
            // Arrange
            var customer = new AlphaPlus.Features.Messages.API.Models.Customer.Customer();

            // Act
            customer.IsAllDayBalTransfer = true;

            // Assert
            Assert.True(customer.IsAllDayBalTransfer);

            // Act
            customer.IsAllDayBalTransfer = false;

            // Assert
            Assert.False(customer.IsAllDayBalTransfer);
        }

        [Fact]
        public void Enabled_CanBeSetAndRetrieved()
        {
            // Arrange
            var customer = new AlphaPlus.Features.Messages.API.Models.Customer.Customer();

            // Act
            customer.Enabled = false;

            // Assert
            Assert.False(customer.Enabled);

            // Act
            customer.Enabled = true;

            // Assert
            Assert.True(customer.Enabled);
        }

        [Fact]
        public void SiteLabel_CanBeSetAndRetrieved()
        {
            // Arrange
            var customer = new AlphaPlus.Features.Messages.API.Models.Customer.Customer();
            var siteLabel = "TestSite";

            // Act
            customer.SiteLabel = siteLabel;

            // Assert
            Assert.Equal(siteLabel, customer.SiteLabel);
        }

        [Fact]
        public void Properties_AreIndependent()
        {
            // Arrange
            var customer = new AlphaPlus.Features.Messages.API.Models.Customer.Customer();

            // Act
            customer.CustId = 1001;
            customer.UserName = "user1";
            customer.RoleId = 2;
            customer.CurrencyId = 1;
            customer.Site = "site1";
            customer.SiteLabel = "LABEL1";
            customer.IsAllDayBalTransfer = true;
            customer.Enabled = false;

            // Assert
            Assert.Equal(1001, customer.CustId);
            Assert.Equal("user1", customer.UserName);
            Assert.Equal(2, customer.RoleId);
            Assert.Equal(1, customer.CurrencyId);
            Assert.Equal("site1", customer.Site);
            Assert.Equal("LABEL1", customer.SiteLabel);
            Assert.True(customer.IsAllDayBalTransfer);
            Assert.False(customer.Enabled);
        }

        [Fact]
        public void StringProperties_CanBeSetToNull()
        {
            // Arrange
            var customer = new AlphaPlus.Features.Messages.API.Models.Customer.Customer();

            // Act
            customer.UserName = null;
            customer.Site = null;
            customer.SiteLabel = null;

            // Assert
            Assert.Null(customer.UserName);
            Assert.Null(customer.Site);
            Assert.Null(customer.SiteLabel);
            // Note: IsIMBC will throw NullReferenceException when SiteLabel is null
        }

        [Fact]
        public void StringProperties_CanBeSetToEmpty()
        {
            // Arrange
            var customer = new AlphaPlus.Features.Messages.API.Models.Customer.Customer();

            // Act
            customer.UserName = string.Empty;
            customer.Site = string.Empty;
            customer.SiteLabel = string.Empty;

            // Assert
            Assert.Equal(string.Empty, customer.UserName);
            Assert.Equal(string.Empty, customer.Site);
            Assert.Equal(string.Empty, customer.SiteLabel);
        }

        [Fact]
        public void ByteProperties_AcceptFullRange()
        {
            // Arrange
            var customer = new AlphaPlus.Features.Messages.API.Models.Customer.Customer();

            // Act & Assert - Min values
            customer.RoleId = byte.MinValue;
            customer.CurrencyId = byte.MinValue;
            Assert.Equal(byte.MinValue, customer.RoleId);
            Assert.Equal(byte.MinValue, customer.CurrencyId);

            // Act & Assert - Max values
            customer.RoleId = byte.MaxValue;
            customer.CurrencyId = byte.MaxValue;
            Assert.Equal(byte.MaxValue, customer.RoleId);
            Assert.Equal(byte.MaxValue, customer.CurrencyId);
        }

        [Fact]
        public void IntProperties_AcceptExtremeValues()
        {
            // Arrange
            var customer = new AlphaPlus.Features.Messages.API.Models.Customer.Customer();

            // Act & Assert - Min values
            customer.CustId = int.MinValue;
            customer.ARecommendId = int.MinValue;
            customer.MRecommendId = int.MinValue;
            customer.SRecommendId = int.MinValue;
            
            Assert.Equal(int.MinValue, customer.CustId);
            Assert.Equal(int.MinValue, customer.ARecommendId);
            Assert.Equal(int.MinValue, customer.MRecommendId);
            Assert.Equal(int.MinValue, customer.SRecommendId);

            // Act & Assert - Max values
            customer.CustId = int.MaxValue;
            customer.ARecommendId = int.MaxValue;
            customer.MRecommendId = int.MaxValue;
            customer.SRecommendId = int.MaxValue;
            
            Assert.Equal(int.MaxValue, customer.CustId);
            Assert.Equal(int.MaxValue, customer.ARecommendId);
            Assert.Equal(int.MaxValue, customer.MRecommendId);
            Assert.Equal(int.MaxValue, customer.SRecommendId);
        }

        [Fact]
        public void UserName_AcceptsSpecialCharacters()
        {
            // Arrange
            var customer = new AlphaPlus.Features.Messages.API.Models.Customer.Customer();
            var specialUserName = "user@example.com_123!";

            // Act
            customer.UserName = specialUserName;

            // Assert
            Assert.Equal(specialUserName, customer.UserName);
        }

        [Fact]
        public void UserName_AcceptsUnicodeCharacters()
        {
            // Arrange
            var customer = new AlphaPlus.Features.Messages.API.Models.Customer.Customer();
            var unicodeUserName = "用户名_José_Müller";

            // Act
            customer.UserName = unicodeUserName;

            // Assert
            Assert.Equal(unicodeUserName, customer.UserName);
        }

        [Fact]
        public void Site_AcceptsValidUrlFormats()
        {
            // Arrange
            var customer = new AlphaPlus.Features.Messages.API.Models.Customer.Customer();
            var sites = new[]
            {
                "http://example.com",
                "https://www.example.com",
                "ftp://files.example.com",
                "example.com",
                "subdomain.example.co.uk"
            };

            // Act & Assert
            foreach (var site in sites)
            {
                customer.Site = site;
                Assert.Equal(site, customer.Site);
            }
        }

        [Fact]
        public void IsIMBC_IsCaseSensitive()
        {
            // Arrange
            var customer = new AlphaPlus.Features.Messages.API.Models.Customer.Customer();

            // Test various cases
            var testCases = new[]
            {
                ("MBC", true),
                ("mbc", false),
                ("Mbc", false),
                ("mBc", false),
                ("mbC", false),
                ("MbC", false),
                ("mBC", false),
                ("MBc", false)
            };

            foreach (var (siteLabel, expectedResult) in testCases)
            {
                // Act
                customer.SiteLabel = siteLabel;
                var result = customer.IsIMBC;

                // Assert
                Assert.Equal(expectedResult, result);
            }
        }

        [Fact]
        public void Properties_HaveCorrectTypes()
        {
            // Arrange
            var customer = new AlphaPlus.Features.Messages.API.Models.Customer.Customer();

            // Assert
            Assert.IsType<int>(customer.CustId);
            Assert.IsType<string>(customer.UserName);
            Assert.IsType<byte>(customer.RoleId);
            Assert.IsType<byte>(customer.CurrencyId);
            Assert.IsType<int>(customer.ARecommendId);
            Assert.IsType<int>(customer.MRecommendId);
            Assert.IsType<int>(customer.SRecommendId);
            Assert.IsType<string>(customer.Site);
            Assert.IsType<bool>(customer.IsIMBC);
            Assert.IsType<bool>(customer.IsAllDayBalTransfer);
            Assert.IsType<bool>(customer.Enabled);
            Assert.IsType<string>(customer.SiteLabel);
        }
    }
}
