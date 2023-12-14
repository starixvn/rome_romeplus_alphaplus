using TelegramBot.Client.Services;

namespace Alpha.Main.Services.Impl
{
    public class TelegramBotServiceImpl(INotificationService notificationService, Fanex.Logging.ILogger logger) : TelegramBotService
    {
        private readonly INotificationService notificationService = notificationService;
        private readonly Fanex.Logging.ILogger logger = logger;

        public void NotifyChangePassword(int custId, int subAccId)
            => NotifyChangePasswordAsync(custId, subAccId).GetAwaiter().GetResult();

        public async Task NotifyChangePasswordAsync(int custId, int subAccId)
        {
            try
            {
                await notificationService.NotifyChangePassword(custId, subAccId);
            }
            catch (Exception ex)
            {
                await logger.ErrorAsync(ex.Message, ex);
            }
        }

        public void NotifyChangeSecurityCode(int custId, int subAccId)
            => NotifyChangeSecurityCodeAsync(custId, subAccId).GetAwaiter().GetResult();

        public async Task NotifyChangeSecurityCodeAsync(int custId, int subAccId)
        {
            try
            {
                await notificationService.NotifyChangeSecurityCode(custId, subAccId);
            }
            catch (Exception ex)
            {
                await logger.ErrorAsync(ex.Message, ex);
            }
        }
    }
}
