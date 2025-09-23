namespace AlphaPlus.AccountInfo.Site.Features.SearchAccount.GetAccount.Balance
{
    public interface IBalanceInfoModel
    {
        int CustId { get; }

        decimal CustomerCash { get; }

        decimal YesterdayCustomerCash { get; }

        decimal PendingTransfer3rd { get; }

        decimal CustomerOutstanding { get; }
    }
}