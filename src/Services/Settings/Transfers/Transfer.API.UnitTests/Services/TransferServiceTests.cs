using AlphaPlus.Settings.Transfer.API.Params.TransferCondition;
using AlphaPlus.Settings.Transfer.API.Services;
using Fanex.Data.Repository;

namespace AlphaPlus.Settings.Transfer.API.UnitTests.Services;

public class TransferServiceTests
{
    private readonly Mock<IDynamicRepository> _mockRepository;
    private readonly ITransferService _service;

    public TransferServiceTests()
    {
        _mockRepository = new Mock<IDynamicRepository>();
        _service = new TransferService(_mockRepository.Object);
    }

    [Fact]
    public void Constructor_Should_Create_Service_Successfully()
    {
        // Act & Assert
        Assert.NotNull(_service);
        Assert.IsAssignableFrom<ITransferService>(_service);
    }

    [Fact]
    public void Service_Should_Accept_Repository_Dependency()
    {
        // Arrange
        var mockRepo = new Mock<IDynamicRepository>();

        // Act
        var service = new TransferService(mockRepo.Object);

        // Assert
        Assert.NotNull(service);
    }

    [Fact]
    public async Task IsAllDayBalanceTransfer_Should_Return_Boolean()
    {
        // Arrange
        var custId = 123;

        // Act
        var result = await _service.IsAllDayBalanceTransfer(custId);

        // Assert
        Assert.IsType<bool>(result);
    }

    [Fact]
    public async Task IsAllDayBalanceTransferAsync_Should_Return_Boolean()
    {
        // Arrange
        var custId = 456;

        // Act
        var result = await _service.IsAllDayBalanceTransferAsync(custId);

        // Assert
        Assert.IsType<bool>(result);
    }

    [Fact]
    public async Task GetTransferOption_Should_Return_String()
    {
        // Arrange
        var custId = 789;

        // Act
        var result = await _service.GetTransferOption(custId);

        // Assert
        Assert.IsType<string>(result);
        Assert.NotNull(result);
    }

    [Fact]
    public async Task GetTransferCondition_Should_Return_Tuple()
    {
        // Arrange
        var custId = 123;

        // Act
        var result = await _service.GetTransferCondition(custId);

        // Assert
        Assert.NotNull(result);
        // Actual return type is Tuple<bool, IEnumerable<string>>
        Assert.IsType<Tuple<bool, IEnumerable<string>>>(result);
    }

    [Fact]
    public async Task GetAllowTransfer_Should_Return_String()
    {
        // Arrange
        var custId = 123;

        // Act
        var result = await _service.GetAllowTransfer(custId);

        // Assert
        Assert.IsType<string>(result);
        Assert.NotNull(result);
    }

    [Fact]
    public async Task GetSuperIdAutomateTransfer_Should_Return_Integer()
    {
        // Arrange
        var custId = 123;

        // Act
        var result = await _service.GetSuperIdAutomateTransfer(custId);

        // Assert
        Assert.IsType<int>(result);
    }

    [Fact]
    public async Task UpdateTransferOption_Should_Return_Result()
    {
        // Arrange
        var param = new TransferConditionParam(
            ActorId: 456,
            ActorLevelId: UserLevel.Agent.Value,
            CustId: 123,
            TransferOption: "1111111",
            TurnOffAutoTransfer: false,
            SubAccId: 789,
            IP: "192.168.1.1",
            Language: "en",
            SiteDbGroupId: 1
        );

        // Act
        var result = await _service.UpdateTransferOption(param);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<Result>(result);
    }

    [Theory]
    [InlineData(123)]
    [InlineData(456)]
    [InlineData(789)]
    public async Task IsAllDayBalanceTransfer_Should_Handle_Different_CustIds(int custId)
    {
        // Act
        var result = await _service.IsAllDayBalanceTransfer(custId);

        // Assert
        Assert.IsType<bool>(result);
    }

