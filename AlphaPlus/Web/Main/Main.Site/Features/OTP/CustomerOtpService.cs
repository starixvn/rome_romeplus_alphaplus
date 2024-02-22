using AlphaPlus.Core.Models;

namespace AlphaPlus.Main.Site.Features.OTP
{
    public interface ICustomerOtpService
    {
        Task<Result> VerifyOtp(CustomerOtp param);

        Task<Result> SetUpOtp(CustomerOtp param);

        Task<Result> RemoveSecretKey(RemoveSecretKeyParam param);

        Task<Result> ChangeOtpStatus(ChangeOtpStatusParam param);

        Task<CustomerOtp> GenerateCustomerOtp(int custId, string username, int subAccId = 0);

        Task<CustomerOtp> GetCustomerOtp(int custId, int subAccId = 0);

        Task<bool> IsEnabledOtp(int custId, int subAccId = 0);

        Task<bool> IsDifferenceDevice(int custId, string deviceId, int subAccId = 0);
    }

    public class CustomerOtpService(HttpClient httpClient) : ICustomerOtpService
    {
        private readonly HttpClient httpClient = httpClient;

        public Task<Result> ChangeOtpStatus(ChangeOtpStatusParam param)
            => httpClient.PostAsJsonAsync<Result>("customer/otp/change-status", param);

        public Task<CustomerOtp> GenerateCustomerOtp(int custId, string username, int subAccId = 0)
            => httpClient.PostAsJsonAsync<CustomerOtp>("customer/otp/generate", new { custId, username, subAccId });

        public Task<CustomerOtp> GetCustomerOtp(int custId, int subAccId = 0)
        {
            throw new NotImplementedException();
        }

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