using AlphaPlus.PartialUM.Services;
using AlphaPlus.Reports.Features.BetCreditTracking.MoveStatementHistory;
using Fanex.Data.Repository;

namespace AlphaPlus.Reports.UnitTests.Features.BetCreditTracking.MoveStatementHistory
{
    [Trait("Category", "BetCreditTracking")]
    public class MoveStatementHistoryServiceTest
    {
        private readonly Mock<IDynamicRepository> repositoryMock;
        private readonly Mock<IPartialUMService> partialUMServiceMock;
        private readonly MoveStatementHistoryService service;

        public MoveStatementHistoryServiceTest()
        {
            repositoryMock = new Mock<IDynamicRepository>();
            partialUMServiceMock = new Mock<IPartialUMService>();

            service = new MoveStatementHistoryService(
                repositoryMock.Object,
                partialUMServiceMock.Object);
        }

        [Fact]
        public async Task GetMoveStatementHistory_Should_Return_Expected_Data()
        {
            // Arrange
            var param = new MoveStatementHistoryParam(
                Username: "testUser",
                FromDate: DateTime.Now.AddDays(-7),
                ToDate: DateTime.Now,
                ViewerId: 123,
                PageIndex: 1,
                PageSize: 10,
                IsVariance: false,
                SiteName: "TestSite"
            );

            var moveStatementHistories = new List<MoveStatementHistoryByRefNo>
            {
                new() { CustId = 1, Username = "testUser", TransId = 1, MemberWinLoss = 100 },
                new() { CustId = 1, Username = "testUser", TransId = 2, MemberWinLoss = 50 }
            };

            var totalMoveStatementHistories = new List<MoveStatementHistoryByRefNo>
            {
                new() { CustId = 1, Username = "testUser", MemberWinLoss = 150 }
            };

            var summaryMoveStatementHistories = new List<MoveStatementHistorySummary>
            {
                new() { MemberWinLoss = 150 }
            };

            repositoryMock
                .Setup(r => r.FetchMultipleAsync<MoveStatementHistoryByRefNo, MoveStatementHistoryByRefNo, MoveStatementHistorySummary>(
                    It.IsAny<ICriteria>(), string.Empty))
                .Callback<ICriteria, string>((c, s) =>
                {
                    if (c is MoveStatementHistoryCriteria criteria)
                    {
                        criteria.TotalRows = 2;
                    }
                })
                .ReturnsAsync((moveStatementHistories, totalMoveStatementHistories, summaryMoveStatementHistories));

            // Act
            var result = await service.GetMoveStatementHistory(param);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.TotalRows);
            Assert.Single(result.ReportData);
            Assert.Equal("testUser", result.ReportData.First().MemberRow.Username);
            Assert.Equal(2, result.ReportData.First().RefNoRow.Count());
            Assert.Equal(150, result.Summary.MemberWinLoss);

            partialUMServiceMock.Verify(
                s => s.ThrowIfUM(It.IsAny<string>(), param.SiteName),
                Times.Once);

            repositoryMock.Verify(
                r => r.FetchMultipleAsync<MoveStatementHistoryByRefNo, MoveStatementHistoryByRefNo, MoveStatementHistorySummary>(
                    It.Is<MoveStatementHistoryCriteria>(c =>
                        c.Username == param.Username &&
                        c.FromDate == param.FromDate &&
                        c.ToDate == param.ToDate &&
                        c.LoggedId == param.ViewerId &&
                        c.PageIndex == param.PageIndex &&
                        c.PageSize == param.PageSize &&
                        c.IsVariance == param.IsVariance), string.Empty),
                Times.Once);
        }

        [Fact]
        public async Task GetMoveStatementHistory_Should_Return_Empty_Data_When_No_Records()
        {
            // Arrange
            var param = new MoveStatementHistoryParam(
                Username: "testUser",
                FromDate: DateTime.Now.AddDays(-7),
                ToDate: DateTime.Now,
                ViewerId: 123,
                PageIndex: 1,
                PageSize: 10,
                IsVariance: false,
                SiteName: "TestSite"
            );

            var moveStatementHistories = new List<MoveStatementHistoryByRefNo>();
            var totalMoveStatementHistories = new List<MoveStatementHistoryByRefNo>();
            var summaryMoveStatementHistories = new List<MoveStatementHistorySummary>();

            repositoryMock
                .Setup(r => r.FetchMultipleAsync<MoveStatementHistoryByRefNo, MoveStatementHistoryByRefNo, MoveStatementHistorySummary>(
                    It.IsAny<ICriteria>(), string.Empty))
                .Callback<ICriteria, string>((c, s) =>
                {
                    if (c is MoveStatementHistoryCriteria criteria)
                    {
                        criteria.TotalRows = 0;
                    }
                })
                .ReturnsAsync((moveStatementHistories, totalMoveStatementHistories, summaryMoveStatementHistories));

            // Act
            var result = await service.GetMoveStatementHistory(param);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(0, result.TotalRows);
            Assert.Empty(result.ReportData);
            Assert.NotNull(result.Summary);

            partialUMServiceMock.Verify(
                s => s.ThrowIfUM(It.IsAny<string>(), param.SiteName),
                Times.Once);
        }

        [Fact]
        public async Task GetMoveStatementHistory_Should_Check_PartialUM_Before_Fetching_Data()
        {
            // Arrange
            var param = new MoveStatementHistoryParam(
                Username: "testUser",
                FromDate: DateTime.Now.AddDays(-7),
                ToDate: DateTime.Now,
                ViewerId: 123,
                PageIndex: 1,
                PageSize: 10,
                IsVariance: false,
                SiteName: "TestSite"
            );

            partialUMServiceMock
                .Setup(s => s.ThrowIfUM(It.IsAny<string>(), It.IsAny<string>()))
                .Throws(new Exception("UM Check Failed"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => service.GetMoveStatementHistory(param));

            repositoryMock.Verify(
                r => r.FetchMultipleAsync<MoveStatementHistoryByRefNo, MoveStatementHistoryByRefNo, MoveStatementHistorySummary>(
                    It.IsAny<MoveStatementHistoryCriteria>(), string.Empty),
                Times.Never);
        }
    }
}