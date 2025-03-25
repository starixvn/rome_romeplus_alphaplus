using MediatR;
using TelegramBot.Client.Models;
using TelegramBot.Client.Services;
using TelegramBot.Constants;
using AlphaPlus.Core.Models;
using AlphaPlus.SecurityCode.Web;
using AlphaPlus.Core.Helpers;
using AlphaPlus.Core.Utils;
using AlphaPlus.Main.Site.Features.Default.MenuBar;
using AlphaPlus.Main.Site.Features.Account;
using AlphaPlus.Main.Site.Features.CustomerProfile;
using AlphaPlus.Main.Site.Features.Tour;

namespace AlphaPlus.Main.Site.Features.Default
{
    public class DefaultHandler :
        IRequestHandler<IndexQuery, IndexViewModel>,
        IRequestHandler<BrowserQuery, BrowserViewModel>,
        IRequestHandler<KickOutQuery, bool>
    {
        /// <summary>
        /// Please do not change this value, because it was shared with old main site.
        /// </summary>
        private const string PasswordHashSessionKey = "ph";

        private const string SawBrowserWarningPageSessionKey = "DefaultHandler.SawBrowserWarningPageSessionKey";

        private readonly ISessionManager sessionManager;
        private readonly IBrowserHelper browserHelper;
        private readonly Func<string> getDefaultUrl;
        private readonly IProfilePictureService customerProfileService;
        private readonly IAccountService accountService;
        private readonly ISettingService telegramSettingService;
        private readonly IEncryption encryption;
        private readonly Fanex.Logging.ILogger logger;
        private readonly IRewardTourService rewardTourService;

        public DefaultHandler(ISessionManager sessionManager,
            IBrowserHelper browserHelper,
            IAccountService accountService,
            IProfilePictureService customerProfileService,
            ISettingService telegramSettingService,
            IEncryption encryption,
            Fanex.Logging.ILogger logger,
            IRewardTourService rewardTourService)
            : this(sessionManager, browserHelper, accountService, null, customerProfileService, telegramSettingService, encryption, logger, rewardTourService)
        {
        }

        internal DefaultHandler(
            ISessionManager sessionManager,
            IBrowserHelper browserHelper,
            IAccountService accountService,
            Func<string> getDefaultUrl,
            IProfilePictureService customerProfileService,
            ISettingService telegramSettingService,
            IEncryption encryption,
            Fanex.Logging.ILogger logger,
            IRewardTourService rewardTourService)
        {
            this.sessionManager = sessionManager;
            this.browserHelper = browserHelper;
            this.accountService = accountService;

            this.getDefaultUrl = getDefaultUrl ?? FunctionRegistry.GetDefaultUrl;
            this.customerProfileService = customerProfileService;
            this.telegramSettingService = telegramSettingService;
            this.encryption = encryption;
            this.logger = logger;
            this.rewardTourService = rewardTourService;
        }

        public Task<BrowserViewModel> Handle(BrowserQuery request, CancellationToken cancellationToken)
        {
            if (sessionManager.Get<bool>(SawBrowserWarningPageSessionKey))
            {
                return Task.FromResult(new BrowserViewModel());
            }

            var browser = browserHelper.CheckSupportedBrowsers();
            browser.UserLevel = Enumeration.FromValue<UserLevel>(request.UserLevelId);

            sessionManager.Add(SawBrowserWarningPageSessionKey, true);

            return Task.FromResult(browser);
        }

        public async Task<IndexViewModel> Handle(IndexQuery message, CancellationToken cancellationToken)
        {
            var profile = await customerProfileService.GetActorProfilePicture(message.CustId);
            var isShowRewardTour = ((PageAccess)message.SubPageAccess).Transfer && await rewardTourService.IsShow(message.CustId);

            var viewModel = new IndexViewModel(
                message, encryption.RandomString(4), getDefaultUrl, browserHelper, profile, isShowRewardTour);

            var custBotMessenger = await GetCustBotMessenger(message.CustId, message.SubAccId, message.Language);
            if (custBotMessenger == null)
            {
                viewModel.IsShowSuggestEnableTelegramOtp = false;
            }
            else
            {
                var otpFuntion = custBotMessenger.Functions.FirstOrDefault(func => func.FunctionId == FunctionIds.OTP);
                if (!otpFuntion.Enabled && SecurityCodeSessionManager.SecurityCodeSession.IsEnabledOtp)
                {
                    viewModel.IsShowSuggestEnableTelegramOtp = true;
                }
            }

            sessionManager.Add(PasswordHashSessionKey, viewModel.PasswordHash);

            return viewModel;
        }

        private async Task<CustBotMessenger> GetCustBotMessenger(int custId, int subAccId, string language)
        {
            try
            {
                return await telegramSettingService.GetCustBotMessengerAsync(custId, subAccId, language);
            }
            catch (Exception ex)
            {
                await Fanex.Logging.Logger.Log.ErrorAsync(ex.Message, ex);
                return null;
            }
        }

        public async Task<bool> Handle(KickOutQuery request, CancellationToken cancellationToken)
        {
            try
            {
                await accountService.CheckOut(" Kicked out: Session Changed");
            }
            catch (Exception ex)
            {
                await logger.ErrorAsync(ex.Message, ex);
                return false;
            }

            return true;
        }
    }
}