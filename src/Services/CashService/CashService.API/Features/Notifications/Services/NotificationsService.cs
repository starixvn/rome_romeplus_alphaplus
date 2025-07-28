using AlphaPlus.CashService.API.Features.Notifications.Criteria;
using AlphaPlus.CashService.API.Features.Notifications.Models;
using Fanex.Data.Repository;

namespace AlphaPlus.CashService.API.Features.Notifications.Services;

public interface INotificationsService
{
    Task<TransactionPendingCount> GetPendingCount(int custId, byte type);
}

public class NotificationsService(IDynamicRepository repository) : INotificationsService
{
    private readonly IDynamicRepository repository = repository;

    public Task<TransactionPendingCount> GetPendingCount(int custId, byte typeId)
        => repository.GetAsync<TransactionPendingCount>(new GetTransactionPendingCountCriteria(custId, typeId));
}