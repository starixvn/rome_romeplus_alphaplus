using AlphaPlus.Main.Site.Features.NovaCasino.Models;
using Fanex.Data.Repository;

namespace AlphaPlus.Main.Site.Features.NovaCasino
{
    using System.Globalization;
    using System.Text.Json;
    using AlphaPlus.Main.Site.Features.NovaCasino.Criteria;
    using AlphaPlus.Main.Site.Shared.Constants;
    using AlphaPlus.Shared.AppResources.Main;

    public interface ICustomerService
    {
        Task<IEnumerable<User>> GetUplines(int custId);

        Task UpdateSyncedNovaCasino(string[] custIds, string ip, string language);

        Task InsertPersonalMessage(UserInfo userInfo);
    }

    public class CustomerService(IDynamicRepository dynamicRepository) : ICustomerService
    {
        private readonly IDynamicRepository dynamicRepository = dynamicRepository;

        public Task<IEnumerable<User>> GetUplines(int custId)
            => dynamicRepository.FetchAsync<User>(new GetUplineInfoCriteria(custId));

        public async Task InsertPersonalMessage(UserInfo userInfo)
        {
            var codeKeys = AppSettings.DisabledVietnamese ? Language.CodeNamesWithoutVietNamese : Language.CodeKeys;
            var personalMessages = codeKeys.ToDictionary(p => p.Key, p => string.Format(GetMessageTemplate(p.Key), userInfo.Username));
            var upline = userInfo.Upline.Where(x => x.RoleId != 5);
            if (upline != null && upline.Any())
            {
                var command = new InsertPersonalMessageCommand(
                    JsonSerializer.Serialize(upline),
                    personalMessages[Language.English],
                    personalMessages[Language.ChineseSimplified],
                    personalMessages[Language.ChineseTraditional],
                    personalMessages[Language.Thai],
                    personalMessages.ContainsKey(Language.Vietnamese) ? personalMessages[Language.Vietnamese] : null);
                await dynamicRepository.ExecuteAsync(command);
            }
        }

        public Task UpdateSyncedNovaCasino(string[] custIds, string ip, string language)
        {
            var tasks = new List<Task>();
            foreach (var item in custIds)
            {
                int custId;
                if (int.TryParse(item, out custId))
                {
                    tasks.Add(Task.Factory.StartNew(() =>
                    {
                        var command = new UpdateSyncedNovaCasinoCommand(custId, ip, language);
                        return dynamicRepository.ExecuteAsync(command);
                    }));
                }
            }

            return Task.WhenAll(tasks);
        }

        private static string GetMessageTemplate(string languageCode)
            => Resources.ResourceManager.GetString(nameof(Resources.SyncedNovaCasinoPersonalMessage), new CultureInfo(languageCode));
    }
}