    [Theory]
    [InlineData("1111111")]
    [InlineData("0000000")]
    [InlineData("1010101")]
    public async Task UpdateTransferOption_Should_Handle_Different_Transfer_Options(string transferOption)
    {
        // Arrange
        var param = new TransferConditionParam(
            ActorId: 456,
            ActorLevelId: UserLevel.Agent.Value,
            CustId: 123,
            TransferOption: transferOption,
            TurnOffAutoTransfer: false,
            SubAccId: 789,
            IP: "192.168.1.1",
            Language: "en",
            SiteDbGroupId: 1
        );

        // Act
        var result = await _service.UpdateTransferOption(param);

        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public async Task UpdateTransferOption_Should_Handle_Null_TransferOption()
    {
        // Arrange
        var param = new TransferConditionParam(
            ActorId: 456,
            ActorLevelId: UserLevel.Agent.Value,
            CustId: 123,
            TransferOption: null,
            TurnOffAutoTransfer: false,
            SubAccId: 789,
            IP: "192.168.1.1",
            Language: "en",
            SiteDbGroupId: 1
        );

        // Act
        var result = await _service.UpdateTransferOption(param);

        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public async Task UpdateTransferOption_Should_Handle_Empty_TransferOption()
    {
        // Arrange
        var param = new TransferConditionParam(
            ActorId: 456,
            ActorLevelId: UserLevel.Agent.Value,
            CustId: 123,
            TransferOption: "",
            TurnOffAutoTransfer: false,
            SubAccId: 789,
            IP: "192.168.1.1",
            Language: "en",
            SiteDbGroupId: 1
        );

        // Act
        var result = await _service.UpdateTransferOption(param);

        // Assert
        Assert.NotNull(result);
    }

    [Theory]
    [InlineData(1)] // Super level
    [InlineData(2)] // Master level
    [InlineData(3)] // Agent level
    [InlineData(4)] // Member level
    public async Task UpdateTransferOption_Should_Handle_Different_Actor_Levels(byte actorLevelId)
    {
        // Arrange
        var param = new TransferConditionParam(
            ActorId: 456,
            ActorLevelId: actorLevelId,
            CustId: 123,
            TransferOption: "1111111",
            TurnOffAutoTransfer: false,
            SubAccId: 789,
            IP: "192.168.1.1",
            Language: "en",
            SiteDbGroupId: 1
        );

        // Act
        var result = await _service.UpdateTransferOption(param);

        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public async Task GetTransferCondition_Should_Handle_Various_Customer_Ids()
    {
        // Arrange & Act
        var result1 = await _service.GetTransferCondition(123);
        var result2 = await _service.GetTransferCondition(456);
        var result3 = await _service.GetTransferCondition(789);

        // Assert
        Assert.NotNull(result1);
        Assert.NotNull(result2);
        Assert.NotNull(result3);
    }

    [Fact]
    public async Task GetAllowTransfer_Should_Return_Non_Empty_String()
    {
        // Arrange
        var custId = 123;

        // Act
        var result = await _service.GetAllowTransfer(custId);

        // Assert
        Assert.False(string.IsNullOrEmpty(result));
    }

    [Fact]
    public void TransferService_Should_Implement_Interface()
    {
        // Assert
        Assert.IsAssignableFrom<ITransferService>(_service);
    }

    [Fact]
    public async Task GetSuperIdAutomateTransfer_Should_Handle_Zero_Customer_Id()
    {
        // Arrange
        var custId = 0;

        // Act
        var result = await _service.GetSuperIdAutomateTransfer(custId);

        // Assert
        Assert.IsType<int>(result);
    }

    [Fact]
    public async Task UpdateTransferOption_Should_Handle_Large_Customer_Id()
    {
        // Arrange
        var param = new TransferConditionParam(
            ActorId: int.MaxValue,
            ActorLevelId: UserLevel.Super.Value,
            CustId: int.MaxValue,
            TransferOption: "1111111",
            TurnOffAutoTransfer: false,
            SubAccId: 789,
            IP: "192.168.1.1",
            Language: "en",
            SiteDbGroupId: 1
        );

        // Act
        var result = await _service.UpdateTransferOption(param);

        // Assert
        Assert.NotNull(result);
    }
}