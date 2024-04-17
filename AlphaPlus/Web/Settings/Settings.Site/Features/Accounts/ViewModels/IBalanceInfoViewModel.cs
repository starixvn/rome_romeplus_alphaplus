namespace AlphaPlus.Settings.Site.Features.Accounts.ViewModels
{
    public interface IBalanceInfoViewModel
    {
        decimal TotalBalance { get; }

        decimal YesterdayBalance { get; }

        // string TotalBalanceTransferLink { get; }

        // string YesterdayBalanceTransferLink { get; }

        decimal GivenCredit { get; }

        decimal GivenCreditMin { get; }

        decimal GivenCreditMax { get; }

        decimal PendingTransfer3rd { get; }
    }
}