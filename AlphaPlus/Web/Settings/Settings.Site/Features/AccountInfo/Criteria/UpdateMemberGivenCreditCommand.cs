namespace AlphaPlus.Settings.Site.Features.AccountInfo.Criteria
{
    public class UpdateMemberGivenCreditCommand : UpdateSMAGivenCreditCommand
    {
        public UpdateMemberGivenCreditCommand(
            int searchAccountId,
            byte searchAccountRoleId,
            decimal creditAmount,
            int actorId,
            int subAccountId,
            string language,
            string ip,
            decimal totalAmt) : base(
                searchAccountId,
                searchAccountRoleId,
                creditAmount,
                actorId,
                subAccountId,
                language,
                ip)
        {
            TotalAmt = totalAmt;
        }

        public decimal TotalAmt { get; }
    }
}