using AlphaPlus.Core.Models;
using AlphaPlus.Settings.Site.Features.SubAccount.Models;
using AlphaPlus.Settings.Site.Features.SubAccount.Requests;

namespace AlphaPlus.Settings.Site.Features.SubAccount.Services
{
    public interface ISubAccountService
    {
        public Task<IEnumerable<SubAccountViewModel>> GetSubAccounts(SubAccountListRequest subAccountRequest);

        public Task<SubAccountViewModel> GetSubAccount(int custId, int subAccId);

        public Task<Result> UpdateSubAccountInfo(UpdateSubAccountInfoRequest updateSubAccountInfoRequest);

        public Task<Result> UpdateSubAccountPageAccess(UpdateSubAccountPageAccessRequest updateSubAccountPageAccessRequest);

        public Task<Result> AddSubAccount(AddSubAccountRequest addSubAccountRequest);
    }

    public class SubAccountService(HttpClient httpClient) : ISubAccountService
    {
        private const string subAccountApiGroup = "customers";

        private readonly HttpClient httpClient = httpClient;

        public async Task<Result> AddSubAccount(AddSubAccountRequest addSubAccountRequest)
        {
            return await httpClient.PostAsJsonAsync<Result>($"{subAccountApiGroup}/{addSubAccountRequest.CustId}/sub-accounts/add", addSubAccountRequest);
        }

        public async Task<SubAccountViewModel> GetSubAccount(int custId, int subAccId)
        {
            return await httpClient.GetFromJsonAsync<SubAccountViewModel>($"{subAccountApiGroup}/{custId}/sub-accounts/{subAccId}");
        }

        public async Task<IEnumerable<SubAccountViewModel>> GetSubAccounts(SubAccountListRequest subAccountRequest)
        {
            return await httpClient.GetFromJsonAsync<IEnumerable<SubAccountViewModel>>($"{subAccountApiGroup}/{subAccountRequest.CustId}/sub-accounts");
        }

        public async Task<Result> UpdateSubAccountInfo(UpdateSubAccountInfoRequest updateSubAccountInfoRequest)
        {
            return await httpClient.PostAsJsonAsync<Result>($"{subAccountApiGroup}/{updateSubAccountInfoRequest.CustId}/sub-accounts/update-info", updateSubAccountInfoRequest);
        }

        public async Task<Result> UpdateSubAccountPageAccess(UpdateSubAccountPageAccessRequest updateSubAccountPageAccessRequest)
        {
            return await httpClient.PostAsJsonAsync<Result>($"{subAccountApiGroup}/{updateSubAccountPageAccessRequest.CustId}/sub-accounts/update-pageaccess", updateSubAccountPageAccessRequest); ;
        }
    }
}