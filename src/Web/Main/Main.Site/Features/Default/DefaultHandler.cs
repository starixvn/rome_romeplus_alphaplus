using AlphaPlus.Core.Configurations;
using AlphaPlus.Core.Helpers;
using AlphaPlus.Core.Models;
using AlphaPlus.Core.Utils;
using AlphaPlus.Main.Site.Features.CustomerProfile;
using AlphaPlus.Main.Site.Features.Default.MenuBar;
using AlphaPlus.Main.Site.Features.Tour;
using AlphaPlus.SecurityCode.Web;
using MediatR;
using Microsoft.Extensions.Options;
using TelegramBot.Client.Models;
using TelegramBot.Client.Services;
using TelegramBot.Constants;

namespace AlphaPlus.Main.Site.Features.Default
{
    public class DefaultHandler :
        IRequestHandler<IndexQuery, IndexViewModel>,
        IRequestHandler<BrowserQuery, BrowserViewModel>
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
        private readonly ISettingService telegramSettingService;
        private readonly IEncryption encryption;
        private readonly Fanex.Logging.ILogger logger;
        private readonly IRewardTourService rewardTourService;
        private readonly IOptions<SessionCookieComparisonOptions> sessionCookieComparisonOptions;
        public DefaultHandler(ISessionManager sessionManager,
            IBrowserHelper browserHelper,
            IProfilePictureService customerProfileService,
            ISettingService telegramSettingService,
            IEncryption encryption,
            Fanex.Logging.ILogger logger,
            IRewardTourService rewardTourService,
            IOptions<SessionCookieComparisonOptions> sessionCookieComparisonOptions)
            : this(sessionManager, browserHelper, null, customerProfileService, telegramSettingService, encryption, logger, rewardTourService, sessionCookieComparisonOptions)
        {
        }

        internal DefaultHandler(
            ISessionManager sessionManager,
            IBrowserHelper browserHelper,
            Func<string> getDefaultUrl,
            IProfilePictureService customerProfileService,
            ISettingService telegramSettingService,
            IEncryption encryption,
            Fanex.Logging.ILogger logger,
            IRewardTourService rewardTourService,
            IOptions<SessionCookieComparisonOptions> sessionCookieComparisonOptions)
        {
            this.sessionManager = sessionManager;
            this.browserHelper = browserHelper;

            this.getDefaultUrl = getDefaultUrl ?? FunctionRegistry.GetDefaultUrl;
            this.customerProfileService = customerProfileService;
            this.telegramSettingService = telegramSettingService;
            this.encryption = encryption;
            this.logger = logger;
            this.rewardTourService = rewardTourService;
            this.sessionCookieComparisonOptions = sessionCookieComparisonOptions;
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
            var isShowRewardTour = ((Core.Models.PageAccess)message.SubPageAccess).Transfer && await rewardTourService.IsShow(message.CustId);

            var viewModel = new IndexViewModel(
                message, encryption.RandomString(4), getDefaultUrl, browserHelper, profile, isShowRewardTour, sessionCookieComparisonOptions);

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
                await logger.ErrorAsync(ex.Message, ex);
                return null;
            }
        }
    }
}