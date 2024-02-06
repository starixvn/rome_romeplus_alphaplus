using AlphaPlus.Core.Models;
using AlphaPlus.Settings.Site.SubAccount.Models;
using AlphaPlus.Settings.Site.SubAccount.Requests;

namespace AlphaPlus.Settings.Site.SubAccount.Services
{
    public interface ISubAccountService
    {
        public Task<IEnumerable<SubAccountViewModel>> GetSubAccounts(SubAccountListRequest subAccountRequest);

        public Task<SubAccountViewModel> GetSubAccount(int custId, int subAccId);

        public Task<Result> UpdateSubAccountInfo(UpdateSubAccountInfoRequest updateSubAccountInfoRequest);

        public Task<Result> UpdateSubAccountPassword(UpdateSubAccountPasswordRequest updateSubAccountPasswordRequest);

        public Task<Result> UpdateSubAccountPageAccess(UpdateSubAccountPageAccessRequest updateSubAccountPageAccessRequest);

        public Task<Result> UpdateSubAccountSecurityCode(UpdateSubAccountSecurityCodeRequest updateSubAccountSecurityCodeRequest);

        public Task<Result> RemoveSubAccountOtp(RemoveSubAccountOtpRequest removeSubAccountOtpRequest);

        public Task<Result> AddSubAccount(AddSubAccountRequest addSubAccountRequest);
    }

    public class SubAccountService : ISubAccountService
    {
        private const string subAccountApiGroup = "api/sub-account";

        private readonly HttpClient httpClient;

        public SubAccountService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<Result> AddSubAccount(AddSubAccountRequest addSubAccountRequest)
        {
            return await httpClient.PostAsJsonAsync<Result>($"{subAccountApiGroup}/add", addSubAccountRequest);
        }

        public async Task<SubAccountViewModel> GetSubAccount(int custId, int subAccId)
        {
            var queryString = QueryString.Create(new[]
            {
                KeyValuePair.Create("custId", custId.ToString()),
                KeyValuePair.Create("subAccId", subAccId.ToString())
            });

            return await httpClient.GetFromJsonAsync<SubAccountViewModel>($"{subAccountApiGroup}/get{queryString}");
        }

        public async Task<IEnumerable<SubAccountViewModel>> GetSubAccounts(SubAccountListRequest subAccountRequest)
        {
            var queryString = QueryString.Create(new[]
            {
                KeyValuePair.Create("custId", subAccountRequest.CustId.ToString())
            });

            return await httpClient.GetFromJsonAsync<IEnumerable<SubAccountViewModel>>($"{subAccountApiGroup}/index{queryString}");
        }

        public async Task<Result> RemoveSubAccountOtp(RemoveSubAccountOtpRequest removeSubAccountOtpRequest)
        {
            return await httpClient.PostAsJsonAsync<Result>($"api/customer/otp/remove", removeSubAccountOtpRequest);;
        }

        public async Task<Result> UpdateSubAccountInfo(UpdateSubAccountInfoRequest updateSubAccountInfoRequest)
        {
            return await httpClient.PostAsJsonAsync<Result>($"{subAccountApiGroup}/update-info", updateSubAccountInfoRequest);
        }

        public async Task<Result> UpdateSubAccountPageAccess(UpdateSubAccountPageAccessRequest updateSubAccountPageAccessRequest)
        {
            return await httpClient.PostAsJsonAsync<Result>($"{subAccountApiGroup}/update-pageaccess", updateSubAccountPageAccessRequest);;
        }

        public async Task<Result> UpdateSubAccountPassword(UpdateSubAccountPasswordRequest updateSubAccountPasswordRequest)
        {
            return await httpClient.PostAsJsonAsync<Result>($"{subAccountApiGroup}/password/reset", updateSubAccountPasswordRequest);;
        }

        public async Task<Result> UpdateSubAccountSecurityCode(UpdateSubAccountSecurityCodeRequest updateSubAccountSecurityCodeRequest)
        {
            return await httpClient.PostAsJsonAsync<Result>($"api/security-code/verify", updateSubAccountSecurityCodeRequest);;
        }
    }
}