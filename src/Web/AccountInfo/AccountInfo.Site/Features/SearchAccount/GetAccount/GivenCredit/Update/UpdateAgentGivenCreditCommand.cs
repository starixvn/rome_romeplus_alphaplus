namespace AlphaPlus.AccountInfo.Site.Features.SearchAccount.GetAccount.GivenCredit.Update
{
    public class UpdateAgentGivenCreditCommand : UpdateSMAGivenCreditCommand
    {
        public UpdateAgentGivenCreditCommand(
            int searchAccountId,
            byte searchAccountRoleId,
            decimal creditAmount,
            int actorId,
            int subAccountId,
            string language,
            string ip,
            decimal accumCreditAllCashMember) : base(
                searchAccountId,
                searchAccountRoleId,
                creditAmount,
                actorId,
                subAccountId,
                language,
                ip)
        {
            AccumCreditAllCashMember = accumCreditAllCashMember;
        }


        public decimal AccumCreditAllCashMember { get; }
    }
}