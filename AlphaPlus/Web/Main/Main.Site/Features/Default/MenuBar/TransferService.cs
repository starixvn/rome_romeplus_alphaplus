using Fanex.Data.Repository;

namespace AlphaPlus.Main.Site.Features.Default.MenuBar
{
    public interface ITransferService
    {
        bool IsAllDayBalanceTransfer(int custId);

        bool IsEnableAutoTransfer(string currency, int custId);
    }

    public class TransferService(IDynamicRepository dynamicRepository) : ITransferService
    {
        private readonly IDynamicRepository dynamicRepository = dynamicRepository;

        public bool IsAllDayBalanceTransfer(int custId)
        {
            var transferModel = dynamicRepository.Get<TransferModel>(new GetIsAllDayBalTransferCriteria(custId));

            return transferModel != null && transferModel.IsAllDayBalTransfer;
        }

        public bool IsEnableAutoTransfer(string currency, int custId)
        {
            if (IsAllDayBalanceTransfer(custId))
            {
                return false;
            }

            var offerCurrencies = AlphaPlus.Core.Configurations.ConfigurationManager.GetValue<string>("OfferAutoTransfer.Currencies");

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
