using AlphaPlus.Core.Models;
using AlphaPlus.Settings.MemberInfo.Domain.Extensions;

namespace AlphaPlus.Settings.MemberInfo.API.Features.Shared
{
    public interface ICustomerService
    {
        Task<Result> CheckAllLimitedRules(int custId, byte custLevel, string actorUsername, string language);
    }

    public class CustomerService : ICustomerService
    {
        protected readonly HttpClient httpClient;

        public CustomerService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public Task<Result> CheckAllLimitedRules(int custId, byte custLevel, string actorUsername, string language)
        {
            return httpClient.PostAsJsonAsync<Result>("api/rules/CheckAllLimitedRules", new { custId, custLevel, actorUsername, language });
        }
    }
}
