using AlphaPlus.CashService.API.Features.Notifications.Models;

namespace AlphaPlus.CashService.API.Features.Notifications.Services;

public interface INotificationsService
{
    Task<PendingCount> GetPendingCount(int custId);
}

public class NotificationsService : INotificationsService
{
    public Task<PendingCount> GetPendingCount(int custId)
    {
        Random random = new Random();
        int pendingDeposit = random.Next(0, 120);
        int pendingWithdrawal = random.Next(0, 120);
        int pendingPromotion = random.Next(0, 120);

        return Task.FromResult(new PendingCount
        {
            PendingDeposit = pendingDeposit,
            PendingWithdrawal = pendingWithdrawal,
            PendingPromotion = pendingPromotion
        });
    }
}