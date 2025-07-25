using AgencyCash.API.Notifications.Constants;
using AgencyCash.API.Notifications.Criteria;
using AgencyCash.API.Notifications.Models;
using AgencyCash.API.Notifications.Services;
using Fanex.Data.Repository;
using Moq;

namespace AgencyCash.API.Tests.Notifications.Services;

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
    public async Task Notify_Should_Call_AddPersonalMsg_And_AddToastMsg()
    {
        // Arrange
        int agentId = 123;
        int custId = 456;
        string agentName = "TestAgent";
        string resourceKey = "TestResourceKey";

        repositoryMock
            .Setup(r => r.ExecuteAsync(It.IsAny<AddPersonalMsgCriteria>(), string.Empty, default))
            .ReturnsAsync(1);

        repositoryMock
            .Setup(r => r.ExecuteAsync(It.IsAny<AssignPersonalMsgCriteria>(), string.Empty, default))
            .ReturnsAsync(1);

        repositoryMock
            .Setup(r => r.ExecuteAsync(It.IsAny<AddToastMsgCriteria>(), string.Empty, default))
            .ReturnsAsync(1);

        // Act
        await service.Notify(agentId, custId, agentName, resourceKey);

        // Assert
        repositoryMock.Verify(r => r.ExecuteAsync(It.IsAny<AddPersonalMsgCriteria>(), string.Empty, default), Times.Once);
        repositoryMock.Verify(r => r.ExecuteAsync(It.IsAny<AssignPersonalMsgCriteria>(), string.Empty, default), Times.Once);
        repositoryMock.Verify(r => r.ExecuteAsync(It.IsAny<AddToastMsgCriteria>(), string.Empty, default), Times.Once);
    }

    [Fact]
    public async Task AddPersonalMsg_Should_Execute_Expected_Criteria()
    {
        // Arrange
        var message = new LocalizedMessage(123, 456, "TestAgent");
        message.AssignLocalizedMessage("TestResourceKey");

        int messageId = 789;
        repositoryMock
            .Setup(r => r.ExecuteAsync(It.IsAny<INonQueryCommand>(), string.Empty, default))
            .Callback<INonQueryCommand, string, CancellationToken>((c, s, t) =>
            {
                if (c is AddPersonalMsgCriteria)
                {
                    var criteria = c as AddPersonalMsgCriteria;
                    criteria.MessageIdOutput = messageId;
                }
            })
            .ReturnsAsync(1);

        // Act
        await service.AddPersonalMsg(message);

        // Assert
        repositoryMock.Verify(r => r.ExecuteAsync(
            It.Is<AddPersonalMsgCriteria>(c =>
                c.ContentEN == message.En &&
                c.ContentCH == message.Ch &&
                c.ContentCS == message.Cs &&
                c.ContentJP == message.Jp &&
                c.ContentTH == message.Th &&
                c.ContentKO == message.Ko &&
                c.ContentVN == message.Vi), string.Empty, default),
            Times.Once);

        repositoryMock.Verify(r => r.ExecuteAsync(
            It.Is<AssignPersonalMsgCriteria>(c =>
                c.MessageId == messageId &&
                c.TargetId == message.AgentId &&
                c.TargetName == message.AgentName), string.Empty, default),
            Times.Once);
    }

    [Fact]
    public async Task AddToastMsg_Should_Execute_Expected_Criteria()
    {
        // Arrange
        var message = new LocalizedMessage(123, 456, "TestAgent");
        message.AssignLocalizedMessage("TestResourceKey");

        // Act
        await service.AddToastMsg(message);

        // Assert
        repositoryMock.Verify(r => r.ExecuteAsync(
            It.Is<AddToastMsgCriteria>(c =>
                c.TargetId == message.AgentId &&
                c.TypeId == ToastMsgType.Info &&
                c.SiteFunctionId == 0 &&
                c.Creator == message.CustId &&
                c.ContentEN == message.En &&
                c.ContentCH == message.Ch &&
                c.ContentCS == message.Cs &&
                c.ContentTH == message.Th &&
                c.ContentVN == message.Vi &&
                c.ContentJP == message.Jp &&
                c.ContentKO == message.Ko), string.Empty, default),
            Times.Once);
    }
}
