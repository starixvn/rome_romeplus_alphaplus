using AAgencyCash.API.Notifications.Criteria;
using AgencyCash.API.Notifications.Constants;
using AgencyCash.API.Notifications.Criteria;
using AgencyCash.API.Notifications.Models;
using Fanex.Data.Repository;

namespace AgencyCash.API.Notifications.Services;

public interface INotificationsService
{

}

public class NotificationsService(IDynamicRepository repository) : INotificationsService
{
    private readonly IDynamicRepository repository = repository;

    public async Task AddPersonalMsg(LocalizedMessage message, int custId, string custName)
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
            custId,
            custName));
    }

    public async Task AddToastMsg(LocalizedMessage message, int custId, string custName)
    {
        var criteria = new AddToastMsgCriteria(
            custId,
            ToastMsgType.Info,
            0,
            0,
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