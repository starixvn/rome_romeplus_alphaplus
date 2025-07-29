using AgencyCash.API.Notifications.Constants;
using AgencyCash.API.Notifications.Criteria;
using AgencyCash.API.Notifications.Models;
using Fanex.Data.Repository;

namespace AgencyCash.API.Notifications.Services;

public interface INotificationsService
{
    Task Notify(NotificationParam param);
}

public class NotificationsService(IDynamicRepository repository) : INotificationsService
{
    private const byte AgencyCashAPIToastMsgSiteFunctionId = 5;
    private readonly IDynamicRepository repository = repository;

    public Task Notify(NotificationParam param)
        => Task.WhenAll(AddPersonalMsg(param), AddToastMsg(param));

    public async Task AddPersonalMsg(NotificationParam param)
    {
        var message = new LocalizedMessage(param.AgentId, param.AgentName, param.CustId);
        message.AssignPersonalMessage(param.PersonalMessageResourceKey);

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
            message.TargetId,
            message.TargetName));
    }

    public async Task AddToastMsg(NotificationParam param)
    {
        var message = new LocalizedMessage(param.AgentId, param.AgentName, param.CustId);
        message.AssignToastMessage(param.ToastTitleResourceKey, param.ToastBodyResourceKey);

        var criteria = new AddToastMsgCriteria(
            message.TargetId,
            ToastMsgType.Warning,
            AgencyCashAPIToastMsgSiteFunctionId,
            message.Creator,
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
