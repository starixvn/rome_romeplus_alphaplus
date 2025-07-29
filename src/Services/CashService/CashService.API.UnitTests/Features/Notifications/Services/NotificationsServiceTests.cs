using AlphaPlus.CashService.API.Features.Notifications.Criteria;
using AlphaPlus.CashService.API.Features.Notifications.Models;
using AlphaPlus.CashService.API.Features.Notifications.Services;
using Fanex.Data.Repository;
using Moq;
using Xunit;

namespace AlphaPlus.CashService.API.UnitTests.Features.Notifications.Services;

[Trait("Category", "Notifications")]
public class NotificationsServiceTests
{
    private readonly Mock<IDynamicRepository> repositoryMock;
    private readonly NotificationsService service;

    public NotificationsServiceTests()
    {
        repositoryMock = new Mock<IDynamicRepository>();
        service = new NotificationsService(repositoryMock.Object);
    }

    [Fact]
    public async Task GetPendingCount_Should_Return_Expected_Data()
    {
        // Arrange
        const int custId = 123;
        const byte typeId = 1;

        var expectedResult = new TransactionPendingCount
        {
            PendingDeposit = 5
        };

        repositoryMock
            .Setup(r => r.GetAsync<TransactionPendingCount>(
                It.Is<GetTransactionPendingCountCriteria>(c =>
                    c.CustId == custId && c.TypeId == typeId), string.Empty, default))
            .ReturnsAsync(expectedResult);

        // Act
        var result = await service.GetPendingCount(custId, typeId);

        // Assert
        Assert.Equal(expectedResult, result);
        Assert.Equal(expectedResult.PendingDeposit, result.PendingDeposit);

        repositoryMock.Verify(
            r => r.GetAsync<TransactionPendingCount>(
                It.Is<GetTransactionPendingCountCriteria>(c =>
                    c.CustId == custId && c.TypeId == typeId), string.Empty, default),
            Times.Once);
    }

    [Fact]
    public async Task GetPendingCount_Should_Pass_Correct_Parameters_To_Repository()
    {
        // Arrange
        const int custId = 456;
        const byte typeId = 2;

        repositoryMock
            .Setup(r => r.GetAsync<TransactionPendingCount>(
                It.IsAny<GetTransactionPendingCountCriteria>(), string.Empty, default))
            .ReturnsAsync(new TransactionPendingCount());

        // Act
        await service.GetPendingCount(custId, typeId);

        // Assert
        repositoryMock.Verify(
            r => r.GetAsync<TransactionPendingCount>(
                It.Is<GetTransactionPendingCountCriteria>(c =>
                    c.CustId == custId && c.TypeId == typeId), string.Empty, default),
            Times.Once);
    }

    [Fact]
    public async Task GetPendingCount_Should_Return_Null_When_Repository_Returns_Null()
    {
        // Arrange
        const int custId = 789;
        const byte typeId = 3;

        TransactionPendingCount? nullResult = null;

        repositoryMock
            .Setup(r => r.GetAsync<TransactionPendingCount>(
                It.IsAny<GetTransactionPendingCountCriteria>(), string.Empty, default))
            .ReturnsAsync(nullResult);

        // Act
        var result = await service.GetPendingCount(custId, typeId);

        // Assert
        Assert.Null(result);
    }
}