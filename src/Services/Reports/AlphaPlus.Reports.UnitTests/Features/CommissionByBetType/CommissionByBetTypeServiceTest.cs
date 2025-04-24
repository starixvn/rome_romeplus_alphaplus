using AlphaPlus.PartialUM.Services;
using AlphaPlus.Reports.Features.CommissionByBetType;
using AlphaPlus.Reports.Features.Shared;
using AlphaPlus.Reports.Features.Shared.Models;
using Fanex.Data.Repository;

namespace AlphaPlus.Reports.UnitTests.Features.CommissionByBetType;

[Trait("Category", "CommissionByBetType")]
public class CommissionByBetTypeServiceTest
{
    private readonly Mock<IDynamicRepository> repositoryMock;
    private readonly Mock<IPartialUMService> partialUMServiceMock;
    private readonly Mock<IReportDateService> reportDateServiceMock;
    private readonly CommissionByBetTypeService service;

    public CommissionByBetTypeServiceTest()
    {
        repositoryMock = new Mock<IDynamicRepository>();
        partialUMServiceMock = new Mock<IPartialUMService>();
        reportDateServiceMock = new Mock<IReportDateService>();

        service = new CommissionByBetTypeService(
            repositoryMock.Object,
            partialUMServiceMock.Object,
            reportDateServiceMock.Object);
    }

    [Fact]
    public async Task GetCommissionByBetTypes_ShouldCallPartialUMService_ToCheckPermission()
    {
        // Arrange
        var param = new CommissionByBetTypeParam(
            RoleIdConstants.Agent,
            123,
            new DateTime(2023, 1, 1),
            new DateTime(2023, 1, 31),
            0, // Add the missing parameter (likely language or levelType)
            "TestSite");

        var moveBetsDay = new DateTime(2023, 2, 15);
        reportDateServiceMock
            .Setup(x => x.GetMoveBetsDay())
            .ReturnsAsync(moveBetsDay);

        var mockData = new List<CommissionByBetTypeModel>
            {
                new CommissionByBetTypeModel()
            };

        repositoryMock
            .Setup(x => x.FetchAsync<CommissionByBetTypeModel>(It.IsAny<ICriteria>(), string.Empty))
            .ReturnsAsync(mockData);

        // Act
        await service.GetCommissionByBetTypes(param);

        // Assert
        partialUMServiceMock.Verify(
            x => x.ThrowIfUM(It.IsAny<string>(), param.SiteName),
            Times.Once);
    }

    [Fact]
    public async Task GetCommissionByBetTypes_ShouldUseCorrectCriteria_ForAgentLevel()
    {
        // Arrange
        var param = new CommissionByBetTypeParam(
            RoleIdConstants.Agent,
            123,
            new DateTime(2023, 1, 1),
            new DateTime(2023, 1, 31),
            0, // Language or levelType parameter
            "TestSite");

        var moveBetsDay = new DateTime(2023, 2, 15);
        reportDateServiceMock
            .Setup(x => x.GetMoveBetsDay())
            .ReturnsAsync(moveBetsDay);

        var mockData = new List<CommissionByBetTypeModel>
            {
                new CommissionByBetTypeModel()
            };

        repositoryMock
            .Setup(x => x.FetchAsync<CommissionByBetTypeModel>(It.IsAny<ICriteria>(), string.Empty))
            .ReturnsAsync(mockData);

        // Act
        await service.GetCommissionByBetTypes(param);

        // Assert
        repositoryMock.Verify(
            x => x.FetchAsync<CommissionByBetTypeModel>(It.Is<ICriteria>(c => c is CommissionByBetTypeMemberCriteria), string.Empty),
            Times.Once);
    }

    [Fact]
    public async Task GetCommissionByBetTypes_ShouldUseCorrectCriteria_ForMasterLevel()
    {
        // Arrange
        var param = new CommissionByBetTypeParam(
            RoleIdConstants.Master,
            123,
            new DateTime(2023, 1, 1),
            new DateTime(2023, 1, 31),
            0, // Language or levelType parameter
            "TestSite");

        var moveBetsDay = new DateTime(2023, 2, 15);
        reportDateServiceMock
            .Setup(x => x.GetMoveBetsDay())
            .ReturnsAsync(moveBetsDay);

        var mockData = new List<CommissionByBetTypeModel>
            {
                new CommissionByBetTypeModel()
            };

        repositoryMock
            .Setup(x => x.FetchAsync<CommissionByBetTypeModel>(It.IsAny<ICriteria>(), string.Empty))
            .ReturnsAsync(mockData);

        // Act
        await service.GetCommissionByBetTypes(param);

        // Assert
        repositoryMock.Verify(
            x => x.FetchAsync<CommissionByBetTypeModel>(It.Is<ICriteria>(c => c is CommissionByBetTypeAgentCriteria), string.Empty),
            Times.Once);
    }

