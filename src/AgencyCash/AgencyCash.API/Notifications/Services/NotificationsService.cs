using AgencyCash.API.Notifications.Constants;
using AgencyCash.API.Notifications.Criteria;
using AgencyCash.API.Notifications.Models;
using Fanex.Data.Repository;

namespace AgencyCash.API.Notifications.Services;

public interface INotificationsService
{
    Task Notify(int agentId, int custId, string agentName, string resourceKey);
}

public class NotificationsService(IDynamicRepository repository) : INotificationsService
{
    private readonly IDynamicRepository repository = repository;

    public Task Notify(int agentId, int custId, string agentName, string resourceKey)
    {
        var message = new LocalizedMessage(agentId, custId, agentName);
        message.AssignLocalizedMessage(resourceKey);

        return Task.WhenAll(AddPersonalMsg(message), AddToastMsg(message));
    }

    public async Task AddPersonalMsg(LocalizedMessage message)
    {
        var addPersonalMsgCriteria = new AddPersonalMsgCriteria(
            message.En,
            message.Ch,
            message.Cs,
            message.Jp,
            message.Th,
            message.Ko,
            message.Vi);
        await repository.ExecuteAsync(addPersonalMsgCriteria);

        await repository.ExecuteAsync(new AssignPersonalMsgCriteria(
            addPersonalMsgCriteria.MessageIdOutput,
            message.AgentId,
            message.AgentName));
    }

    public async Task AddToastMsg(LocalizedMessage message)
    {
        var criteria = new AddToastMsgCriteria(
            message.AgentId,
            ToastMsgType.Info,
            0,
            message.CustId,
            message.En,
            message.Ch,
            message.Cs,
            message.Th,
            message.Vi,
            message.Jp,
            message.Ko);
        await repository.ExecuteAsync(criteria);
    }
}
