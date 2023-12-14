using Alpha.Main.AppResources;
using Alpha.Main.Criteria.Otp;
using Fanex.Data.Repository;
using Alpha.Main.Helpers;
using Alpha.Main.Models.Otp;
using ONEbook.OTP.Services;
using Alpha.Main.Params.Otp;
using TelegramBot.Client.Services;
using CustomResult = Alpha.Main.Models.Result;
using Fanex.Logging;
using AlphaPlus.Core.Models;

namespace Alpha.Main.Services.Impl
{
    //TODO: net throttling
    public class CustomerOtpServiceImpl : CustomerOtpService
    {
        private const int TimePeriod = 30;
        private const int OtpSize = 6;
        private const int SecretKeyLength = 20;
        private const int DisabledAccountErrorCode = 200;
        private const string Issuer = "MyAppName";
        private const string NotDetectedDevice = "N/A";
        private const int InvalidDataErrorCode = 1;
        private const string InvalidDataMessage = "Data is not valid";
        private const int DownlineIsDisabledErrorCode = 600;
        private const int DownlineDoesNotHaveRegisteredDeviceErrorCode = 100;
        private const int FailedToRemoveDownlineOtpErrorCode = 101;
        private const string CustIdCannotBeNegOrZero = "Cust Id cannot be negative or zero";

        private readonly IDynamicRepository dynamicRepository;
        private readonly OtpService otpService;
        private readonly JsonHelper jsonHelper;
        // private readonly INetThrottlingService netThrottlingService;
        private readonly IOTPService telegramOTPService;
        private readonly ILogger logger;

        public CustomerOtpServiceImpl(
            IDynamicRepository dynamicRepository,
            OtpService otpService,
            JsonHelper jsonHelper,
            // INetThrottlingService netThrottlingService,
            IOTPService telegramOTPService,
            ILogger logger)
        {
            this.dynamicRepository = dynamicRepository;
            this.otpService = otpService;
            this.jsonHelper = jsonHelper;
            // this.netThrottlingService = netThrottlingService;
            this.telegramOTPService = telegramOTPService;
            this.logger = logger;
        }

        public virtual async Task<CustomResult> ChangeOtpStatus(ChangeOtpStatusParam param)
        {
            if (param == null)
            {
                throw new ArgumentNullException(nameof(param), "Change Otp Status Parameters cannot be null");
            }

            // if (netThrottlingService.IsBlacklist())
            // {
            //     return CustomResult.CreateErrorResult("Too many requests");
            // }

            var command = new UpdateCustomerOtpStatusCommand
            {
                CustId = param.CustId,
                SubAccId = param.SubAccId,
                Enable = param.IsEnable,
                IP = param.IP,
                Language = param.Language,
                UserId = param.ActorId,
                ErrCode = InvalidDataErrorCode,
                ErrMsg = InvalidDataMessage
            };

            await dynamicRepository.ExecuteAsync(command);

            if (command.ErrCode == 0)
            {
                // netThrottlingService.Throttle();
            }

            return new CustomResult
            {
                Code = command.ErrCode,
                Message = command.ErrCode == 0 ? Security.UpdateOtpStatusSuccess : command.ErrMsg
            };
        }

        public CustomerOtp GenerateCustomerOtp(int custId, string username, int subAccId = 0)
        {

            var customerOtp = new CustomerOtp
            {
                SecretKey = otpService.GenerateSecretKey(SecretKeyLength),
                CustId = custId,
                SubAccId = subAccId,
                OtpUsername = username,
            };
            customerOtp.QRCode = otpService.GenerateQrCode(customerOtp.SecretKey, TimePeriod, OtpSize, Issuer, customerOtp.OtpUsername);

            return customerOtp;
        }

        public async Task<bool> IsEnabledOtp(int custId, int subAccId = 0)
        {

            var command = new CheckEnableOtpCommand
            {
                CustId = custId,
                SubAccId = subAccId
            };

            await dynamicRepository.ExecuteAsync(command);

            return command.Enable;
        }

        public async Task<CustomResult> SetUpOtp(CustomerOtp param)
        {
            var isCorrect = otpService.VerifyOtp(param.SecretKey, param.Otp, TimePeriod, OtpSize);

            if (!isCorrect)
            {
                return CustomResult.CreateErrorResult(Security.OtpIsInvalidOrExpired);
            }

            var deviceInfoSerialized = jsonHelper.Serialize(param.Device.Info);
            var command = new InsertCustomerOtpCommand
            {
                CustId = param.CustId,
                DeviceId = param.Device.Id,
                DeviceInfo = deviceInfoSerialized,
                IP = param.Device.Info.IP,
                Language = param.Language,
                SecretKey = param.SecretKey,
                SubAccId = param.SubAccId,
                ErrCode = InvalidDataErrorCode,
                ErrMsg = InvalidDataMessage
            };

            await dynamicRepository.ExecuteAsync(command);

            return new CustomResult
            {
                Code = command.ErrCode,
                Message = command.ErrCode == 0 ? Security.AuthenticatorAppVerified : command.ErrMsg
            };
        }

