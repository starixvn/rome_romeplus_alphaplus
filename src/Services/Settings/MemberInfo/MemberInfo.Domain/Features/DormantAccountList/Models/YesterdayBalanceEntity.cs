namespace AlphaPlus.Settings.MemberInfo.Domain.Features.DormantAccountList.Models
{
    public class YesterdayBalanceEntity
    {
        public YesterdayBalanceEntity()
        {
        }

        public YesterdayBalanceEntity(int custId, decimal yesterdayTotalBalance)
        {
            CustId = custId;
            YesterdayTotalBalance = yesterdayTotalBalance;
        }

        public int CustId { get; set; }

        public decimal YesterdayTotalBalance { get; set; }
    }
}
