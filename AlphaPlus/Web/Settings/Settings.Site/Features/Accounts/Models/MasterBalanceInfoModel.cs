namespace AlphaPlus.Settings.Site.Features.Accounts.Models
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
             decimal pendingTransfer3rd)

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
    }
}