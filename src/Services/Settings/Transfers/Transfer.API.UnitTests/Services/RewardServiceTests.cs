using AlphaPlus.Core.Models;
using AlphaPlus.Settings.Transfer.API.Models.Reward;
using AlphaPlus.Settings.Transfer.API.Params.Reward;
using AlphaPlus.Settings.Transfer.API.Services;
using Fanex.Data.Repository;

namespace AlphaPlus.Settings.Transfer.API.UnitTests.Services;

public class RewardServiceTests
{
    private readonly Mock<IDynamicRepository> _mockRepository;
    private readonly IRewardService _service;

    public RewardServiceTests()
    {
        _mockRepository = new Mock<IDynamicRepository>();
        _service = new RewardService(_mockRepository.Object);
    }

    [Fact]
    public void Constructor_Should_Create_Service_Successfully()
    {
        // Act & Assert
        Assert.NotNull(_service);
        Assert.IsAssignableFrom<IRewardService>(_service);
    }

    [Fact]
    public void Service_Should_Accept_Repository_Dependency()
    {
        // Arrange
        var mockRepo = new Mock<IDynamicRepository>();

        // Act
        var service = new RewardService(mockRepo.Object);

        // Assert
        Assert.NotNull(service);
    }

    [Fact]
    public async Task GetRewardPrograms_Should_Return_IEnumerable()
    {
        // Act
        var result = await _service.GetRewardPrograms();

        // Assert
        Assert.NotNull(result);
        Assert.IsAssignableFrom<IEnumerable<RewardProgram>>(result);
    }

    [Fact]
    public async Task GetRewardPayments_Should_Return_RewardPayment()
    {
        // Arrange
        var param = new RewardParam(
            CustId: 123,
            CustLevel: UserLevel.Agent,
            FromDate: DateTime.Now.AddDays(-30),
            ToDate: DateTime.Now,
            ProgramIds: "1,2,3",
            PaymentStatus: true,
            LevelType: 1,
            Username: "testuser",
            PageIndex: 0,
            PageSize: 10
        );

        // Act
        var result = await _service.GetRewardPayments(param);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<RewardPayment>(result);
    }

    [Fact]
    public async Task GetRewardDetails_Should_Return_RewardDetail()
    {
        // Arrange
        var param = new RewardParam(
            CustId: 123,
            CustLevel: UserLevel.Master,
            FromDate: DateTime.Now.AddDays(-30),
            ToDate: DateTime.Now,
            ProgramIds: "1,2,3",
            PaymentStatus: null,
            LevelType: 2,
            Username: "testuser"
        );

        // Act
        var result = await _service.GetRewardDetails(param);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<RewardDetail>(result);
    }

    [Fact]
    public async Task UpdateRewardPaymentStatus_Should_Return_Result()
    {
        // Arrange
        var paymentStatusData = new[]
        {
            new RewardPaymentStatus
            {
                CustId = 123,
                Username = "user1",
                ProgramId = 1,
                MaxTransId = 1001,
                IsDirectMember = true
            },
            new RewardPaymentStatus
            {
                CustId = 456,
                Username = "user2",
                ProgramId = 2,
                MaxTransId = 1002,
                IsDirectMember = false
            }
        };

        var param = new UpdateRewardPaymentStatusParam(
            FromDate: DateTime.Now.AddDays(-30),
            ToDate: DateTime.Now,
            Data: paymentStatusData,
            ActorId: 789,
            SubAccId: 101112,
            SubAccName: "TestSubAcc",
            IP: "192.168.1.1",
            Language: "en",
            AGIP: "agip"
        );

        // Act
        var result = await _service.UpdateRewardPaymentStatus(param);

        // Assert
        Assert.NotNull(result);
        // Actual return type is RewardResult, not Result
        Assert.IsType<RewardResult>(result);
    }

