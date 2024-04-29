namespace AlphaPlus.Settings.Site.Features.AccountInfo.Models
{
    public class CreditInfo
    {
        public CreditInfo()
        {
        }

        public CreditInfo(
            decimal credit,
            decimal masterCreditMin,
            decimal masterCreditMax,
            decimal masterUsedCredit,
            decimal agentCreditMin,
            decimal agentCreditMax,
            decimal agentUsedCredit,
            decimal memberMaxCredit,
            decimal memMaxCreditMin,
            decimal memMaxCreditMax)
        {
            Credit = credit;
            MasterCreditMin = masterCreditMin;
            MasterCreditMax = masterCreditMax;
            MasterUsedCredit = masterUsedCredit;
            AgentCreditMin = agentCreditMin;
            AgentCreditMax = agentCreditMax;
            AgentUsedCredit = agentUsedCredit;
            MemberMaxCredit = memberMaxCredit;
            MemMaxCreditMin = memMaxCreditMin;
            MemMaxCreditMax = memMaxCreditMax;
        }

        public decimal Credit { get; set; }

        public decimal MasterCreditMin { get; set; }

        public decimal MasterCreditMax { get; set; }

        public decimal MasterUsedCredit { get; set; }

        public decimal AgentCreditMin { get; set; }

        public decimal AgentCreditMax { get; set; }

        public decimal AgentUsedCredit { get; set; }

        public decimal MemberMaxCredit { get; set; }

        public decimal MemMaxCreditMin { get; set; }

        public decimal MemMaxCreditMax { get; set; }
    }
}