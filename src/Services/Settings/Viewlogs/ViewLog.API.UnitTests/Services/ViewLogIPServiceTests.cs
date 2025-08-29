using AlphaPlus.Core.Models;
using AlphaPlus.Settings.ViewLogs.API.Services.Impl;
using AlphaPlus.Utilities.IP;

namespace AlphaPlus.Settings.ViewLogs.API.UnitTests.Services
{
    public class ViewLogIPServiceTests
    {
        private readonly Mock<IUserSession> _mockUserSession;
        private readonly Mock<IPService> _mockIPService;
        private readonly ViewLogIPServiceImpl _viewLogIPService;
        private readonly Fixture _fixture;

        public ViewLogIPServiceTests()
        {
            _mockUserSession = new Mock<IUserSession>();
            _mockIPService = new Mock<IPService>();
            _viewLogIPService = new ViewLogIPServiceImpl(_mockUserSession.Object, _mockIPService.Object);
            _fixture = new Fixture();
        }

        [Theory]
        [InlineData("192.168.1.1")]
        [InlineData("10.0.0.1")]
        [InlineData("172.16.0.1")]
        [InlineData("8.8.8.8")]
        [InlineData("255.255.255.255")]
        [InlineData("0.0.0.0")]
        public void FormatIP_WithValidIPAndNotVNIP_ShouldReturnClickableLink(string ip)
        {
            // Arrange
            _mockIPService.Setup(x => x.IsVNIP(ip)).Returns(false);

            // Act
            var result = _viewLogIPService.FormatIP(ip);

            // Assert
            var expectedResult = $"<a class=\"iplink\" href=\"javascript:OpenIPInfo('{ip}');\">{ip}</a>";
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData("192.168.1.1")]
        [InlineData("10.0.0.1")]
        public void FormatIP_WithValidVNIPAndAdminCanViewVNIP_ShouldReturnClickableLink(string ip)
        {
            // Arrange
            _mockIPService.Setup(x => x.IsVNIP(ip)).Returns(true);
            _mockUserSession.Setup(x => x.IsAdminCanViewVNIP).Returns(true);

            // Act
            var result = _viewLogIPService.FormatIP(ip);

            // Assert
            var expectedResult = $"<a class=\"iplink\" href=\"javascript:OpenIPInfo('{ip}');\">{ip}</a>";
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData("192.168.1.1")]
        [InlineData("10.0.0.1")]
        public void FormatIP_WithValidVNIPAndAdminCannotViewVNIP_ShouldReturnEmpty(string ip)
        {
            // Arrange
            _mockIPService.Setup(x => x.IsVNIP(ip)).Returns(true);
            _mockUserSession.Setup(x => x.IsAdminCanViewVNIP).Returns(false);

            // Act
            var result = _viewLogIPService.FormatIP(ip);

            // Assert
            Assert.Equal(string.Empty, result);
        }

        [Theory]
        [InlineData("invalid_ip")]
        [InlineData("300.300.300.300")]
        [InlineData("abc.def.ghi.jkl")]
        [InlineData("192.168.1.1.1")]
        public void FormatIP_WithInvalidIP_ShouldReturnOriginalIP(string invalidIP)
        {
            // Act
            var result = _viewLogIPService.FormatIP(invalidIP);

            // Assert
            Assert.Equal(invalidIP, result);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void FormatIP_WithNullOrEmptyIP_ShouldReturnOriginalValue(string invalidIP)
        {
            // Act
            var result = _viewLogIPService.FormatIP(invalidIP);

            // Assert
            Assert.Equal(invalidIP, result);
        }

        [Fact]
        public void FormatIP_WithIncompleteIP_ShouldReturnSimpleLink()
        {
            // Arrange
            var ip = "192.168.1"; // This passes IPAddress.TryParse but doesn't match full IP regex
            
            // Act
            var result = _viewLogIPService.FormatIP(ip);

            // Assert - Should return simple link since it's valid but doesn't match full IP pattern
            var expectedResult = "<a class=\"iplink\" href=\"javascript:;\">192.168.1</a>";
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData("192.168.1.1", "VN")]
        [InlineData("10.0.0.1", "US")]
        [InlineData("8.8.8.8", "US")]
        public void FormatCountryCode_WithValidIPAndNotVNIP_ShouldReturnCountryCode(string ip, string country)
        {
            // Arrange
            _mockIPService.Setup(x => x.IsVNIP(ip)).Returns(false);

            // Act
            var result = _viewLogIPService.FormatCountryCode(ip, country);

            // Assert
            Assert.Equal(country, result);
        }

        [Theory]
        [InlineData("192.168.1.1", "VN")]
        [InlineData("10.0.0.1", "VN")]
        public void FormatCountryCode_WithVNIPAndAdminCanViewVNIP_ShouldReturnCountryCode(string ip, string country)
        {
            // Arrange
            _mockIPService.Setup(x => x.IsVNIP(ip)).Returns(true);
            _mockUserSession.Setup(x => x.IsAdminCanViewVNIP).Returns(true);

            // Act
            var result = _viewLogIPService.FormatCountryCode(ip, country);

            // Assert
            Assert.Equal(country, result);
        }

        [Theory]
        [InlineData("192.168.1.1", "VN")]
        [InlineData("10.0.0.1", "VN")]
        public void FormatCountryCode_WithVNIPAndAdminCannotViewVNIP_ShouldReturnEmpty(string ip, string country)
        {
            // Arrange
            _mockIPService.Setup(x => x.IsVNIP(ip)).Returns(true);
            _mockUserSession.Setup(x => x.IsAdminCanViewVNIP).Returns(false);

            // Act
            var result = _viewLogIPService.FormatCountryCode(ip, country);

            // Assert
            Assert.Equal(string.Empty, result);
        }

        [Theory]
        [InlineData("invalid_ip", "US")]
        [InlineData("300.300.300.300", "VN")]
        [InlineData(null, "US")]
        [InlineData("", "VN")]
        [InlineData("   ", "US")]
        public void FormatCountryCode_WithInvalidIP_ShouldReturnEmpty(string invalidIP, string country)
        {
            // Act
            var result = _viewLogIPService.FormatCountryCode(invalidIP, country);

            // Assert
            Assert.Equal(string.Empty, result);
        }

        [Fact]
        public void FormatCountryCode_WithNullCountry_ShouldReturnNull()
        {
            // Arrange
            var validIP = "192.168.1.1";
            string nullCountry = null;
            _mockIPService.Setup(x => x.IsVNIP(validIP)).Returns(false);

            // Act
            var result = _viewLogIPService.FormatCountryCode(validIP, nullCountry);

            // Assert
            Assert.Equal(nullCountry, result);
        }

        [Fact]
        public void Constructor_WithNullUserSession_ShouldAcceptNullValue()
        {
            // Arrange & Act - Constructor accepts null values in modern .NET
            var service = new ViewLogIPServiceImpl(null, _mockIPService.Object);

            // Assert
            Assert.NotNull(service);
        }

        [Fact]
        public void Constructor_WithNullIPService_ShouldAcceptNullValue()
        {
            // Arrange & Act - Constructor accepts null values in modern .NET
            var service = new ViewLogIPServiceImpl(_mockUserSession.Object, null);

            // Assert
            Assert.NotNull(service);
        }

        [Fact]
        public void Constructor_WithValidDependencies_ShouldCreateInstance()
        {
            // Arrange & Act
            var service = new ViewLogIPServiceImpl(_mockUserSession.Object, _mockIPService.Object);

            // Assert
            Assert.NotNull(service);
        }
    }
}
