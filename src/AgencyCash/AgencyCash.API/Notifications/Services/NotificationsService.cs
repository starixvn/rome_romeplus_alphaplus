using AgencyCash.API.Notifications.Constants;
using AgencyCash.API.Notifications.Criteria;
using AgencyCash.API.Notifications.Models;
using Fanex.Data.Repository;

namespace AgencyCash.API.Notifications.Services;

public interface INotificationsService
{
    Task NotifyPendingDeposit(int agentId, int custId, object[] messageParams);

    Task NotifyPendingWithdrawal(int agentId, int custId, object[] messageParams);
}

public class NotificationsService(IDynamicRepository repository) : INotificationsService
{
    private const byte AgencyCashAPIToastMsgSiteFunctionId = 5;
    private readonly IDynamicRepository repository = repository;

    public Task NotifyPendingDeposit(int agentId, int custId, object[] messageParams)
    {
        const string NotifyPendingDeposit_Title = "NotifyPendingDeposit_Title";
        const string NotifyPendingDeposit = "NotifyPendingDeposit";
        var message = new LocalizedMessage(
            agentId,
            string.Empty,
            custId,
            NotifyPendingDeposit_Title,
            NotifyPendingDeposit,
            messageParams);

        return AddPersonalAndToastMsg(message);
    }

    public Task NotifyPendingWithdrawal(int agentId, int custId, object[] messageParams)
    {
        const string NotifyPendingWithdrawal_Title = "NotifyPendingWithdrawal_Title";
        const string NotifyPendingWithdrawal = "NotifyPendingWithdrawal";
        var message = new LocalizedMessage(
            agentId,
            string.Empty,
            custId,
            NotifyPendingWithdrawal_Title,
            NotifyPendingWithdrawal,
            messageParams);

        return AddPersonalAndToastMsg(message);
    }

    public async Task AddPersonalAndToastMsg(LocalizedMessage message)
    {
        message.AssignPersonalMessage();
        await AddPersonalMsg(message);

        message.AssignToastMessage();
        await AddToastMsg(message);
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
            message.TargetId,
            message.TargetName));
    }

    public async Task AddToastMsg(LocalizedMessage message)
    {
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
