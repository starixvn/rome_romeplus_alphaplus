// using AlphaPlus.BetLists.Shared.Helpers;
// using AlphaPlus.BetLists.Shared.Models;
// using AlphaPlus.BetLists.Shared.Services;
// using AlphaPlus.BetLists.WinLossByMatch;
// using AlphaPlus.IPInfo.Services;
// using AlphaPlus.PartialUM.Services;
// using AlphaPlus.Utilities.IP;
// using Fanex.Data.Repository;
// using System.Threading.Tasks;

// namespace AlphaPlus.BetLists.UnitTests.WinLossByMatch;

// public class WinLossByMatchServiceTests
// {
//     private readonly Mock<IReportDateService> _reportDateServiceMock;
//     private readonly Mock<IPartialUMService> _partialUMServiceMock;
//     private readonly Mock<IDynamicRepository> _repositoryMock;
//     private readonly Mock<IIPInfoService> _ipInfoServiceMock;
//     private readonly Mock<IPService> _ipServiceMock;
//     private readonly WinLossByMatchService _service;

//     public WinLossByMatchServiceTests()
//     {
//         _reportDateServiceMock = new Mock<IReportDateService>();
//         _partialUMServiceMock = new Mock<IPartialUMService>();
//         _repositoryMock = new Mock<IDynamicRepository>();
//         _ipInfoServiceMock = new Mock<IIPInfoService>();
//         _ipServiceMock = new Mock<IPService>();

//         _service = new WinLossByMatchService(
//             _reportDateServiceMock.Object,
//             _partialUMServiceMock.Object,
//             _repositoryMock.Object,
//             _ipInfoServiceMock.Object,
//             _ipServiceMock.Object);
//     }

//     [Fact]
//     public async Task GetBetList_ShouldUseCorrectCriteria_WhenRegularMatch()
//     {
//         // Arrange
//         var param = new WinLossByMatchParam
//         {
//             CustId = 123,
//             FromDate = new DateTime(2025, 5, 1),
//             ToDate = new DateTime(2025, 5, 2),
//             MatchId = 456,
//             BetType = 1,
//             LeagueType = "Soccer",
//             CustName = "Test",
//             LiveIndicator = 0,
//             CustLevelId = 7,
//             SiteName = "TestSite",
//             DisplayVNIP = false
//         };

//         var expectedMoveBetsDay = 1;
//         _reportDateServiceMock.Setup(x => x.GetMoveBetsDay()).ReturnsAsync(expectedMoveBetsDay);

//         _repositoryMock.Setup(x => x.FetchAsync<TicketDto>(It.IsAny<ICriteria>()))
//             .ReturnsAsync(new List<TicketDto>());

//         // Act
//         await _service.GetBetList(param);

//         // Assert
//         _repositoryMock.Verify(x => x.FetchAsync<TicketDto>(
//             It.Is<ICriteria>(c => c is GetWinLossByMatchCriteria)), Times.Once);

//         _partialUMServiceMock.Verify(x => x.ThrowIfUM(
//             It.IsAny<string>(),
//             It.Is<string>(s => s == param.SiteName)), Times.Once);
//     }

//     [Fact]
//     public async Task GetBetList_ShouldUseCorrectCriteria_WhenOutrightMatch()
//     {
//         // Arrange
//         var param = new WinLossByMatchParam
//         {
//             CustId = 123,
//             FromDate = new DateTime(2025, 5, 1),
//             ToDate = new DateTime(2025, 5, 2),
//             LeagueId = 789,
//             BetType = 6, // Outright type
//             Language = "en",
//             CustLevelId = 7,
//             SiteName = "TestSite",
//             DisplayVNIP = false
//         };

//         var expectedMoveBetsDay = 1;
//         _reportDateServiceMock.Setup(x => x.GetMoveBetsDay()).ReturnsAsync(expectedMoveBetsDay);

//         // Setup BetTypeHelper to recognize this as an outright bet
//         // Note: This might need additional setup depending on the implementation

//         _repositoryMock.Setup(x => x.FetchAsync<TicketDto>(It.IsAny<ICriteria>()))
//             .ReturnsAsync(new List<TicketDto>());

//         // Act
//         await _service.GetBetList(param);

//         // Assert
//         _repositoryMock.Verify(x => x.FetchAsync<TicketDto>(
//             It.Is<ICriteria>(c => c is GetWinLossOutrightCriteria)), Times.Once);

//         _partialUMServiceMock.Verify(x => x.ThrowIfUM(
//             It.IsAny<string>(),
//             It.Is<string>(s => s == param.SiteName)), Times.Once);
//     }
// }