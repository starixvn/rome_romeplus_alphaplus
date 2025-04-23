// using AlphaPlus.PartialUM.Services;
// using AlphaPlus.Reports.Features.BetCreditTracking.MoveStatementHistory;
// using Fanex.Data.Repository;

// namespace AlphaPlus.Reports.UnitTests.Features.BetCreditTracking.MoveStatementHistory;

// [Trait("Category", "MoveStatementHistory")]
// public class MoveStatementHistoryServiceTests
// {
//     private readonly Mock<IDynamicRepository> repositoryMock = new();
//     private readonly Mock<IPartialUMService> partialUMServiceMock = new();
//     private readonly MoveStatementHistoryService service;

//     private readonly MoveStatementHistoryParam testParam = new(
//         Username: "testUser",
//         FromDate: new DateTime(2023, 1, 1),
//         ToDate: new DateTime(2023, 1, 31),
//         ViewerId: 123,
//         PageIndex: 1,
//         PageSize: 10,
//         IsVariance: false,
//         SiteName: "TestSite"
//     );

//     public MoveStatementHistoryServiceTests()
//     {
//         service = new MoveStatementHistoryService(
//             repositoryMock.Object,
//             partialUMServiceMock.Object);
//     }

//     [Fact]
//     public async Task GetMoveStatementHistory_Should_Call_PartialUMService_With_CorrectSettingKey()
//     {
//         // Arrange
//         SetupMocks();

//         // Act
//         await service.GetMoveStatementHistory(testParam);

//         // Assert
//         partialUMServiceMock.Verify(
//             s => s.ThrowIfUM(It.IsAny<string>(), testParam.SiteName),
//             Times.Once);
//     }

//     [Fact]
//     public async Task GetMoveStatementHistory_Should_Call_Repository_With_CorrectCriteria()
//     {
//         // Arrange
//         SetupMocks();

//         // Act
//         await service.GetMoveStatementHistory(testParam);

//         // Assert
//         repositoryMock.Verify(
//             r => r.FetchMultipleAsync<MoveStatementHistoryByRefNo, MoveStatementHistoryByRefNo, MoveStatementHistorySummary>(
//                 It.Is<MoveStatementHistoryCriteria>(c =>
//                     c.Username == testParam.Username &&
//                     c.FromDate == testParam.FromDate &&
//                     c.ToDate == testParam.ToDate &&
//                     c.LoggedId == testParam.ViewerId &&
//                     c.PageIndex == testParam.PageIndex &&
//                     c.PageSize == testParam.PageSize &&
//                     c.IsVariance == testParam.IsVariance), string.Empty),
//             Times.Once);
//     }

//     [Fact]
//     public async Task GetMoveStatementHistory_Should_Return_Empty_Data_When_No_Records()
//     {
//         // Arrange
//         SetupMocks(new List<MoveStatementHistoryByRefNo>(), new List<MoveStatementHistoryByRefNo>(), new List<MoveStatementHistorySummary>(), 0);

//         // Act
//         var result = await service.GetMoveStatementHistory(testParam);

//         // Assert
//         Assert.Empty(result.ReportData);
//         Assert.NotNull(result.Summary);
//         Assert.Equal(0, result.TotalRows);
//     }

//     [Fact]
//     public async Task GetMoveStatementHistory_Should_Group_Data_By_Member_When_Records_Exist()
//     {
//         // Arrange
//         var moveStatementHistories = new List<MoveStatementHistoryByRefNo>
//         {
//             new() { CustId = 1, Username = "user1", MemberWinLoss = 100 },
//             new() { CustId = 1, Username = "user1", MemberWinLoss = 200 },
//             new() { CustId = 2, Username = "user2", MemberWinLoss = 300 }
//         };

//         var totalMoveStatementHistories = new List<MoveStatementHistoryByRefNo>
//         {
//             new() { CustId = 1, Username = "user1", MemberWinLoss = 300 },
//             new() { CustId = 2, Username = "user2", MemberWinLoss = 300 }
//         };

//         var summaries = new List<MoveStatementHistorySummary>
//         {
//             new() { MemberWinLoss = 600 }
//         };

//         SetupMocks(moveStatementHistories, totalMoveStatementHistories, summaries, 2);

//         // Act
//         var result = await service.GetMoveStatementHistory(testParam);

//         // Assert
//         Assert.Equal(2, result.ReportData.Count());
//         Assert.Equal(2, result.TotalRows);
//         Assert.Equal(600, result.Summary.MemberWinLoss);

//         var user1Data = result.ReportData.FirstOrDefault(d => d.MemberRow.Username == "user1");
//         Assert.NotNull(user1Data);
//         Assert.Equal(300, user1Data.TotalRow.TotalAmount);
//         Assert.Equal(2, user1Data.DetailRows.Count());

//         var user2Data = result.ReportData.FirstOrDefault(d => d.MemberRow.Username == "user2");
//         Assert.NotNull(user2Data);
//         Assert.Equal(300, user2Data.TotalRow.TotalAmount);
//         Assert.Single(user2Data.DetailRows);
//     }

//     [Fact]
//     public async Task GetMoveStatementHistory_Should_Handle_Missing_TotalRow_For_Member()
//     {
//         // Arrange
//         var moveStatementHistories = new List<MoveStatementHistoryByRefNo>
//         {
//             new() { CustId = 1, Username = "user1", Amount = 100 },
//             new() { CustId = 3, Username = "user3", Amount = 400 } // No matching total row
//         };

//         var totalMoveStatementHistories = new List<MoveStatementHistoryByRefNo>
//         {
//             new() { CustId = 1, Username = "user1", TotalAmount = 100 }
//             // Missing total for user3
//         };

//         var summaries = new List<MoveStatementHistorySummary>
//         {
//             new() { TotalAmount = 500 }
//         };

//         SetupMocks(moveStatementHistories, totalMoveStatementHistories, summaries, 2);

//         // Act
//         var result = await service.GetMoveStatementHistory(testParam);

//         // Assert
//         Assert.Equal(2, result.ReportData.Count());

//         var user3Data = result.ReportData.FirstOrDefault(d => d.Username == "user3");
//         Assert.NotNull(user3Data);
//         Assert.Equal(0, user3Data.TotalRow.TotalAmount); // Should have default values
//     }

//     private void SetupMocks(
//         List<MoveStatementHistoryByRefNo> moveStatementHistories = null,
//         List<MoveStatementHistoryByRefNo> totalMoveStatementHistories = null,
//         List<MoveStatementHistorySummary> summaries = null,
//         int totalRows = 0)
//     {
//         var criteria = It.IsAny<MoveStatementHistoryCriteria>();

//         // Set TotalRows property
//         repositoryMock
//             .Setup(r => r.FetchMultipleAsync<MoveStatementHistoryByRefNo, MoveStatementHistoryByRefNo, MoveStatementHistorySummary>(criteria, string.Empty))
//             .Callback<object>(c =>
//             {
//                 if (c is MoveStatementHistoryCriteria crit)
//                 {
//                     crit.TotalRows = totalRows;
//                 }
//             })
//             .ReturnsAsync((
//                 moveStatementHistories ?? new List<MoveStatementHistoryByRefNo>(),
//                 totalMoveStatementHistories ?? new List<MoveStatementHistoryByRefNo>(),
//                 summaries ?? new List<MoveStatementHistorySummary>()
//             ));

//         partialUMServiceMock
//             .Setup(s => s.ThrowIfUM(It.IsAny<string>(), It.IsAny<string>()))
//             .Returns(Task.CompletedTask);
//     }
// }