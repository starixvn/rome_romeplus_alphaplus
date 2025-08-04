namespace AlphaPlus.AccountInfo.Site.Features.SearchAccount.GetAccount.GivenCredit.Update
{
    public class UpdateMemberGivenCreditCommand : UpdateAgentGivenCreditCommand
    {
        public UpdateMemberGivenCreditCommand(
            int searchAccountId,
            byte searchAccountRoleId,
            decimal creditAmount,
            int actorId,
            int subAccountId,
            string language,
            string ip,
            decimal accumCreditAllCashMember,
            decimal totalAmt) : base(
                searchAccountId,
                searchAccountRoleId,
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
    }
}