    [Fact]
    public async Task GetCommissionByBetTypes_ShouldUseCorrectCriteria_ForSuperLevel()
    {
        // Arrange
        var param = new CommissionByBetTypeParam(
            RoleIdConstants.Super,
            123,
            new DateTime(2023, 1, 1),
            new DateTime(2023, 1, 31),
            0, // Language or levelType parameter
            "TestSite");

        var moveBetsDay = new DateTime(2023, 2, 15);
        reportDateServiceMock
            .Setup(x => x.GetMoveBetsDay())
            .ReturnsAsync(moveBetsDay);

        var mockData = new List<CommissionByBetTypeModel>
            {
                new CommissionByBetTypeModel()
            };

        repositoryMock
            .Setup(x => x.FetchAsync<CommissionByBetTypeModel>(It.IsAny<ICriteria>(), string.Empty))
            .ReturnsAsync(mockData);

        // Act
        await service.GetCommissionByBetTypes(param);

        // Assert
        repositoryMock.Verify(
            x => x.FetchAsync<CommissionByBetTypeModel>(It.Is<ICriteria>(c => c is CommissionByBetTypeMasterCriteria), string.Empty),
            Times.Once);
    }

    [Fact]
    public async Task GetCommissionByBetTypes_ShouldCalculateTotal_FromReturnedData()
    {
        // Arrange
        var param = new CommissionByBetTypeParam(
            RoleIdConstants.Agent,
            123,
            new DateTime(2023, 1, 1),
            new DateTime(2023, 1, 31),
            0, // Language or levelType parameter
            "TestSite");

        var mockData = new List<CommissionByBetTypeModel>
        {
            new CommissionByBetTypeModel
            {
                CustId = 101,
                CustName = "John Doe",
                Comm1 = 125.50m,
                Comm2 = 75.25m,
                Comm3 = 30.00m,
                CasinoComm = 200.75m,
                IsDirectMember = true
            },
            new CommissionByBetTypeModel
            {
                CustId = 202,
                CustName = "Alice Smith",
                Comm1 = 89.75m,
                Comm2 = 120.30m,
                Comm3 = 45.50m,
                CasinoComm = 175.60m,
                IsDirectMember = false
            },
            new CommissionByBetTypeModel
            {
                CustId = 303,
                CustName = "Robert Johnson",
                Comm1 = 215.40m,
                Comm2 = 95.80m,
                Comm3 = 60.25m,
                CasinoComm = 320.15m,
                IsDirectMember = true
            }
        };

        var moveBetsDay = new DateTime(2023, 2, 15);
        reportDateServiceMock
            .Setup(x => x.GetMoveBetsDay())
            .ReturnsAsync(moveBetsDay);

        repositoryMock
            .Setup(x => x.FetchAsync<CommissionByBetTypeModel>(It.IsAny<ICriteria>(), string.Empty))
            .ReturnsAsync(mockData);

        // Act
        var result = await service.GetCommissionByBetTypes(param);

        // Assert
        Assert.Equal(430.65m, result.Total.Comm1);
        Assert.Equal(291.35m, result.Total.Comm2);
        Assert.Equal(135.75m, result.Total.Comm3);
        Assert.Equal(696.50m, result.Total.CasinoComm);
        Assert.Equal(mockData.Count, result.DataSource.Count());
    }

    [Fact]
    public async Task GetCommissionByBetTypes_ShouldReturnEmptyTotal_WhenNoData()
    {
        // Arrange
        var param = new CommissionByBetTypeParam(
            RoleIdConstants.Agent,
            123,
            new DateTime(2023, 1, 1),
            new DateTime(2023, 1, 31),
            0, // Language or levelType parameter
            "TestSite");

        var mockData = new List<CommissionByBetTypeModel>();

        var moveBetsDay = new DateTime(2023, 2, 15);
        reportDateServiceMock
            .Setup(x => x.GetMoveBetsDay())
            .ReturnsAsync(moveBetsDay);

        repositoryMock
            .Setup(x => x.FetchAsync<CommissionByBetTypeModel>(It.IsAny<ICriteria>(), string.Empty))
            .ReturnsAsync(mockData);

        // Act
        var result = await service.GetCommissionByBetTypes(param);

        // Assert
        Assert.Equal(0, result.TotalRecords);
        Assert.False(result.HasData);
        Assert.False(result.MoreThanOne);
    }
}