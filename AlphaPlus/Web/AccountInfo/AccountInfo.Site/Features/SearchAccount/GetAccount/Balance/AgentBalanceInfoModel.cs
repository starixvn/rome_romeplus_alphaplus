namespace AlphaPlus.AccountInfo.Site.Features.SearchAccount.GetAccount.Balance
{
    public class AgentBalanceInfoModel : IBalanceInfoModel
    {
        public AgentBalanceInfoModel()
        {
        }

        public AgentBalanceInfoModel(
             int custId,
             decimal agentCash,
             decimal yesterdayAgentCash,
             decimal customerCash,
             decimal yesterdayCustomerCash,
             decimal customerOutstanding,
             decimal agentPositionTaking,
             decimal pendingTransfer3rd,
             decimal? rewardReceivable,
             decimal? rewardPayable,
             string rewardCurrency)
        {
            CustId = custId;

            AgentCash = agentCash;
            YesterdayAgentCash = yesterdayAgentCash;

            CustomerCash = customerCash;
            YesterdayCustomerCash = yesterdayCustomerCash;
            CustomerOutstanding = customerOutstanding;

            AgentPositionTaking = agentPositionTaking;
            PendingTransfer3rd = pendingTransfer3rd;

            RewardReceivable = rewardReceivable;
            RewardPayable = rewardPayable;
            RewardCurrency = rewardCurrency;
        }

        public int CustId { get; set; }

        public decimal AgentCash { get; set; }

        public decimal YesterdayAgentCash { get; set; }

        public decimal CustomerCash { get; set; }

        public decimal YesterdayCustomerCash { get; set; }

        public decimal AgentPositionTaking { get; set; }

        public decimal CustomerOutstanding { get; set; }

        public decimal PendingTransfer3rd { get; set; }

        public decimal? RewardReceivable { get; set; }

        public decimal? RewardPayable { get; set; }

        public string RewardCurrency { get; set; }
    }
}