        public async Task<CustomResult> VerifyOtp(CustomerOtp param)
        {
            if (param == null)
            {
                throw new ArgumentNullException(nameof(param), "Verify Otp Param Parameters cannot be null");
            }

            var isCorrect = false;

            if (param.IsEnabledTelegramOTP)
            {
                var verifyResult = await telegramOTPService.VerifyOTP(param.CustId, param.SubAccId, param.Otp);
                isCorrect = verifyResult.IsOk;
            }

            if (!isCorrect)
            {
                var customerOtp = await GetCustomerOtp(param.CustId, param.SubAccId);
                if (customerOtp.Enabled)
                {
                    isCorrect = otpService.VerifyOtp(customerOtp.SecretKey, param.Otp, TimePeriod, OtpSize);
                }
            }

            if (isCorrect)
            {
                await UpdateDevice(param);
            }

            var changeStatusCommand = new UpdateCustomerStatusAfterVerifyOtpCommand
            {
                CheckedOtp = isCorrect,
                IP = param.Device.Info.IP,
                Language = param.Language,
                CustId = param.CustId,
                SubAccId = param.SubAccId,
                ErrCode = InvalidDataErrorCode,
                ErrMsg = InvalidDataMessage
            };

            await dynamicRepository.ExecuteAsync(changeStatusCommand);

            var result = new CustomResult
            {
                Code = changeStatusCommand.ErrCode,
                Message = isCorrect ? changeStatusCommand.ErrMsg : Security.OtpInvalidOrExpiredAndContactUpline
            };

            if (!isCorrect
                && result.Code == DisabledAccountErrorCode)
            {
                result.Message = Security.LockAccountOtpMessage;
            }

            return result;
        }

        public virtual async Task<CustomerOtp> GetCustomerOtp(int custId, int subAccId = 0)
        {
            var customerOtpDto = await dynamicRepository.GetAsync<CustomerOtpDto>(
                new GetCustomerOtpCriteria
                {
                    CustId = custId,
                    SubAccId = subAccId
                });

            if (customerOtpDto == null
                || customerOtpDto.IsDelete)
            {
                return new CustomerOtp();
            }

            return new CustomerOtp
            {
                CustId = customerOtpDto.CustId,
                Enabled = customerOtpDto.Enabled,
                SecretKey = customerOtpDto.SecretKey,
                SubAccId = customerOtpDto.SubAccId
            };
        }

        public async Task<CustomResult> RemoveSecretKey(RemoveSecretKeyParam param)
        {
            var command = new DeleteCustOtpSecretKeyCommand
            {
                CustId = param.CustId,
                SubAccId = param.SubAccId,
                IP = param.IP,
                Language = param.Language,
                UserId = param.ActorId,
                ErrCode = InvalidDataErrorCode,
                ErrMsg = InvalidDataMessage,
                DeviceInfo = jsonHelper.Serialize(param.DeviceInfo)
            };

            await dynamicRepository.ExecuteAsync(command);

            var deleteResult = new CustomResult
            {
                Code = command.ErrCode,
                Message = GetRemovedOtpMessage(command.ErrCode, param.IsResetDownlineOrSubAccount, command.ErrMsg)
            };

            return deleteResult;
        }

        private static string GetRemovedOtpMessage(int code, bool isResetDownlineOrSubAccount, string errorMessage)
        {
            if (isResetDownlineOrSubAccount)
            {
                return code switch
                {
                    DownlineIsDisabledErrorCode => Security.RemoveDisableOtpDownline,
                    DownlineDoesNotHaveRegisteredDeviceErrorCode => Security.NoRegisterDeviceError,
                    FailedToRemoveDownlineOtpErrorCode => Security.FailedToRemoveOtp,
                    0 => Security.RemoveRegisteredDeviceSuccess,
                    _ => errorMessage,
                };
            }

            return code == 0 ? Security.RemoveOtpSuccessMessage : errorMessage;
        }

        private async Task UpdateDevice(CustomerOtp customerOtp)
        {
            var command = new InsertCustomerDeviceInfoCommand
            {
                CustId = customerOtp.CustId,
                DeviceId = customerOtp.Device.Id,
                DeviceInfo = jsonHelper.Serialize(customerOtp.Device.Info),
                IP = customerOtp.Device.Info.IP,
                Language = customerOtp.Language,
                SubAccId = customerOtp.SubAccId
            };

            await dynamicRepository.ExecuteAsync(command);
        }

        public async Task<bool> IsDifferenceDevice(int custId, string deviceId, int subAccId = 0)
        {
            if (deviceId == NotDetectedDevice)
            {
                await logger.InfoAsync("User device was not detected");
                return false;
            }

            var devices = await dynamicRepository.FetchAsync<Device>(
                new GetDevicesCriteria
                {
                    CustId = custId,
                    SubAccId = subAccId
                });

            var currentDevice = devices.FirstOrDefault(device => device.Id == deviceId);

            return currentDevice == null;
        }

        public Task GenerateTelegramOTP(int custId, int subAccId)
        {
            return telegramOTPService.SendOTP(custId, subAccId);
        }
    }
}