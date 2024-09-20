using AlphaPlus.Core.Models;
using AlphaPlus.Main.Site.Shared.Services;
using ONEbook.OTP.Helpers;
using ONEbook.OTP.Services;
using PureOtp.Types;

namespace AlphaPlus.Main.Site.Features.OTP
{
    public interface ICustomerOtpService
    {
        Task<Result> VerifyOtp(CustomerOtp param);

        Task<Result> SetUpOtp(CustomerOtp param);

        Task<Result> RemoveSecretKey(RemoveSecretKeyParam param);

        Task<Result> ChangeOtpStatus(ChangeOtpStatusParam param);

        CustomerOtp GenerateCustomerOtp(int custId, string username, int subAccId = 0);

        Task<CustomerOtp> GetCustomerOtp(int custId, int subAccId = 0);

        Task<bool> IsEnabledOtp(int custId, int subAccId = 0);

        Task<bool> IsDifferenceDevice(int custId, string deviceId, int subAccId = 0);
    }

    public class CustomerOtpService(HttpClient httpClient, OtpService otpService, OtpAdapter otpAdapter, INetThrottlingService netThrottlingService) : ICustomerOtpService
    {
        private const int TimePeriod = 30;
        private const int OtpSize = 6;
        private const int SecretKeyLength = 20;
        private const string Issuer = "MyAppName";

        private readonly HttpClient httpClient = httpClient;
        private readonly OtpService otpService = otpService;
        private readonly OtpAdapter otpAdapter = otpAdapter;
        private readonly INetThrottlingService netThrottlingService = netThrottlingService;

        public async Task<Result> ChangeOtpStatus(ChangeOtpStatusParam param)
        {
            if (netThrottlingService.IsBlacklist())
            {
                return Result.CreateErrorResult("Too many requests");
            }

            var result = await httpClient.PostAsJsonAsync<Result>("customer/otp/change-status", param);
            if (result.IsSuccessful)
            {
                netThrottlingService.Throttle();
            }

            return result;
        }


        public CustomerOtp GenerateCustomerOtp(int custId, string username, int subAccId = 0)
        {
            var customerOtp = new CustomerOtp
            {
                SecretKey = otpService.GenerateSecretKey(SecretKeyLength),
                CustId = custId,
                SubAccId = subAccId,
                OtpUsername = username
            };
            customerOtp.OTPUrl = otpAdapter.GenerateTotpUrl(customerOtp.SecretKey, username, Issuer, TimePeriod, OtpHashMode.Sha1, OtpSize);

            return customerOtp;
        }

        public Task<CustomerOtp> GetCustomerOtp(int custId, int subAccId = 0)
            => httpClient.GetAsJsonAsync<CustomerOtp>($"customer/otp?custId={custId}&subAccId={subAccId}");

        public Task<bool> IsDifferenceDevice(int custId, string deviceId, int subAccId = 0)
            => httpClient.PostAsJsonAsync<bool>("customer/otp/isdifferent-device", new { custId, deviceId, subAccId });

        public Task<bool> IsEnabledOtp(int custId, int subAccId = 0)
            => httpClient.PostAsJsonAsync<bool>("customer/otp/isenabled", new { custId, subAccId });

        public Task<Result> RemoveSecretKey(RemoveSecretKeyParam param)
            => httpClient.PostAsJsonAsync<Result>("customer/otp/remove", param);

        public Task<Result> SetUpOtp(CustomerOtp param)
            => httpClient.PostAsJsonAsync<Result>("customer/otp/setup", param);

        public Task<Result> VerifyOtp(CustomerOtp param)
            => httpClient.PostAsJsonAsync<Result>("customer/otp/verify", param);
    }
}