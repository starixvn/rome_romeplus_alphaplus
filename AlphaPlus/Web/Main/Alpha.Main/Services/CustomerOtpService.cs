namespace Alpha.Main.Services
{
    using AlphaPlus.Core.Models;
    using Models.Otp;
    using Params.Otp;
    using System.Threading.Tasks;

    public interface CustomerOtpService
    {
        Task<Result> VerifyOtp(CustomerOtp param);

        Task<Result> SetUpOtp(CustomerOtp param);

        Task<Result> RemoveSecretKey(RemoveSecretKeyParam param);

        Task<Result> ChangeOtpStatus(ChangeOtpStatusParam param);

        CustomerOtp GenerateCustomerOtp(int custId, string username, int subAccId = 0);

        Task<CustomerOtp> GetCustomerOtp(int custId, int subAccId = 0);

        Task<bool> IsEnabledOtp(int custId, int subAccId = 0);

        Task<bool> IsDifferenceDevice(int custId, string deviceId, int subAccId = 0);

        Task GenerateTelegramOTP(int custId, int subAccId);
    }
}