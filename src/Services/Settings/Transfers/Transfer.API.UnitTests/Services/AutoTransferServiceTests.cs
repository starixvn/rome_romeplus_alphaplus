using AlphaPlus.Settings.Transfer.API.Models.AutoTransfer;
using AlphaPlus.Settings.Transfer.API.Params.AutoTransfer;
using AlphaPlus.Settings.Transfer.API.Services;
using Fanex.Data.Repository;

namespace AlphaPlus.Settings.Transfer.API.UnitTests.Services;

public class AutoTransferServiceTests
{
    private readonly Mock<IDynamicRepository> _mockRepository;
    private readonly AutoTransferService _service;

    public AutoTransferServiceTests()
    {
        _mockRepository = new Mock<IDynamicRepository>();
        _service = new AutoTransferServiceImpl(_mockRepository.Object);
    }

    [Fact]
    public void Constructor_Should_Create_Service_Successfully()
    {
        // Act & Assert
        Assert.NotNull(_service);
        Assert.IsAssignableFrom<AutoTransferService>(_service);
    }

    [Fact]
    public void Service_Should_Accept_Repository_Dependency()
    {
        // Arrange
        var mockRepo = new Mock<IDynamicRepository>();

        // Act
        var service = new AutoTransferServiceImpl(mockRepo.Object);

        // Assert
        Assert.NotNull(service);
    }

    [Fact]
    public async Task UpdateAsync_Should_Return_Result_Object()
    {
        // Arrange
        var param = new UpdateSingleParam
        {
            CustId = 123,
            UplineId = 456,
            State = AutoTransferConstant.On
        };

        // Act
        var result = await _service.UpdateAsync(param);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<Result>(result);
    }

    [Fact]
    public async Task GetTodayTransfersAsync_Should_Return_Monitoring_Page_Model()
    {
        // Arrange
        var param = new GetTodayTransfersParam
        {
            CustId = 123,
            PageSize = 10,
            PageIndex = 1,
            SortedBy = 1,
            Direction = 1,
            LevelType = 1,
            SiteDbGroupId = 1
        };

        // Act
        var result = await _service.GetTodayTransfersAsync(param);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<AutoTransferMonitoringPageModel>(result);
    }

    [Fact]
    public async Task GetHistoryTransfersAsync_Should_Return_Monitoring_Page_Model()
    {
        // Arrange
        var param = new GetHistoryTransfersParam
        {
            CustId = 123,
            FromDate = DateTime.Now.AddDays(-30),
            ToDate = DateTime.Now,
            LimitedDateToShowInfo = DateTime.Now.AddDays(-60),
            PageSize = 10,
            PageIndex = 1
        };

        // Act
        var result = await _service.GetHistoryTransfersAsync(param);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<AutoTransferMonitoringPageModel>(result);
    }

    [Fact]
    public async Task CancelJobAsync_Should_Return_Result()
    {
        // Arrange
        var param = new CancelJobParam
        {
            FromCustId = 123,
            ToCustId = 456
        };

        // Act
        var result = await _service.CancelJobAsync(param);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<Result>(result);
    }

    [Fact]
    public async Task CancelAllJobAsync_Should_Return_Result()
    {
        // Arrange
        var param = new CancelAllJobParam
        {
            FromCustId = 123
        };

        // Act
        var result = await _service.CancelAllJobAsync(param);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<Result>(result);
    }

    [Fact]
    public async Task UpdateScheduleAsync_Should_Return_Result()
    {
        // Arrange
        var param = new UpdateScheduleParam
        {
            CustId = 123,
            UplineId = 456,
            Schedule = "Monday,Tuesday"
        };

        // Act
        var result = await _service.UpdateScheduleAsync(param);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<Result>(result);
    }

    [Fact]
    public async Task UpdateMultipleAutoTransferAsync_Should_Return_Result_Model()
    {
        // Arrange
        var param = new UpdateMultipleAutoTransferParam
        {
            UplineId = 123,
            OffIds = "1,2",
            OffNames = "User1,User2", // Must match ids count
            OnIds = "3,4",
            OnNames = "User3,User4" // Must match ids count
        };

        // Act
        var result = await _service.UpdateMultipleAutoTransferAsync(param);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<UpdateMultipleAutoTransferResultModel>(result);
    }

    [Theory]
    [InlineData(AutoTransferConstant.On)]
    [InlineData(AutoTransferConstant.Off)]
    [InlineData("")]
    [InlineData(null)]
    public async Task UpdateAsync_Should_Handle_Different_States(string state)
    {
        // Arrange
        var param = new UpdateSingleParam
        {
            CustId = 123,
            UplineId = 456,
            State = state
        };

        // Act
        var result = await _service.UpdateAsync(param);

        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public async Task UpdateAsync_Should_Handle_Null_Schedule()
    {
        // Arrange
        var param = new UpdateSingleParam
        {
            CustId = 123,
            UplineId = 456,
            Schedule = null
        };

        // Act
        var result = await _service.UpdateAsync(param);

        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public async Task UpdateMultipleAutoTransferAsync_Should_Handle_Empty_Lists()
    {
        // Arrange
        var param = new UpdateMultipleAutoTransferParam
        {
            UplineId = 123,
            OffIds = "",
            OnIds = ""
        };

        // Act
        var result = await _service.UpdateMultipleAutoTransferAsync(param);

        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public void AutoTransferConstant_Values_Should_Be_Available()
    {
        // Verify constants are accessible for testing
        Assert.NotNull(AutoTransferConstant.On);
        Assert.NotNull(AutoTransferConstant.Off);
        Assert.NotEqual(AutoTransferConstant.On, AutoTransferConstant.Off);
    }
}