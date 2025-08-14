namespace AlphaPlus.AccountInfo.Site.Features.SearchAccount.GetAccount.GivenCredit.Update
{
    public class UpdateMemberGivenCreditCommand : UpdateAgentGivenCreditCommand
    {
        public UpdateMemberGivenCreditCommand(
            int searchAccountId,
            decimal creditAmount,
            int actorId,
            int subAccountId,
            string language,
            string ip,
            decimal accumCreditAllCashMember,
            decimal totalAmt) : base(
                searchAccountId,
                creditAmount,
                actorId,
                subAccountId,
                language,
                ip,
                accumCreditAllCashMember)
        {
            TotalAmt = totalAmt;
        }

        public decimal TotalAmt { get; }

        public override string GetSettingKey() => "Age_CustomerUpdCredit_Ag";
    }
}