using Alpha.MemberInfo.Site.Features.CreateAccount.Models;
using Newtonsoft.Json.Linq;
using AlphaPlus.Settings.MemberInfo.API.Features.Shared;
using AlphaPlus.Settings.MemberInfo.Domain.Configurations;

namespace Alpha.MemberInfo.Site.Features.CreateAccount
{
    public interface ICreateAccountService : ICustomerService
    {
        Task<string> GetCustomerInfo(int custId, byte roleId, string language);

        Task<string> CreateAccount(int creatorId, int subAccId, string ip, string language, string uiBehavior, GeneralInfo generalInfo, string productGroups);

        Task<string> GetPTTemplates(int custId);

        Task<string> CreatePTTemplate(int custId, string templateName, bool isDefault, IEnumerable<PTTemplateSetting> settings, int subAccId, string language, string ip);

        Task<string> GetPTSettings(int custId, byte roleId, string site, int currency, string language);

        Task<string> EditPTTemplate(int custId, int templateId, bool isDefault, IEnumerable<PTTemplateSetting> settings, int subAccId, string language, string ip);

        Task<string> DeletePTTemplate(int custId, int templateId, int subAccId, string language, string ip);
    }

    public class CreateAccountService : CustomerService, ICreateAccountService
    {
        public CreateAccountService(HttpClient httpClient) : base(httpClient)
        {
        }

        public Task<string> GetCustomerInfo(int custId, byte roleId, string language)
            => httpClient.PostAsStringAsync($"api/customer/query",
                new
                {
                    UplineCustId = custId,
                    UplineRoleId = roleId,
                    Language = language
                });

        public Task<string> GetPTTemplates(int custId)
            => httpClient.GetStringAsync($"api/customer/{custId}/pt-templates");

        public Task<string> GetPTSettings(int custId, byte roleId, string site, int currency, string language)
            => httpClient.PostAsStringAsync("api/customer/pt-settings", new { custId, roleId, site, currency, language });

        public Task<string> CreatePTTemplate(int custId, string templateName, bool isDefault, IEnumerable<PTTemplateSetting> settings, int subAccId, string language, string ip)
            => httpClient.PostAsStringAsync("api/customer/pt-templates", new
            {
                custId,
                templateName,
                isDefault,
                settings,
                subAccId,
                language,
                ip
            });

        public Task<string> EditPTTemplate(int custId, int templateId, bool isDefault, IEnumerable<PTTemplateSetting> settings, int subAccId, string language, string ip)
            => httpClient.PutAsStringAsync("api/customer/pt-templates", new
            {
                custId,
                templateId,
                isDefault,
                settings,
                subAccId,
                language,
                ip
            });

        public Task<string> DeletePTTemplate(int custId, int templateId, int subAccId, string language, string ip)
            => httpClient.PutAsStringAsync("api/customer/pt-templates/delete", new
            {
                custId,
                templateId,
                subAccId,
                language,
                ip
            });

        public Task<string> CreateAccount(int creatorId, int subAccId, string ip, string language, string uiBehavior, GeneralInfo generalInfo, string productGroups)
        {
            return httpClient.PostAsStringAsync("api/customer/create", new
            {
                context = new { creatorId, subAccId, ip, systemId = AppSettings.SystemId, language, uiBehavior = JObject.Parse(uiBehavior) },
                settings = new { generalInfo, productGroups = JArray.Parse(productGroups) }
            });
        }
    }
}