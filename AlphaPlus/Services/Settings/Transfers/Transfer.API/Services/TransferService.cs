namespace AlphaPlus.Settings.Transfer.API.Services
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using AlphaPlus.Settings.Transfer.API.Criteria;
    using AlphaPlus.Settings.Transfer.API.Models;
    using AlphaPlus.Core.Models;
    using Fanex.Data.Repository;

    public interface TransferService
    {
        bool IsAllDayBalanceTransfer(int custId);

        Task<bool> IsAllDayBalanceTransferAsync(int custId);

        bool IsEnableAutoTransfer(
            int custId,
            string currency,
            bool isSubAcc,
            PageAccess subPageAccess,
            HashSet<string> offerCurrencies);

        Task<bool> IsEnableAutoTransfer(
            int custId,
            string currency,
            PageAccess subPageAccess,
            HashSet<string> offerCurrencies);
    }

    public class TransferServiceImpl : TransferService
    {
        private readonly IDynamicRepository dynamicRepository;

        public TransferServiceImpl(IDynamicRepository dynamicRepository)
            => this.dynamicRepository = dynamicRepository;

        public bool IsAllDayBalanceTransfer(int custId)
        {
            var transferModel = dynamicRepository.Get<TransferModel>(
                new GetIsAllDayBalTransferCriteria(custId));

            return transferModel != null && transferModel.IsAllDayBalTransfer;
        }

        public async Task<bool> IsAllDayBalanceTransferAsync(int custId)
        {
            var transferModel = await dynamicRepository.GetAsync<TransferModel>(
                new GetIsAllDayBalTransferCriteria(custId));

            return transferModel != null && transferModel.IsAllDayBalTransfer;
        }

        public bool IsEnableAutoTransfer(
            int custId,
            string currency,
            bool isSubAcc,
            PageAccess subPageAccess,
            HashSet<string> offerCurrencies)
        {
            if (isSubAcc && !subPageAccess.Transfer)
            {
                return false;
            }

            return offerCurrencies.Contains("*") || offerCurrencies.Contains(currency);
        }

        public async Task<bool> IsEnableAutoTransfer(
            int custId,
            string currency,
            PageAccess subPageAccess,
            HashSet<string> offerCurrencies)
        {
            if (!subPageAccess.Transfer)
            {
                return false;
            }

            if (offerCurrencies.Contains("*") || offerCurrencies.Contains(currency))
            {
                return !await IsAllDayBalanceTransferAsync(custId);
            }

            return false;
        }
    }
}
