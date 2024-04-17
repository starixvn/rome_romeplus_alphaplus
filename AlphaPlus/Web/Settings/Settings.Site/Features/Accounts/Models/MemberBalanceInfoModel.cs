namespace AlphaPlus.Settings.Site.Features.Accounts.Models
{
    public class MemberBalanceInfoModel : IBalanceInfoModel
    {
        public MemberBalanceInfoModel(
             MemberBalanceInfoDTO memberBalanceInfoDTO,
             decimal balance,
             decimal pendingTransfer3rd,
             decimal customerOutstanding,
             decimal netPosition)
        {
            Balance = balance;
            CustomerOutstanding = customerOutstanding;
            PendingTransfer3rd = pendingTransfer3rd;
            NetPosition = netPosition;

            CustId = memberBalanceInfoDTO.CustId;
            CustomerCash = balance + customerOutstanding + pendingTransfer3rd;
            YesterdayCustomerCash = memberBalanceInfoDTO.YesterdayCustomerCash;
        }

        public int CustId { get; private set; }

        public decimal Balance { get; private set; }

        public decimal NetPosition { get; private set; }

        public decimal CustomerCash { get; private set; }

        public decimal YesterdayCustomerCash { get; private set; }

        public decimal CustomerOutstanding { get; private set; }

        public decimal PendingTransfer3rd { get; private set; }
    }

    public class MemberBalanceInfoDTO
    {
        public MemberBalanceInfoDTO()
        {
        }

        public MemberBalanceInfoDTO(
             int custId,
             decimal credit,
             decimal yesterdayCustomerCash,
             decimal memMaxCreditMax)
        {
            CustId = custId;
            YesterdayCustomerCash = yesterdayCustomerCash;
        }

        public int CustId { get; set; }

        public decimal YesterdayCustomerCash { get; set; }
    }
}