    [Theory]
    [InlineData(1)] // Super level
    [InlineData(2)] // Master level
    [InlineData(3)] // Agent level
    [InlineData(4)] // Member level
    public async Task GetRewardPayments_Should_Handle_Different_User_Levels(byte levelType)
    {
        // Arrange
        var userLevel = levelType switch
        {
            1 => UserLevel.Super,
            2 => UserLevel.Master,
            3 => UserLevel.Agent,
            _ => UserLevel.Member
        };

        var param = new RewardParam(
            CustId: 123,
            CustLevel: userLevel,
            FromDate: DateTime.Now.AddDays(-30),
            ToDate: DateTime.Now,
            ProgramIds: "1,2,3",
            PaymentStatus: true,
            LevelType: levelType,
            Username: "testuser"
        );

        // Act
        var result = await _service.GetRewardPayments(param);

        // Assert
        Assert.NotNull(result);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    [InlineData(null)]
    public async Task GetRewardPayments_Should_Handle_Different_Payment_Status(bool? paymentStatus)
    {
        // Arrange
        var param = new RewardParam(
            CustId: 123,
            CustLevel: UserLevel.Agent,
            FromDate: DateTime.Now.AddDays(-30),
            ToDate: DateTime.Now,
            ProgramIds: "1,2,3",
            PaymentStatus: paymentStatus,
            LevelType: 3,
            Username: "testuser"
        );

        // Act
        var result = await _service.GetRewardPayments(param);

        // Assert
        Assert.NotNull(result);
    }

    [Theory]
    [InlineData("1")]
    [InlineData("1,2")]
    [InlineData("1,2,3,4,5")]
    [InlineData("")]
    [InlineData(null)]
    public async Task GetRewardPayments_Should_Handle_Different_Program_Ids(string programIds)
    {
        // Arrange
        var param = new RewardParam(
            CustId: 123,
            CustLevel: UserLevel.Agent,
            FromDate: DateTime.Now.AddDays(-30),
            ToDate: DateTime.Now,
            ProgramIds: programIds,
            PaymentStatus: true,
            LevelType: 3,
            Username: "testuser"
        );

        // Act
        var result = await _service.GetRewardPayments(param);

        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public async Task GetRewardPayments_Should_Handle_Date_Range()
    {
        // Arrange
        var fromDate = new DateTime(2024, 1, 1);
        var toDate = new DateTime(2024, 1, 31);

        var param = new RewardParam(
            CustId: 123,
            CustLevel: UserLevel.Agent,
            FromDate: fromDate,
            ToDate: toDate,
            ProgramIds: "1,2,3",
            PaymentStatus: true,
            LevelType: 3,
            Username: "testuser"
        );

        // Act
        var result = await _service.GetRewardPayments(param);

        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public async Task UpdateRewardPaymentStatus_Should_Handle_Empty_Data_Array()
    {
        // Arrange
        var param = new UpdateRewardPaymentStatusParam(
            FromDate: DateTime.Now.AddDays(-30),
            ToDate: DateTime.Now,
            Data: new RewardPaymentStatus[0],
            ActorId: 789,
            SubAccId: 101112,
            SubAccName: "TestSubAcc",
            IP: "192.168.1.1",
            Language: "en",
            AGIP: "agip"
        );

        // Act
        var result = await _service.UpdateRewardPaymentStatus(param);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<RewardResult>(result);
    }

    [Fact]
    public async Task UpdateRewardPaymentStatus_Should_Handle_Null_SubAccName()
    {
        // Arrange
        var paymentStatusData = new[]
        {
            new RewardPaymentStatus
            {
                CustId = 123,
                Username = "user1",
                ProgramId = 1,
                MaxTransId = 1001
            }
        };

        var param = new UpdateRewardPaymentStatusParam(
            FromDate: DateTime.Now.AddDays(-30),
            ToDate: DateTime.Now,
            Data: paymentStatusData,
            ActorId: 789,
            SubAccId: 101112,
            SubAccName: null,
            IP: "192.168.1.1",
            Language: "en",
            AGIP: "agip"
        );

        // Act
        var result = await _service.UpdateRewardPaymentStatus(param);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<RewardResult>(result);
    }

    [Fact]
    public async Task GetRewardDetails_Should_Handle_Zero_PageSize()
    {
        // Arrange
        var param = new RewardParam(
            CustId: 123,
            CustLevel: UserLevel.Agent,
            FromDate: DateTime.Now.AddDays(-30),
            ToDate: DateTime.Now,
            ProgramIds: "1,2,3",
            PaymentStatus: true,
            LevelType: 3,
            Username: "testuser",
            PageIndex: 0,
            PageSize: 0
        );

        // Act
        var result = await _service.GetRewardDetails(param);

        // Assert
        Assert.NotNull(result);
    }

    [Theory]
    [InlineData(0, 10)]
    [InlineData(1, 20)]
    [InlineData(5, 50)]
    public async Task GetRewardPayments_Should_Handle_Pagination(int pageIndex, int pageSize)
    {
        // Arrange
        var param = new RewardParam(
            CustId: 123,
            CustLevel: UserLevel.Agent,
            FromDate: DateTime.Now.AddDays(-30),
            ToDate: DateTime.Now,
            ProgramIds: "1,2,3",
            PaymentStatus: true,
            LevelType: 3,
            Username: "testuser",
            PageIndex: pageIndex,
            PageSize: pageSize
        );

        // Act
        var result = await _service.GetRewardPayments(param);

        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public async Task GetRewardPrograms_Should_Handle_No_Programs()
    {
        // Act
        var result = await _service.GetRewardPrograms();

        // Assert
        Assert.NotNull(result);
        // Should return empty enumerable if no programs exist
    }

    [Fact]
    public void RewardService_Should_Implement_Interface()
    {
        // Assert
        Assert.IsAssignableFrom<IRewardService>(_service);
    }

    [Theory]
    [InlineData("en")]
    [InlineData("zh")]
    [InlineData("ko")]
    [InlineData("")]
    [InlineData(null)]
    public async Task UpdateRewardPaymentStatus_Should_Handle_Different_Languages(string language)
    {
        // Arrange
        var paymentStatusData = new[]
        {
            new RewardPaymentStatus
            {
                CustId = 123,
                Username = "user1",
                ProgramId = 1,
                MaxTransId = 1001
            }
        };

        var param = new UpdateRewardPaymentStatusParam(
            FromDate: DateTime.Now.AddDays(-30),
            ToDate: DateTime.Now,
            Data: paymentStatusData,
            ActorId: 789,
            SubAccId: 101112,
            SubAccName: "TestSubAcc",
            IP: "192.168.1.1",
            Language: language,
            AGIP: "agip"
        );

        // Act
        var result = await _service.UpdateRewardPaymentStatus(param);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<RewardResult>(result);
    }
}
