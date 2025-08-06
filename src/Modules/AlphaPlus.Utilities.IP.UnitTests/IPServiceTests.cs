using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AlphaPlus.Utilities.IP;
using AlphaPlus.Utilities.IP.Model;
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

        private void SetupVNIPCache(List<Ipv4Range> ipv4Ranges, List<Ipv6Range> ipv6Ranges, ulong? ipv6Number = null)
        {
            var cacheEntryMock = new Mock<ICacheEntry>();
            memoryCacheMock.Setup(m => m.GetOrCreate(It.IsAny<object>(), It.IsAny<Func<ICacheEntry, object>>()))
                .Returns((object key, Func<ICacheEntry, object> factory) => factory(cacheEntryMock.Object));
            if (ipv6Ranges.Any() && ipv6Number.HasValue)
            {
                foreach (var range in ipv6Ranges)
                {
                    range.GetType().GetProperty("From")?.SetValue(range, ipv6Number.Value.ToString());
                    range.GetType().GetProperty("To")?.SetValue(range, (ipv6Number.Value + 10).ToString());
                }
            }
            dynamicRepositoryMock
                .Setup(x => x.FetchMultiple<Ipv4Range, Ipv6Range>(It.IsAny<ICriteria>(), It.IsAny<string>()))
                .Returns((ipv4Ranges, ipv6Ranges));
        }
    }
}