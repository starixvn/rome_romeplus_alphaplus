using AlphaPlus.PartialUM.Services;
using AlphaPlus.Reports.Features.MatchWinLossDetail;
using AlphaPlus.Reports.Features.Products;
using AlphaPlus.Reports.Features.Shared;
using Fanex.Data.Repository;
using Microsoft.Extensions.Caching.Memory;

namespace AlphaPlus.Reports.UnitTests.Features.MatchWinLossDetail
{
    [Trait("Category", "MatchWinLossDetail")]
    public class MatchWinLossDetailServiceTest
    {
        private readonly Mock<IDynamicRepository> repositoryMock;
        private readonly Mock<IMemoryCache> cacheMock;
        private readonly Mock<ILeagueService> leagueServiceMock;
        private readonly Mock<IReportDateService> reportDateServiceMock;
        private readonly Mock<ICacheRefService> cacheRefServiceMock;
        private readonly Mock<IPartialUMService> partialUMServiceMock;
        private readonly MatchWinLossDetailService service;

        public MatchWinLossDetailServiceTest()
        {
            repositoryMock = new Mock<IDynamicRepository>();
            cacheMock = new Mock<IMemoryCache>();
            leagueServiceMock = new Mock<ILeagueService>();
            reportDateServiceMock = new Mock<IReportDateService>();
            cacheRefServiceMock = new Mock<ICacheRefService>();
            partialUMServiceMock = new Mock<IPartialUMService>();

            service = new MatchWinLossDetailService(
                repositoryMock.Object,
                cacheMock.Object,
                leagueServiceMock.Object,
                reportDateServiceMock.Object,
                cacheRefServiceMock.Object,
                partialUMServiceMock.Object);

            // Setup cache mock for GetProducts method
            SetupCache();
        }

        private void SetupCache()
        {
            var cacheEntryMock = new Mock<ICacheEntry>();
            cacheMock
                .Setup(c => c.CreateEntry(It.IsAny<object>()))
                .Returns(cacheEntryMock.Object);
        }

        [Fact]
        public async Task GetProducts_Should_Return_Products_From_Cache_Or_Repository()
        {
            // Arrange
            var products = new List<Product>
            {
                new() { Id = 1 },
                new() { Id = 2 },
                new() { Id = 3 }
            };

            object cachedProducts = null;

            cacheMock
                .Setup(c => c.TryGetValue(It.IsAny<object>(), out cachedProducts))
                .Returns(false);

            repositoryMock
                .Setup(r => r.FetchAsync<Product>(It.IsAny<ProductsForMatchWinLossDetailCriteria>(), string.Empty))
                .ReturnsAsync(products);

            // Act
            var result = await service.GetProducts();

            // Assert
            Assert.Equal(products.Select(p => p.Id).ToArray(), result);
            repositoryMock.Verify(r => r.FetchAsync<Product>(It.IsAny<ProductsForMatchWinLossDetailCriteria>(), string.Empty), Times.Once);
        }

        [Fact]
        public async Task GetLeagues_Should_Return_Expected_Leagues()
        {
            // Arrange
            var param = new GetLeaguesParam(
                CustId: 123,
                ProductIds: "1,2,3",
                FromDate: DateTime.Now.AddDays(-7),
                ToDate: DateTime.Now,
                Language: "en-US",
                LanguageKey: "en",
                IsShowAll: false
            );

            var moveBetsDay = DateTime.Now.AddDays(-1);
            var leagues = new List<League>
            {
                new() { LeagueId = 1, LeagueType = 1, LeagueGroupId = 10 },
                new() { LeagueId = 2, LeagueType = 1, LeagueGroupId = 20 }
            };

            reportDateServiceMock
                .Setup(s => s.GetMoveBetsDay())
                .ReturnsAsync(moveBetsDay);

            repositoryMock
                .Setup(r => r.FetchAsync<League>(It.IsAny<GetLeaguesCriteria>(), string.Empty))
                .ReturnsAsync(leagues);

            leagueServiceMock
                .Setup(s => s.GetLeagueName(It.IsAny<int>(), It.IsAny<byte>(), It.IsAny<int>()))
                .Returns<int, byte, int>((id, type, groupId) => $"League_{id}_{type}_{groupId}");

            // Act
            var result = await service.GetLeagues(param);

            // Assert
            Assert.Equal(leagues.Count, result.Count());
            foreach (var league in leagues)
            {
                Assert.Contains(result, l => l.LeagueId == league.LeagueId &&
                                             l.Name == $"League_{league.LeagueId}_{league.LeagueType}_{league.LeagueGroupId}");
            }

            reportDateServiceMock.Verify(s => s.GetMoveBetsDay(), Times.Once);
            repositoryMock.Verify(r => r.FetchAsync<League>(It.Is<GetLeaguesCriteria>(c =>
                c.CustId == param.CustId &&
                c.ProductIds == param.ProductIds &&
                c.FromDate == param.FromDate &&
                c.ToDate == param.ToDate &&
                c.MoveBetsDay == moveBetsDay &&
                c.Language == param.LanguageKey &&
                c.IsShowAll == param.IsShowAll), string.Empty), Times.Once);
        }

