using AlphaPlus.Main.Site.Features.IPInfo;
using AlphaPlus.Utilities.IP;
using AlphaPlus.Utilities.IP.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace AlphaPlus.Main.Site.UnitTests.Features.IPInfo
{
    public class IPInfoControllerTests
    {
        private readonly Mock<IPService> _ipServiceMock;
        private readonly IPInfoController _controller;

        public IPInfoControllerTests()
        {
            _ipServiceMock = new Mock<IPService>();
            _controller = new IPInfoController(_ipServiceMock.Object);
        }

        [Theory]
        [InlineData("192.168.1.1")]
        [InlineData("10.0.0.1")]
        [InlineData("172.16.0.1")]
        public async Task Index_WithValidIP_ReturnsViewWithModel(string validIp)
        {
            // Arrange
            var ipInfo = new IPInformationModel
            {
                CountryShort = "US",
                CountryLong = "United States",
                IpRegion = "California",
                IpCity = "San Francisco"
            };
            _ipServiceMock.Setup(x => x.GetIPInformation(validIp))
                .ReturnsAsync(ipInfo);

            // Act
            var result = await _controller.Index(validIp);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<IPInfoViewModel>(viewResult.Model);
            Assert.Equal(validIp, model.IP);
            Assert.True(model.IsValid);
            Assert.NotNull(model.IPInfo);
            Assert.Equal(ipInfo.CountryShort, model.IPInfo.CountryShort);
            Assert.Equal(ipInfo.CountryLong, model.IPInfo.CountryLong);
            Assert.Equal(ipInfo.IpRegion, model.IPInfo.IpRegion);
            Assert.Equal(ipInfo.IpCity, model.IPInfo.IpCity);
        }

        [Theory]
        [InlineData("192.168.1.1")]
        [InlineData("10.0.0.1")]
        public async Task Index_WithValidIPButNoInfoFound_ReturnsViewWithValidIPButNoInfo(string validIp)
        {
            // Arrange
            _ipServiceMock.Setup(x => x.GetIPInformation(validIp))
                .ReturnsAsync((IPInformationModel)null);

            // Act
            var result = await _controller.Index(validIp);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<IPInfoViewModel>(viewResult.Model);
            Assert.Equal(validIp, model.IP);
            Assert.False(model.IsValid);
            Assert.Null(model.IPInfo);
        }

        [Theory]
        [InlineData("invalid.ip")]
        [InlineData("256.256.256.256")]
        [InlineData("")]
        [InlineData(null)]
        public async Task Index_WithInvalidIP_ReturnsViewWithInvalidModel(string invalidIp)
        {
            // Act
            var result = await _controller.Index(invalidIp);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<IPInfoViewModel>(viewResult.Model);
            Assert.Equal(invalidIp, model.IP);
            Assert.False(model.IsValid);
            Assert.Null(model.IPInfo);

            // Verify that the service was never called
            _ipServiceMock.Verify(x => x.GetIPInformation(It.IsAny<string>()), Times.Never);
        }
    }
}