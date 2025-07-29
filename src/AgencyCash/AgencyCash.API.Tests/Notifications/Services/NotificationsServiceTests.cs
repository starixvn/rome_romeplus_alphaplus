using AgencyCash.API.Notifications.Constants;
using AgencyCash.API.Notifications.Criteria;
using AgencyCash.API.Notifications.Models;
using AgencyCash.API.Notifications.Services;
using Fanex.Data.Repository;
using Moq;

namespace AgencyCash.API.UnitTests.Notifications.Services;

public class NotificationsServiceTests
{
    private readonly Mock<IDynamicRepository> _repositoryMock;
    private readonly NotificationsService _service;
    private const int _defaultAgentId = 123;
    private const int _defaultCustId = 456;

    public NotificationsServiceTests()
    {
        _repositoryMock = new Mock<IDynamicRepository>();
        _service = new NotificationsService(_repositoryMock.Object);
    }

    [Fact]
    public async Task NotifyPendingDeposit_Should_Call_AddPersonalMsg_And_AddToastMsg()
    {
        // Arrange
        _repositoryMock.Setup(r => r.ExecuteAsync(It.IsAny<INonQueryCommand>(), string.Empty, default)).Returns(Task.FromResult(1));

        // Act
        await _service.NotifyPendingDeposit(_defaultAgentId, _defaultCustId);

        // Assert
        _repositoryMock.Verify(r => r.ExecuteAsync(It.IsAny<AddPersonalMsgCriteria>(), string.Empty, default), Times.Once);
        _repositoryMock.Verify(r => r.ExecuteAsync(It.IsAny<AssignPersonalMsgCriteria>(), string.Empty, default), Times.Once);
        _repositoryMock.Verify(r => r.ExecuteAsync(It.IsAny<AddToastMsgCriteria>(), string.Empty, default), Times.Once);
    }

    [Fact]
    public async Task NotifyPendingWithdrawal_Should_Call_AddPersonalMsg_And_AddToastMsg()
    {
        // Arrange
        _repositoryMock.Setup(r => r.ExecuteAsync(It.IsAny<INonQueryCommand>(), string.Empty, default)).Returns(Task.FromResult(1));

        // Act
        await _service.NotifyPendingWithdrawal(_defaultAgentId, _defaultCustId);

        // Assert
        _repositoryMock.Verify(r => r.ExecuteAsync(It.IsAny<AddPersonalMsgCriteria>(), string.Empty, default), Times.Once);
        _repositoryMock.Verify(r => r.ExecuteAsync(It.IsAny<AssignPersonalMsgCriteria>(), string.Empty, default), Times.Once);
        _repositoryMock.Verify(r => r.ExecuteAsync(It.IsAny<AddToastMsgCriteria>(), string.Empty, default), Times.Once);
    }

    [Fact]
    public async Task AddPersonalMsg_Should_Execute_Repository_Calls_With_Correct_Parameters()
    {
        // Arrange
        const int messageId = 789;
        _repositoryMock.Setup(r => r.ExecuteAsync(It.IsAny<INonQueryCommand>(), string.Empty, default))
            .Callback<INonQueryCommand, string, CancellationToken>((criteria, s, t) =>
            {
                if (criteria is AddPersonalMsgCriteria addMsgCriteria)
                {
                    addMsgCriteria!.MessageIdOutput = messageId;
                }
            })
            .Returns(Task.FromResult(1));

        var param = new NotificationParam(
            _defaultAgentId,
            "TestAgent",
            _defaultCustId,
            "TestResource",
            "TestTitle",
            "TestBody");

        // Act
        await _service.AddPersonalMsg(param);

        // Assert
        _repositoryMock.Verify(r => r.ExecuteAsync(It.Is<AddPersonalMsgCriteria>(
            c => c.ContentEN != null && c.ContentCH != null && c.ContentCS != null && c.ContentJP != null &&
                 c.ContentTH != null && c.ContentKO != null && c.ContentVN != null), string.Empty, default),
            Times.Once);

        _repositoryMock.Verify(r => r.ExecuteAsync(It.Is<AssignPersonalMsgCriteria>(
            c => c.MessageId == messageId && c.TargetId == _defaultAgentId), string.Empty, default),
            Times.Once);
    }

    [Fact]
    public async Task AddToastMsg_Should_Execute_Repository_Call_With_Correct_Parameters()
    {
        // Arrange
        _repositoryMock.Setup(r => r.ExecuteAsync(It.IsAny<AddToastMsgCriteria>(), string.Empty, default))
            .Returns(Task.FromResult(1));

        var param = new NotificationParam(
            _defaultAgentId,
            "TestAgent",
            _defaultCustId,
            "TestResource",
            "TestTitle",
            "TestBody");

        // Act
        await _service.AddToastMsg(param);

        // Assert
        _repositoryMock.Verify(r => r.ExecuteAsync(It.Is<AddToastMsgCriteria>(
            c => c.TargetId == _defaultAgentId &&
                 c.TypeId == ToastMsgType.Warning &&
                 c.SiteFunctionId == 5 &&
                 c.ContentEN != null && c.ContentCH != null && c.ContentCS != null &&
                 c.ContentTH != null && c.ContentVN != null && c.ContentJP != null && c.ContentKO != null), string.Empty, default),
            Times.Once);
    }

    [Fact]
    public async Task AddPersonalMsg_Should_Handle_Empty_AgentName()
    {
        // Arrange
        _repositoryMock.Setup(r => r.ExecuteAsync(It.IsAny<INonQueryCommand>(), string.Empty, default)).Returns(Task.FromResult(1));

        var param = new NotificationParam(
            _defaultAgentId,
            string.Empty,
            _defaultCustId,
            "TestResource",
            "TestTitle",
            "TestBody");

        // Act
        await _service.AddPersonalMsg(param);

        // Assert
        _repositoryMock.Verify(r => r.ExecuteAsync(It.IsAny<AddPersonalMsgCriteria>(), string.Empty, default), Times.Once);
        _repositoryMock.Verify(r => r.ExecuteAsync(It.IsAny<AssignPersonalMsgCriteria>(), string.Empty, default), Times.Once);
    }

    [Fact]
    public async Task AddToastMsg_Should_Use_Warning_Toast_Type()
    {
        // Arrange
        _repositoryMock.Setup(r => r.ExecuteAsync(It.IsAny<INonQueryCommand>(), string.Empty, default)).Returns(Task.FromResult(1));

        var param = new NotificationParam(
            _defaultAgentId,
            "TestAgent",
            _defaultCustId,
            "TestResource",
            "TestTitle",
            "TestBody");

        // Act
        await _service.AddToastMsg(param);

        // Assert
        _repositoryMock.Verify(r => r.ExecuteAsync(It.Is<AddToastMsgCriteria>(
            c => c.TypeId == ToastMsgType.Warning), string.Empty, default), Times.Once);
    }
}