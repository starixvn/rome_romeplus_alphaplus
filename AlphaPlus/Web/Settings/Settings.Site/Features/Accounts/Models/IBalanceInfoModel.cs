namespace AlphaPlus.Settings.Site.Features.Accounts.Models
{
    public interface IBalanceInfoModel
    {
        int CustId { get; }

        decimal CustomerCash { get; }

        decimal YesterdayCustomerCash { get; }

        decimal PendingTransfer3rd { get; }
    }
}