using AlphaPlus.Settings.Site.Features.MemberInfo.Features.CopyAccount.Models;
using AlphaPlus.Settings.Site.Features.MemberInfo.Features.CopyAccount.Request;
using AlphaPlus.Settings.Site.Features.MemberInfo.Features.CopyAccount.ViewModels;
using Newtonsoft.Json;
using System.Text;
using AlphaPlus.Settings.MemberInfo.API.Features.Shared;

namespace AlphaPlus.Settings.Site.Features.MemberInfo.Features.CopyAccount
{
    public interface ICopyAccountService : ICustomerService
    {
        Task<CopyAccountResponse> CreateAccount(CopyAccountCommand command, int sourceCustId);

        Task<CreditInfo> GetCreditInfo(string username);

        Task<string> GetSuggestUsername(string actorName, byte roleId);

        Task<bool> CheckExistedCustomer(string username);

        Task<Downline> GetDownlineByUsername(int custId, int subAccountId, string username);

        Task<bool> IsCustomerEnabled(int custId);
    }

    public class CopyAccountService : CustomerService, ICopyAccountService
    {
        public CopyAccountService(HttpClient httpClient) 
            : base(httpClient)
        {
        }

        public async Task<CopyAccountResponse> CreateAccount(CopyAccountCommand command, int sourceCustId)
        {
            var payload = new
            {
                actorId = command.ViewerId,
                actorSubAccId = command.SubAccId,
                actorSubAccName = command.SubUsername,
                actorUserLevel = command.ViewerLevelId,
                userName = command.Username,
                password = command.Password,
                credit = command.Credit,
                maxCredit = command.MemberMaxCredit,
                sourceCustId,
                sourceUsername = command.SourceUsername,
                numberOfAccounts = command.NumberOfAccounts,
                language = command.Language,
                ip = command.IP,
                systemId = 2,
                actorUsername = command.ActorUsername
            };

            return await PostAsJsonAsync<CopyAccountResponse>("api/customer", command.SiteDBGroupIp, payload);
        }

        public Task<CreditInfo> GetCreditInfo(string username)
            => httpClient.GetAsJsonAsync<CreditInfo>($"api/customer/{username}/credit");

        private async Task<T> PostAsJsonAsync<T>(string requestUri, string siteDBGroupIp, object payload = null)
        {
            using (var stringContent = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json"))
            {
                using (var request = new HttpRequestMessage(HttpMethod.Post, requestUri) { Content = stringContent })
                {
                    request.Headers.Add("X-SiteDBServer", siteDBGroupIp);
                    using (var response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false))
                    {
                        response.EnsureSuccessStatusCode();
                        return JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync());
                    }
                }
            }
        }

        public Task<string> GetSuggestUsername(string actorName, byte roleId)
        {
            return httpClient.PostAsJsonAsync<string>("api/customer/suggest-username", new { actorName, roleId });
        }

        public Task<bool> CheckExistedCustomer(string username)
        {
            return httpClient.GetAsJsonAsync<bool>($"api/customer/{username}/exist");
        }

        public Task<Downline> GetDownlineByUsername(int custId, int subAccountId, string username)
        {
            return httpClient.GetAsJsonAsync<Downline>($"api/customer/{custId}/{subAccountId}/downlines/{username}");
        }

        public Task<bool> IsCustomerEnabled(int custId)
        {
            return httpClient.GetAsJsonAsync<bool>($"api/customer/{custId}/IsEnabled");
        }
    }
}