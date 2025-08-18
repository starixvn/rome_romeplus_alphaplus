using System.Threading.Tasks;
using AlphaPlus.Utilities.IP.Criteria;
using AlphaPlus.Utilities.IP.Models;
using AutoFixture;
using Fanex.Data.Repository;
using Microsoft.Extensions.Caching.Memory;

namespace AlphaPlus.Utilities.IP.UnitTests
{
    public class IPServiceTests
    {
        private readonly Mock<IDynamicRepository> dynamicRepositoryMock;
        private readonly Mock<IPHelper> ipHelperMock;
        private readonly Mock<IMemoryCache> memoryCacheMock;
        private readonly IPServiceImpl ipService;

        public IPServiceTests()
        {
            dynamicRepositoryMock = new Mock<IDynamicRepository>();
            ipHelperMock = new Mock<IPHelper>();
            memoryCacheMock = new Mock<IMemoryCache>();
            ipService = new IPServiceImpl(dynamicRepositoryMock.Object, ipHelperMock.Object, memoryCacheMock.Object);
        }

        [Fact]
        public void IsVNIP_ReturnsFalse_WhenIpIsNullOrEmpty()
        {
            Assert.False(ipService.IsVNIP(null));
            Assert.False(ipService.IsVNIP(""));
        }

        [Fact]
        public void FormatIP_ReturnsInvalidFormat_WhenIPIsNull()
        {
            // Act
            var result = ipService.FormatIP(null, true);

            // Assert
            Assert.Null(result.IP);
            Assert.False(result.AllowOpenInfoPopup);
        }

        [Fact]
        public void FormatIP_ReturnsInvalidFormat_WhenIPIsEmpty()
        {
            // Act
            var result = ipService.FormatIP("", true);

            // Assert
            Assert.Equal("", result.IP);
            Assert.False(result.AllowOpenInfoPopup);
        }

        [Fact]
        public void FormatIP_ReturnsInvalidFormat_WhenIPIsInvalid()
        {
            // Act
            var result = ipService.FormatIP("invalid-ip", true);

            // Assert
            Assert.Equal("invalid-ip", result.IP);
            Assert.False(result.AllowOpenInfoPopup);
        }

        [Fact]
        public void FormatIP_ReturnsInvalidFormat_WhenIPDoesNotMatchRegex()
        {
            // Act
            var result = ipService.FormatIP("256.256.256.256", true);

            // Assert
            Assert.Equal("256.256.256.256", result.IP);
            Assert.False(result.AllowOpenInfoPopup);
        }

        [Fact]
        public async Task GetIPInformation_ReturnsModel()
        {
            var fixture = new Fixture();
            var ip = "1.2.3.4";
            var model = fixture.Create<IPInformationModel>();
            dynamicRepositoryMock.Setup(x => x.GetAsync<IPInformationModel>(It.IsAny<ICriteria>(), It.IsAny<string>())).ReturnsAsync(model);
            var result = await ipService.GetIPInformation(ip);
            Assert.Equal(model.CountryLong, result.CountryLong);
        }

        [Fact]
        public async Task GetIPInformation_CallsRepositoryWithCorrectCriteria()
        {
            // Arrange
            var ip = "1.2.3.4";
            var fixture = new Fixture();
            var model = fixture.Create<IPInformationModel>();
            dynamicRepositoryMock.Setup(x => x.GetAsync<IPInformationModel>(It.IsAny<ICriteria>(), It.IsAny<string>())).ReturnsAsync(model);

            // Act
            await ipService.GetIPInformation(ip);

            // Assert
            dynamicRepositoryMock.Verify(x => x.GetAsync<IPInformationModel>(
                It.Is<ICriteria>(c => c is GetIPInformationCriteria && ((GetIPInformationCriteria)c).Ip == ip),
                It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task GetCountryCode_ReturnsUnknown_WhenNull()
        {
            dynamicRepositoryMock.Setup(x => x.GetAsync<IPInformationModel>(It.IsAny<ICriteria>(), It.IsAny<string>())).ReturnsAsync((IPInformationModel)null);
            var result = await ipService.GetCountryCode("1.2.3.4");
            Assert.Equal("Unknown", result);
        }

        [Fact]
        public async Task GetCountryCode_ReturnsNull_WhenReserved()
        {
            var fixture = new Fixture();
            var reservedModel = fixture.Build<IPInformationModel>().With(x => x.CountryLong, "Reserved").Create();
            dynamicRepositoryMock.Setup(x => x.GetAsync<IPInformationModel>(It.IsAny<ICriteria>(), It.IsAny<string>())).ReturnsAsync(reservedModel);
            var result = await ipService.GetCountryCode("1.2.3.4");
            Assert.Null(result);
        }

        [Fact]
        public async Task GetCountryCode_ReturnsCountryLong()
        {
            var fixture = new Fixture();
            var model = fixture.Build<IPInformationModel>().With(x => x.CountryLong, "Vietnam").Create();
            dynamicRepositoryMock.Setup(x => x.GetAsync<IPInformationModel>(It.IsAny<ICriteria>(), It.IsAny<string>())).ReturnsAsync(model);
            var result = await ipService.GetCountryCode("1.2.3.4");
            Assert.Equal("Vietnam", result);
        }
    }
}