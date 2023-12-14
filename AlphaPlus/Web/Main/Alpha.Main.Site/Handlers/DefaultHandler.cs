using Alpha.Main.ProfilePicture.Services;
using MediatR;
using Alpha.Main.Site.Models.Requests;
using Alpha.Main.Params;
using Alpha.Main.Site.Requests;
using Alpha.Main.Services;
using TelegramBot.Client.Models;
using TelegramBot.Client.Services;
using TelegramBot.Constants;
using Alpha.Main.Site.ViewModels;
using AlphaPlus.Core.Models;
using AlphaPlus.SecurityCode.Web;
using AlphaPlus.Core.Helpers;
using AlphaPlus.Core.Utils;

namespace Alpha.Main.Site.Handlers
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
        private readonly ProfilePictureServices customerProfileService;
        private readonly LogoutService logoutService;
        private readonly ISettingService telegramSettingService;
        private readonly TransferService transferService;
        private readonly IEncryption encryption;
        private readonly Fanex.Logging.ILogger logger;

        public DefaultHandler(ISessionManager sessionManager,
            IBrowserHelper browserHelper,
            LogoutService logoutService,
            ProfilePictureServices customerProfileService,
            ISettingService telegramSettingService,
            TransferService transferService,
            IEncryption encryption,
            Fanex.Logging.ILogger logger)
            : this(sessionManager, browserHelper, logoutService, null, customerProfileService, telegramSettingService, transferService, encryption, logger)
        {

        }

        internal DefaultHandler(
            ISessionManager sessionManager,
            IBrowserHelper browserHelper,
            LogoutService logoutService,
            Func<string> getDefaultUrl,
            ProfilePictureServices customerProfileService,
            ISettingService telegramSettingService,
            TransferService transferService,
            IEncryption encryption,
            Fanex.Logging.ILogger logger)
        {
            this.sessionManager = sessionManager;
            this.browserHelper = browserHelper;
            this.logoutService = logoutService;

            this.getDefaultUrl = getDefaultUrl ?? FunctionRegistry.GetDefaultUrl;
            this.customerProfileService = customerProfileService;
            this.telegramSettingService = telegramSettingService;
            this.transferService = transferService;
            this.encryption = encryption;
            this.logger = logger;
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

            var isEnableAutoTransfer = transferService.IsEnableAutoTransfer(message.Currency, message.CustId);

            var viewModel = new IndexViewModel(
                message, encryption.RandomString(4), getDefaultUrl, browserHelper, profile, isEnableAutoTransfer);

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
                await logoutService.Logout(new LogoutParam(
                    request.OnlineUserId,
                    request.ActorName,
                    request.SubUserName,
                    request.NickName ?? string.Empty,
                    request.ActorLevelId,
                    request.ServerId,
                    " Kicked out: Session Changed"));
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