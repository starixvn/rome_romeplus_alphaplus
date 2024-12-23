namespace AlphaPlus.AccountInfo.Site.Features.SearchAccount.GetAccount.Balance
{
    public class MasterBalanceInfoModel : IBalanceInfoModel
    {
        public MasterBalanceInfoModel()
        {
        }

        public MasterBalanceInfoModel(
             int custId,
             decimal masterCash,
             decimal yesterdayMasterCash,
             decimal masterPositionTaking,
             decimal agentCash,
             decimal yesterdayAgentCash,
             decimal customerCash,
             decimal yesterdayCustomerCash,
             decimal customerOutstanding,
             decimal pendingTransfer3rd,
             decimal? rewardReceivable,
             decimal? rewardPayable,
             string rewardCurrency)

        {
            CustId = custId;

            MasterCash = masterCash;
            YesterdayMasterCash = yesterdayMasterCash;
            MasterPositionTaking = masterPositionTaking;

            AgentCash = agentCash;
            YesterdayAgentCash = yesterdayAgentCash;

            CustomerCash = customerCash;
            YesterdayCustomerCash = yesterdayCustomerCash;
            CustomerOutstanding = customerOutstanding;
            PendingTransfer3rd = pendingTransfer3rd;

            RewardReceivable = rewardReceivable;
            RewardPayable = rewardPayable;
            RewardCurrency = rewardCurrency;
        }

        public int CustId { get; set; }

        public decimal MasterCash { get; set; }

        public decimal YesterdayMasterCash { get; set; }

        public decimal MasterPositionTaking { get; set; }

        public decimal AgentCash { get; set; }

        public decimal YesterdayAgentCash { get; set; }

        public decimal CustomerCash { get; set; }

        public decimal YesterdayCustomerCash { get; set; }

        public decimal CustomerOutstanding { get; set; }

        public decimal PendingTransfer3rd { get; set; }

        public decimal? RewardReceivable { get; set; }

        public decimal? RewardPayable { get; set; }

        public string RewardCurrency { get; set; }
    }
}