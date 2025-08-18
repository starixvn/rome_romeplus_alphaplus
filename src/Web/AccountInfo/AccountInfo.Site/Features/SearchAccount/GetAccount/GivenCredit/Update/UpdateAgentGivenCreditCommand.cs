namespace AlphaPlus.AccountInfo.Site.Features.SearchAccount.GetAccount.GivenCredit.Update
{
    public class UpdateAgentGivenCreditCommand : UpdateMasterGivenCreditCommand
    {
        public UpdateAgentGivenCreditCommand(
            int searchAccountId,
            decimal creditAmount,
            int actorId,
            int subAccountId,
            string language,
            string ip,
            decimal accumCreditAllCashMember) : base(
                searchAccountId,
                creditAmount,
                actorId,
                subAccountId,
                language,
                ip)
        {
            AccumCreditAllCashMember = accumCreditAllCashMember;
        }

        public decimal AccumCreditAllCashMember { get; }

        public override string GetSettingKey() => "Age_CustomerUpdCredit_MAg";
    }
}