using AlphaPlus.Core.Models;
using AlphaPlus.Main.Site.AppResources;
using MediatR;
using TelegramBot.Client.Services;
using Function = TelegramBot.Client.Models.Function;
using AlphaPlus.SecurityCode.Services;
using AlphaPlus.SecurityCode.Web;
using AlphaPlus.Core.Helpers;
using AlphaPlus.Main.Site.Features.TelegramChatBot;
using AlphaPlus.Main.Site.Shared.Models;

namespace AlphaPlus.Main.Site.Features.OTP
{
    using Fanex.Logging;
    using AlphaPlus.Main.Site.Shared.Constants;


    public class CustomerOtpHandler :
        IRequestHandler<SetUpOtpQuery, OtpViewModel>,
        IRequestHandler<ResetOtpQuery, ResetOtpViewModel>,
        IRequestHandler<ChangeOtpStatusCommand, Result>,
        IRequestHandler<RemoveOtpCommand, Result>,
        IRequestHandler<ResetOtpCommand, ResetOtpViewModel>,
        IRequestHandler<SetUpOtpCommand, Result>,
        IRequestHandler<VerifyOtpCommand, Result>,
        IRequestHandler<VerifyOtpDifferentDeviceQuery, VerifyOtpDifferenceDeviceViewModel>,
        IRequestHandler<CheckDifferenceDeviceCommand, bool>,
        IRequestHandler<VerifyOtpQuery, VerifyOTPViewModel>,
        IRequestHandler<GenerateOTPCommand, Result>,
        IRequestHandler<UpdateOtpTelegramCommand, Result>
    {
        private const string SecretKeySessionKey = "CustomerOtpHandler.SecretKeySessionKey";
        private const int UnlinkedErrorCode = 102;

        private readonly ICustomerOtpService customerOtpService;
        private readonly IUserSession userSession;
        private readonly ISessionManager sessionManager;
        private readonly IBrowserHelper browserHelper;
        private readonly IUser user;
        private readonly ISettingService telegramSettingService;
        private readonly ISecurityCodeChecker securityCodeChecker;
        private readonly IOTPService telegramOTPService;
        private readonly ILogger logger;

        public CustomerOtpHandler(
            ICustomerOtpService customerOtpService,
            IUserSession userSession,
            ISessionManager sessionManager,
            IBrowserHelper browserHelper,
            IUser user,
            ISettingService telegramSettingService,
            ISecurityCodeChecker securityCodeChecker,
            IOTPService telegramOTPService,
            ILogger logger)
        {
            this.customerOtpService = customerOtpService;
            this.userSession = userSession;
            this.sessionManager = sessionManager;
            this.browserHelper = browserHelper;
            this.user = user;
            this.telegramSettingService = telegramSettingService;
            this.securityCodeChecker = securityCodeChecker;
            this.telegramOTPService = telegramOTPService;
            this.logger = logger;
        }

        public async Task<Result> Handle(ChangeOtpStatusCommand request, CancellationToken cancellationToken)
        {

            if (!HasUpdatePermission())
            {
                return Result.CreateErrorResult(Security.NotAuthorized);
            }

            var result = await customerOtpService.ChangeOtpStatus(new ChangeOtpStatusParam
            {
                CustId = request.CustId,
                ActorId = request.CustId,
                IP = request.IP,
                Language = request.Language,
                IsEnable = request.IsEnable,
                SubAccId = request.SubAccountId
            });

            if (result.IsSuccessful)
            {
                var enabled = user.IsEnabledTelegramOTP || request.IsEnable;
                SetOtpStatusToSession(enabled);
            }

            return result;
        }

        public async Task<ResetOtpViewModel> Handle(ResetOtpCommand request, CancellationToken cancellationToken)
        {
            var viewModel = new ResetOtpViewModel
            {
                CustId = request.CustId,
                SubAccountId = request.SubAccountId,
                Username = request.Username
            };

            if (!HasUpdatePermission(true, request.SubAccountId > 0))
            {
                viewModel.Result = Result.CreateErrorResult(Security.NotAuthorized);
            }
            else
            {
                var deviceInfo = browserHelper.GetDevice().Info;
                var result = await customerOtpService.RemoveSecretKey(new RemoveSecretKeyParam
                {
                    CustId = request.CustId,
                    ActorId = request.ActorId,
                    IP = deviceInfo.IP,
                    Language = request.Language,
                    SubAccId = request.SubAccountId,
                    DeviceInfo = deviceInfo,
                    IsResetDownlineOrSubAccount = true
                });

                if (await IsEnabledTelegramOTP(request.CustId, request.SubAccountId))
                {
                    result = await UpdateTelegramOTPStatus(request.CustId, request.SubAccountId, false);
                }

                viewModel.Result = result;
            }

            return viewModel;
        }

        public async Task<Result> Handle(SetUpOtpCommand request, CancellationToken cancellationToken)
        {
            if (!HasUpdatePermission())
            {
                return Result.CreateErrorResult(Security.NotAuthorized);
            }

            var customerOtp = new CustomerOtp
            {
                CustId = request.CustId,
                Otp = request.Otp,
                Language = request.Language,
                SubAccId = request.SubAccountId,
                SecretKey = sessionManager.Get<string>(SecretKeySessionKey),
                Device = browserHelper.GetDevice()
            };

            var result = await customerOtpService.SetUpOtp(customerOtp);

            if (result.IsSuccessful)
            {
                SetVerifiedOtp();
            }

            return result;
        }

        public Task<VerifyOtpDifferenceDeviceViewModel> Handle(VerifyOtpDifferentDeviceQuery request, CancellationToken cancellationToken)
        {

            return Task.FromResult(new VerifyOtpDifferenceDeviceViewModel
            {
                Username = request.UserName,
                AlternateUsername = BuildAlternateUsername(request.IsSubAcc, request.SubUserName, request.NickName),
                UsernameShower = BuildUsernameShower(request.IsSubAcc, request.SubUserName, request.NickName, request.UserName),
                IsShowGenerateOTPButton = user.IsEnabledTelegramOTP
            });
        }

        public async Task<Result> Handle(VerifyOtpCommand request, CancellationToken cancellationToken)
        {

            var deviceInfo = browserHelper.GetDevice();
            var result = await customerOtpService.VerifyOtp(new CustomerOtp
            {
                CustId = request.CustId,
                SubAccId = request.SubAccountId,
                Otp = request.Otp,
                Device = deviceInfo,
                Language = request.Language,
                IsEnabledTelegramOTP = await IsEnabledTelegramOTP(request.CustId, request.SubAccountId)
            });

            if (result.IsSuccessful)
            {
                SetVerifiedOtp();
            }

            return result;
        }

        public async Task<Result> Handle(RemoveOtpCommand request, CancellationToken cancellationToken)
        {
            if (!HasUpdatePermission())
            {
                return Result.CreateErrorResult(Security.NotAuthorized);
            }

            var result = await customerOtpService.RemoveSecretKey(new RemoveSecretKeyParam
            {
                CustId = request.CustId,
                ActorId = request.CustId,
                IP = request.IP,
                Language = request.Language,
                SubAccId = request.SubAccountId,
                DeviceInfo = browserHelper.GetDevice().Info
            });

            return result;
        }

        private async Task<Result> UpdateTelegramOTPStatus(int custId, int subAccId, bool isEnabled)
        {
            var messenger = await telegramSettingService.GetCustBotMessengerAsync(custId, subAccId, "en-US");

            var updateResult = await telegramSettingService.UpdateUserFunction(
                messenger.UserId,
                messenger.BotId,
                new Function { FunctionId = TelegramBot.Constants.FunctionIds.OTP, Enabled = isEnabled },
                userSession.UserId,
                userSession.UserLoginIp);

            return new Result { Code = updateResult.Code, Message = updateResult.IsOk ? Security.RemoveOtpSuccessMessage : Security.FailedToRemoveOtp };
        }

        public Task<ResetOtpViewModel> Handle(ResetOtpQuery request, CancellationToken cancellationToken)
        {
            return Task.FromResult(new ResetOtpViewModel
            {
                CustId = request.SubAccountId > 0 ? userSession.CustId : request.CustId,
                SubAccountId = request.SubAccountId,
                Username = request.Username
            });
        }

        public Task<VerifyOTPViewModel> Handle(VerifyOtpQuery request, CancellationToken cancellationToken)
        {
            return Task.FromResult(new VerifyOTPViewModel
            {
                UsernameShower = BuildUsernameShower(request.IsSubAcc, request.SubUserName, request.NickName, request.UserName),
                IsShowGenerateOTPButton = user.IsEnabledTelegramOTP
            });
        }

        public async Task<OtpViewModel> Handle(SetUpOtpQuery request, CancellationToken cancellationToken)
        {
            var customerOtp = await customerOtpService.GetCustomerOtp(request.CustId, request.SubAccountId);

            var viewModel = new OtpViewModel
            {
                CustomerOtp = customerOtp,
                IsAlreadySetup = true,
                IsOfferProtectedMode = request.IsOfferProtectedMode,
                EnableSecurityCode = securityCodeChecker.IsEnableSecurityCode()
            };

            var custBotMessenger = await telegramSettingService.GetCustBotMessengerAsync(
                request.CustId,
                request.SubAccountId,
                Language.MapNewChineseLanguageCode(request.Language));

            viewModel.RegisterAccountViewModel =
                new RegisterAccountViewModel(new ChatBotViewModel(custBotMessenger), new SetUpChatBotQuery());

            if (!viewModel.CustomerOtp.Enabled
                && string.IsNullOrWhiteSpace(viewModel.CustomerOtp.SecretKey))
            {
                if (string.IsNullOrWhiteSpace(request.Username))
                {
                    request.Username = userSession.IsSubAcc ? userSession.SubUserName : userSession.UserName;
                }

                viewModel.CustomerOtp = await customerOtpService.GenerateCustomerOtp(request.CustId, request.Username, request.SubAccountId);
                viewModel.IsAlreadySetup = false;
                sessionManager.Add(SecretKeySessionKey, viewModel.CustomerOtp.SecretKey);
            }

            return viewModel;
        }

        private bool HasUpdatePermission(
            bool isResetOtp = false,
            bool isResetSubAccount = false)
        {
            if (userSession.IsInternal && !isResetSubAccount)
            {
                return false;
            }

            if (userSession.IsLimitedHostPermissionOnSubAccountFunction)
            {
                return false;
            }

            if (isResetOtp
                && (userSession.IsSubAcc && !userSession.SubPageAccess.UpdateAccount))
            {
                return false;
            }

            if (isResetSubAccount && !user.IsAdminCanUpdateSubAccount)
            {
                return false;
            }

            return true;
        }

        private static string BuildUsernameShower(bool isSubAcc, string subAccName, string nickName, string username)
        {
            if (isSubAcc)
            {
                return subAccName;
            }

            return string.IsNullOrWhiteSpace(nickName) ? username : nickName;
        }

        private static string BuildAlternateUsername(bool isSubAcc, string subAccName, string nickName)
        {
            if (isSubAcc)
            {
                return $"({Resources.subAcc.ToUpper()} {subAccName})";
            }

            return string.IsNullOrWhiteSpace(nickName) ? string.Empty : $"({nickName})";
        }

        public async Task<bool> Handle(CheckDifferenceDeviceCommand request, CancellationToken cancellationToken)
        {

            user.IsEnabledTelegramOTP = await IsEnabledTelegramOTP(request.CustId, request.SubAccountId);

            var enabled = await customerOtpService.IsEnabledOtp(request.CustId, request.SubAccountId) || user.IsEnabledTelegramOTP;

            SetOtpStatusToSession(enabled);

            if (!enabled)
            {
                return false;
            }

            var deviceId = browserHelper.GetDeviceId();

            return await customerOtpService.IsDifferenceDevice(request.CustId, deviceId, request.SubAccountId);
        }

        public async Task<Result> Handle(UpdateOtpTelegramCommand request, CancellationToken cancellationToken)
        {
            var custMessenger = await telegramSettingService.GetCustBotMessengerAsync(request.CustId, request.SubAccId, request.Language);
            if (custMessenger == null || !custMessenger.Enabled || !custMessenger.IsVerifyOTP)
            {
                return new Result(UnlinkedErrorCode);
            }

            var function = new Function
            {
                FunctionId = TelegramBot.Constants.FunctionIds.OTP,
                Enabled = request.Enabled,
            };
            var result = await telegramSettingService.UpdateUserFunction(custMessenger.UserId, custMessenger.BotId, function, request.CustId, request.IP);
            if (result.IsOk)
            {
                user.IsEnabledTelegramOTP = request.Enabled;
                var enabled = await customerOtpService.IsEnabledOtp(request.CustId, request.SubAccId) || user.IsEnabledTelegramOTP;
                SetOtpStatusToSession(enabled);
            }

            return new Result(result.Code, result.message, CancellationToken cancellation);
        }

        private static void SetOtpStatusToSession(bool isEnabledOtp)
        {
            SecurityCodeSessionManager.SecurityCodeSession.IsEnabledOtp = isEnabledOtp;
        }

        private static void SetVerifiedOtp()
        {
            SecurityCodeSessionManager.SecurityCodeSession.IsAlreadyInputSecurityCode = true;
        }

        public async Task<Result> Handle(GenerateOTPCommand request, CancellationToken cancellationToken)
        {
            if (await IsEnabledTelegramOTP(request.CustId, request.SubAccId))
            {
                await telegramOTPService.SendOTP(request.CustId, request.SubAccId);
                return Result.CreateSuccessfulResult();
            }
            else
            {
                return Result.CreateErrorResult(Security.OTPViaTelegramDisabled);
            }
        }

        private async Task<bool> IsEnabledTelegramOTP(int custId, int subAccId)
        {
            try
            {
                return await telegramSettingService.IsEnableOTP(custId, subAccId);
            }
            catch (Exception ex)
            {
                await logger.ErrorAsync(ex.Message, ex);
                return false;
            }
        }
    }
}