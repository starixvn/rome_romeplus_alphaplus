using AlphaPlus.PartialUM.Services;
using AlphaPlus.Reports.Features.Outstanding;
using Fanex.Data.Repository;

namespace AlphaPlus.Reports.UnitTests.Features.Outstanding;

[Trait("Category", "Outstanding")]
public class OutstandingServiceTest
{
    private readonly Mock<IDynamicRepository> repositoryMock;
    private readonly Mock<IPartialUMService> partialUMServiceMock;
    private readonly Mock<IAutoFollowBetService> autoFollowBetServiceMock;
    private readonly OutstandingService service;

    public OutstandingServiceTest()
    {
        repositoryMock = new Mock<IDynamicRepository>();
        partialUMServiceMock = new Mock<IPartialUMService>();
        autoFollowBetServiceMock = new Mock<IAutoFollowBetService>();
        service = new OutstandingService(repositoryMock.Object, partialUMServiceMock.Object, autoFollowBetServiceMock.Object);
    }

    [Fact]
    public async Task GetOutstanding_WithNormalParam_ShouldReturnCorrectData()
    {
        // Arrange
        var param = new OutstandingParam(
            CustId: 1,
            CustLevelId: 2, // Agent level
            ProductIds: "1,2,3",
            LevelType: 0,
            AutoFollowBetIsDummyAgent: false,
            SiteName: "TestSite"
        );

        var outstandingData = BuildFakeOutstandingModel();

        repositoryMock.Setup(r => r.FetchAsync<OutstandingModel>(It.IsAny<ICriteria>(), string.Empty))
            .ReturnsAsync(outstandingData);

        // Act
        var result = await service.GetOutstanding(param);

        // Assert
        Assert.Equal(2, result.DataSource.Count());
        Assert.Equal(3351.00m, result.Total.MemberOutstanding);
        Assert.Equal(2426.30m, result.Total.AgentOutstanding);
        Assert.Equal(1400.75m, result.Total.MasterOutstanding);
        Assert.Equal(662.30m, result.Total.SuperOutstanding);
        Assert.Equal(1250.50m, result.Total.ToWin);
        Assert.Equal(306.25m, result.Total.SuperRisk);
        Assert.Equal(571.00m, result.Total.MasterRisk);
        Assert.Equal(855.75m, result.Total.AgentRisk);

        partialUMServiceMock.Verify(p => p.ThrowIfUM(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        repositoryMock.Verify(r => r.FetchAsync<OutstandingModel>(It.IsAny<OutstandingMemberCriteria>(), string.Empty), Times.Once);
    }

    [Fact]
    public async Task GetOutstanding_WithMasterLevelParam_ShouldUseAgentCriteria()
    {
        // Arrange
        var param = new OutstandingParam(
            CustId: 1,
            CustLevelId: 3, // Master level
            ProductIds: "1,2,3",
            LevelType: 1,
            AutoFollowBetIsDummyAgent: false,
            SiteName: "TestSite"
        );

        repositoryMock.Setup(r => r.FetchAsync<OutstandingModel>(It.IsAny<ICriteria>(), string.Empty))
            .ReturnsAsync(new List<OutstandingModel>());

        // Act
        await service.GetOutstanding(param);

        // Assert
        repositoryMock.Verify(r => r.FetchAsync<OutstandingModel>(It.IsAny<OutstandingAgentCriteria>(), string.Empty), Times.Once);
    }

    [Fact]
    public async Task GetOutstanding_WithSuperLevelParam_ShouldUseMasterCriteria()
    {
        // Arrange
        var param = new OutstandingParam(
            CustId: 1,
            CustLevelId: 4, // Super level
            ProductIds: "1,2,3",
            LevelType: 1,
            AutoFollowBetIsDummyAgent: false,
            SiteName: "TestSite"
        );

        repositoryMock.Setup(r => r.FetchAsync<OutstandingModel>(It.IsAny<ICriteria>(), string.Empty))
            .ReturnsAsync(new List<OutstandingModel>());

        // Act
        await service.GetOutstanding(param);

        // Assert
        repositoryMock.Verify(r => r.FetchAsync<OutstandingModel>(It.IsAny<OutstandingMasterCriteria>(), string.Empty), Times.Once);
    }

    [Fact]
    public async Task GetOutstanding_WithAutoFollowBet_ShouldFetchDataForFollowBetAccounts()
    {
        // Arrange
        var param = new OutstandingParam(
            CustId: 1,
            CustLevelId: 4,
            ProductIds: "1,2,3",
            LevelType: 0,
            AutoFollowBetIsDummyAgent: true,
            SiteName: "TestSite"
        );

        var followBetAccounts = new List<Tuple<int, IEnumerable<FollowBetAccount>>>
        {
            new Tuple<int, IEnumerable<FollowBetAccount>>(
                10,
                new List<FollowBetAccount>
                {
                    new FollowBetAccount { CustId = 100, DummyCustId = 1000, DummyUsername = "Dummy1", AgentId = 1 },
                    new FollowBetAccount { CustId = 101, DummyCustId = 1001, DummyUsername = "Dummy2", AgentId = 2 }
                })
        };

        var outstandingData = new List<OutstandingModel>
        {
            new OutstandingModel { CustId = 100, MemberOutstanding = 100 },
            new OutstandingModel { CustId = 101, MemberOutstanding = 200 }
        };

        autoFollowBetServiceMock.Setup(a => a.GetFollowBetAccounts(param.CustId))
            .ReturnsAsync(followBetAccounts);

        repositoryMock.Setup(r => r.FetchAsync<OutstandingModel>(It.IsAny<OutstandingMemberCriteria>(), string.Empty))
            .ReturnsAsync(outstandingData);

        // Act
        var result = await service.GetOutstanding(param);

        // Assert
        Assert.Equal(2, result.DataSource.Count());
        Assert.Equal(300, result.Total.MemberOutstanding);

        Assert.Equal(1000, result.DataSource.ElementAt(0).CustId);
        Assert.Equal("Dummy1", result.DataSource.ElementAt(0).CustName);

        Assert.Equal(1001, result.DataSource.ElementAt(1).CustId);
        Assert.Equal("Dummy2", result.DataSource.ElementAt(1).CustName);

        autoFollowBetServiceMock.Verify(a => a.GetFollowBetAccounts(param.CustId), Times.Once);
        repositoryMock.Verify(r => r.FetchAsync<OutstandingModel>(It.IsAny<OutstandingMemberCriteria>(), string.Empty), Times.Once);
    }

    [Fact]
    public async Task GetOutstanding_WhenNoData_ShouldReturnEmptyReportWithZeroTotal()
    {
        // Arrange
        var param = new OutstandingParam(
            CustId: 1,
            CustLevelId: 4,
            ProductIds: "1,2,3",
            LevelType: 0,
            AutoFollowBetIsDummyAgent: false,
            SiteName: "TestSite"
        );

        repositoryMock.Setup(r => r.FetchAsync<OutstandingModel>(It.IsAny<ICriteria>(), string.Empty))
            .ReturnsAsync(new List<OutstandingModel>());

        // Act
        var result = await service.GetOutstanding(param);

        // Assert
        Assert.Equal(0, result.TotalRecords);
        Assert.False(result.HasData);
        Assert.False(result.MoreThanOne);
    }

    private static IEnumerable<OutstandingModel> BuildFakeOutstandingModel()
    {
        return new List<OutstandingModel>
        {
            new()
            {
                CustId = 1001,
                CustName = "John Smith",
                MemberOutstanding = 1250.75m,
                AgentOutstanding = 850.50m,
                MasterOutstanding = 425.25m,
                SuperOutstanding = 212.10m,
                ToWin = 500.00m,
                SuperRisk = 125.50m,
                MasterRisk = 250.75m,
                AgentRisk = 375.25m,
                IsDirectMember = true
            },
            new()
            {
                CustId = 1002,
                CustName = "Alice Johnson",
                MemberOutstanding = 2100.25m,
                AgentOutstanding = 1575.80m,
                MasterOutstanding = 975.50m,
                SuperOutstanding = 450.20m,
                ToWin = 750.50m,
                SuperRisk = 180.75m,
                MasterRisk = 320.25m,
                AgentRisk = 480.50m,
                IsDirectMember = false
            }
        };
    }
}