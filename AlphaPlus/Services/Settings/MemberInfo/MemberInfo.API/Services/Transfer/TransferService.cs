namespace AlphaPlus.Settings.MemberInfo.API.Services.Transfer
{
    using System;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using AlphaPlus.Shared.AppResources.Settings.MemberInfo;
    using AlphaPlus.Settings.MemberInfo.Domain.Models;
    using Criteria.Transfer;
    using Fanex.Data.Repository;

    public interface TransferService
    {
        Task<string> GetAllowTransfer(int custId);

        Task<bool> IsAllDayBalanceTransferAsync(int custId);

        bool IsAllDayBalanceTransfer(int custId);
        bool IsAutoTransferAffect(IUser viewer, string offerCurrencies);
        bool IsEnableAutoTransfer(IUser viewer, string offerCurrencies);
        bool IsAutoTransferAffect(string currency, string offerCurrencies);
    }

    public class TransferServiceImpl : TransferService
    {
        private readonly IDynamicRepository repository;
        private const int DailyIndex = 3;
        private const int WeekyIndex = 7;

        public TransferServiceImpl(IDynamicRepository dynamicRepository)
        {
            repository = dynamicRepository;
        }

        public async Task<string> GetAllowTransfer(int custId)
        {
            string transferOption = await GetTransferOption(custId);

            string transferTime = Lang.AllowTransfer;
            char[] charArray = transferOption.ToCharArray();

            if (charArray.Length > 0)
            {
                StringBuilder dayBuilder = new StringBuilder();
                string[] dayArray =
                {
                    Lang.Sunday,
                    Lang.Monday,
                    Lang.Tuesday,
                    Lang.Wednesday,
                    Lang.Thursday,
                    Lang.Friday,
                    Lang.Saturday
                };

                for (int i = 1, max = charArray.Length; i < max - 1; i++)
                {
                    if (charArray[i] == '1')
                    {
                        dayBuilder.Append(" - ").Append(dayArray[i]);
                    }
                }

                if (charArray[0] == '1')
                {
                    dayBuilder.Append(" - ").Append(dayArray[0]);
                }

                string day = dayBuilder.ToString();

                if (day.Length > 0)
                {
                    day = day.Substring(DailyIndex);
                }

                transferTime += charArray[WeekyIndex] == '1' ?
                    " " + day :
                    " " + Lang.Daily;
            }

            return transferTime;
        }

        private async Task<string> GetTransferOption(int custId)
        {
            var criteria = new GetTransferOptionCriteria(custId);

            await repository.ExecuteAsync(criteria);

            return criteria.TransferOption;
        }

        public async Task<bool> IsAllDayBalanceTransferAsync(int custId)
        {
            var customerTransferStatus = await repository.GetAsync<CustomerTransferStatus>(
                new GetIsAllDayBalTransferCriteria(custId));

            return customerTransferStatus != null && customerTransferStatus.IsAllDayBalTransfer;
        }

        public bool IsAllDayBalanceTransfer(int custId)
        {
            var customerTransferStatus = repository.Get<CustomerTransferStatus>(
                new GetIsAllDayBalTransferCriteria(custId));

            return customerTransferStatus != null && customerTransferStatus.IsAllDayBalTransfer;
        }

        public bool IsAutoTransferAffect(IUser viewer, string offerCurrencies)
        {
            var currency = viewer.Currency;
            if (string.IsNullOrWhiteSpace(offerCurrencies))
            {
                return false;
            }

            if (offerCurrencies == "*")
            {
                return true;
            }

            return offerCurrencies.Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries)
                .Any(c => string.Equals(currency, c.Trim(), StringComparison.OrdinalIgnoreCase));
        }

        public bool IsEnableAutoTransfer(IUser viewer, string offerCurrencies)
        {
            if (IsAllDayBalanceTransfer(viewer.Id)
                || (viewer.IsSubAcc && !viewer.SubPageAccess.Transfer))
            {
                return false;
            }

            return IsAutoTransferAffect(viewer, offerCurrencies);
        }

        public bool IsAutoTransferAffect(string currency, string offerCurrencies)
        {
            if (string.IsNullOrWhiteSpace(offerCurrencies))
            {
                return false;
            }

            if (offerCurrencies == "*")
            {
                return true;
            }

            return offerCurrencies.Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries)
                .Any(c => string.Equals(currency, c.Trim(), StringComparison.OrdinalIgnoreCase));
        }
    }
}
