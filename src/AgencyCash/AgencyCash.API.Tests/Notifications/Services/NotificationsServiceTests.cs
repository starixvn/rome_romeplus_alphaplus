using AgencyCash.API.Notifications.Constants;
using AgencyCash.API.Notifications.Criteria;
using AgencyCash.API.Notifications.Models;
using AgencyCash.API.Notifications.Services;
using Fanex.Data.Repository;
using Moq;

namespace AgencyCash.API.Tests.Notifications.Services;

public class NotificationsServiceTests
{
    private readonly Mock<IDynamicRepository> _repositoryMock;
    private readonly NotificationsService _service;

    public NotificationsServiceTests()
    {
        _repositoryMock = new Mock<IDynamicRepository>();
        _service = new NotificationsService(_repositoryMock.Object);
    }

    [Fact]
    public async Task NotifyPendingDeposit_Should_Call_AddPersonalAndToastMsg()
    {
        // Arrange
        var agentId = 100;
        var custId = 200;
        var messageParams = new object[] { "param1", "param2" };

        _repositoryMock
            .Setup(r => r.ExecuteAsync(It.IsAny<INonQueryCommand>(), string.Empty, default))
            .ReturnsAsync(1);

        // Act
        await _service.NotifyPendingDeposit(agentId, custId, messageParams);

        // Assert
        _repositoryMock.Verify(
            r => r.ExecuteAsync(It.IsAny<AddPersonalMsgCriteria>(), string.Empty, default),
            Times.Once);

        _repositoryMock.Verify(
            r => r.ExecuteAsync(It.IsAny<AssignPersonalMsgCriteria>(), string.Empty, default),
            Times.Once);

        _repositoryMock.Verify(
            r => r.ExecuteAsync(It.Is<AddToastMsgCriteria>(c =>
                c.TargetId == agentId &&
                c.TypeId == ToastMsgType.Warning), string.Empty, default),
            Times.Once);
    }

    [Fact]
    public async Task NotifyPendingWithdrawal_Should_Call_AddPersonalAndToastMsg()
    {
        // Arrange
        var agentId = 100;
        var custId = 200;
        var messageParams = new object[] { "param1", "param2" };

        _repositoryMock
            .Setup(r => r.ExecuteAsync(It.IsAny<INonQueryCommand>(), string.Empty, default))
            .ReturnsAsync(1);

        // Act
        await _service.NotifyPendingWithdrawal(agentId, custId, messageParams);

        // Assert
        _repositoryMock.Verify(
            r => r.ExecuteAsync(It.IsAny<AddPersonalMsgCriteria>(), string.Empty, default),
            Times.Once);

        _repositoryMock.Verify(
            r => r.ExecuteAsync(It.IsAny<AssignPersonalMsgCriteria>(), string.Empty, default),
            Times.Once);

        _repositoryMock.Verify(
            r => r.ExecuteAsync(It.Is<AddToastMsgCriteria>(c =>
                c.TargetId == agentId &&
                c.TypeId == ToastMsgType.Warning), string.Empty, default),
            Times.Once);
    }

    [Fact]
    public async Task AddPersonalMsg_Should_Execute_AddPersonalMsgCriteria_And_AssignPersonalMsgCriteria()
    {
        // Arrange
        var message = new LocalizedMessage(
            targetId: 100,
            targetName: string.Empty,
            creator: 200,
            titleMessageResourceKey: "title",
            messageResourceKey: "message",
            messageParams: ["param"]);

        message.AssignPersonalMessage();

        const int messageId = 42;
        _repositoryMock
            .Setup(r => r.ExecuteAsync(It.IsAny<INonQueryCommand>(), string.Empty, default))
            .Callback<INonQueryCommand, string, CancellationToken>((c, _, _) =>
            {
                if (c is AddPersonalMsgCriteria criteria)
                {
                    criteria.MessageIdOutput = messageId;
                }
            })
            .ReturnsAsync(1);

        // Act
        await _service.AddPersonalMsg(message);

        // Assert
        _repositoryMock.Verify(
            r => r.ExecuteAsync(It.IsAny<AddPersonalMsgCriteria>(), string.Empty, default),
            Times.Once);

        _repositoryMock.Verify(
            r => r.ExecuteAsync(It.Is<AssignPersonalMsgCriteria>(c =>
                c.MessageId == messageId &&
                c.TargetId == message.TargetId), string.Empty, default),
            Times.Once);
    }

    [Fact]
    public async Task AddToastMsg_Should_Execute_AddToastMsgCriteria()
    {
        // Arrange
        var message = new LocalizedMessage(
            targetId: 100,
            targetName: string.Empty,
            creator: 200,
            titleMessageResourceKey: "title",
            messageResourceKey: "message",
            messageParams: ["param"]);

        message.AssignToastMessage();

        _repositoryMock
            .Setup(r => r.ExecuteAsync(It.IsAny<AddToastMsgCriteria>(), string.Empty, default))
            .ReturnsAsync(1);

        // Act
        await _service.AddToastMsg(message);

        // Assert
        _repositoryMock.Verify(
            r => r.ExecuteAsync(It.Is<AddToastMsgCriteria>(c =>
                c.TargetId == message.TargetId &&
                c.TypeId == ToastMsgType.Warning &&
                c.SiteFunctionId == 5 &&
                c.Creator == message.Creator), string.Empty, default),
            Times.Once);
    }

    [Fact]
    public async Task AddPersonalAndToastMsg_Should_Call_AddPersonalMsg_And_AddToastMsg()
    {
        // Arrange
        var message = new LocalizedMessage(
            targetId: 100,
            targetName: string.Empty,
            creator: 200,
            titleMessageResourceKey: "title",
            messageResourceKey: "message",
            messageParams: ["param"]);

        _repositoryMock
            .Setup(r => r.ExecuteAsync(It.IsAny<INonQueryCommand>(), string.Empty, default))
            .ReturnsAsync(1);

        // Act
        await _service.AddPersonalAndToastMsg(message);

        // Assert
        _repositoryMock.Verify(
            r => r.ExecuteAsync(It.IsAny<AddPersonalMsgCriteria>(), string.Empty, default),
            Times.Once);

        _repositoryMock.Verify(
            r => r.ExecuteAsync(It.IsAny<AssignPersonalMsgCriteria>(), string.Empty, default),
            Times.Once);

        _repositoryMock.Verify(
            r => r.ExecuteAsync(It.IsAny<AddToastMsgCriteria>(), string.Empty, default),
            Times.Once);
    }
}