        [Fact]
        public async Task GetMatchWinLossDetail_Should_Return_Expected_Report()
        {
            // Arrange
            var param = new MatchWinLossDetailParam(
                ViewerId: 123,
                ViewerLevelId: 4,
                FromDate: DateTime.Now.AddDays(-7),
                ToDate: DateTime.Now,
                ProductIds: "1,2,3",
                LeagueIds: "10,20",
                League3rdIds: "30,40",
                SiteName: "TestSite"
            );

            var moveBetsDay = DateTime.Now.AddDays(-1);
            var rawData = new List<MatchWinLossDetailEntity>
            {
                // Normal match
                new() {
                    MatchId = 1001, LeagueId = 10, LeagueType = 0, LeagueGroupId = 100,
                    HomeId = 201, AwayId = 202, BetType = 1, BetGroup = 1, MemberWinLoss = 50
                },
                new() {
                    MatchId = 1001, LeagueId = 10, LeagueType = 0, LeagueGroupId = 100,
                    HomeId = 201, AwayId = 202, BetType = 2, BetGroup = 2, MemberWinLoss = -30
                },
                // Mix parlay
                new() {
                    BetType = BetTypeConstants.MixParlay, BetGroup = 3, MemberWinLoss = 20
                },
                // System parlay
                new() {
                    BetType = BetTypeConstants.CombineMixParlay, BetGroup = 4, MemberWinLoss = 30
                },
                // Parlay match win loss detail
                new() {
                    BetType = BetTypeConstants.LeapGamingMin + 1, BetGroup = 4, MemberWinLoss = 10
                }
            };

            reportDateServiceMock
                .Setup(s => s.GetMoveBetsDay())
                .ReturnsAsync(moveBetsDay);

            repositoryMock
                .Setup(r => r.FetchAsync<MatchWinLossDetailEntity>(It.IsAny<MatchWinLossDetailCriteria>(), string.Empty))
                .ReturnsAsync(rawData);

            leagueServiceMock
                .Setup(s => s.GetLeagueName(It.IsAny<int>(), It.IsAny<byte>(), It.IsAny<int>()))
                .Returns<int, byte, int>((id, type, groupId) => $"League_{id}_{type}_{groupId}");

            cacheRefServiceMock
                .Setup(s => s.GetBetTypeName(It.IsAny<int>()))
                .Returns<int>(bt => $"BetType_{bt}");

            cacheRefServiceMock
                .Setup(s => s.GetTeamName(It.IsAny<int>()))
                .Returns<int>(id => $"Team_{id}");

            // Act
            var result = await service.GetMatchWinLossDetail(param);

            // Assert
            Assert.True(result.HasData);

            // Check normal match details
            Assert.Single(result.NormalMatchWinLoss);
            var normalMatch = result.NormalMatchWinLoss.First();
            Assert.Equal(2, normalMatch.BetTypeWinLoss.Count());
            Assert.Equal("League_10_0_100", normalMatch.LeagueName);
            Assert.Equal("Team_201", normalMatch.HomeName);
            Assert.Equal("Team_202", normalMatch.AwayName);
            Assert.Equal(20, normalMatch.Total.MemberWinLoss); // 50 - 30

            // Check mix parlay
            Assert.NotNull(result.MixParlay);
            Assert.Equal(20, result.MixParlay.MemberWinLoss);

            // Check system parlay
            Assert.NotNull(result.SystemParlay);
            Assert.Equal(30, result.SystemParlay.MemberWinLoss);

            // Check parlay match details
            Assert.Single(result.ParlayMatchWinLoss);
            Assert.Equal(10, result.ParlayMatchWinLoss.First().MemberWinLoss);

            // Check grand total
            Assert.Equal(80, result.GrandTotal.MemberWinLoss); // 50 - 30 + 20 + 30 + 10

            partialUMServiceMock.Verify(s => s.ThrowIfUM(It.IsAny<string>(), param.SiteName), Times.Once);
            reportDateServiceMock.Verify(s => s.GetMoveBetsDay(), Times.Once);
            repositoryMock.Verify(r => r.FetchAsync<MatchWinLossDetailEntity>(It.Is<MatchWinLossDetailCriteria>(c =>
                c.CustId == param.ViewerId &&
                c.FromDate == param.FromDate &&
                c.ToDate == param.ToDate &&
                c.MoveBetsDay == moveBetsDay &&
                c.ProductIds == param.ProductIds &&
                c.LeagueIds == param.LeagueIds &&
                c.League3rdIds == param.League3rdIds), string.Empty), Times.Once);
        }

        [Fact]
        public async Task GetMatchWinLossDetail_Should_Return_Empty_Report_When_No_Data()
        {
            // Arrange
            var param = new MatchWinLossDetailParam(
                ViewerId: 123,
                ViewerLevelId: 4,
                FromDate: DateTime.Now.AddDays(-7),
                ToDate: DateTime.Now,
                ProductIds: "1,2,3",
                LeagueIds: "10,20",
                League3rdIds: "30,40",
                SiteName: "TestSite"
            );

            var moveBetsDay = DateTime.Now.AddDays(-1);
            var rawData = new List<MatchWinLossDetailEntity>();

            reportDateServiceMock
                .Setup(s => s.GetMoveBetsDay())
                .ReturnsAsync(moveBetsDay);

            repositoryMock
                .Setup(r => r.FetchAsync<MatchWinLossDetailEntity>(It.IsAny<MatchWinLossDetailCriteria>(), string.Empty))
                .ReturnsAsync(rawData);

            // Act
            var result = await service.GetMatchWinLossDetail(param);

            // Assert
            Assert.False(result.HasData);
            Assert.Empty(result.NormalMatchWinLoss);
            Assert.Null(result.MixParlay);
            Assert.Null(result.SystemParlay);
            Assert.Empty(result.ParlayMatchWinLoss);
        }
    }
}