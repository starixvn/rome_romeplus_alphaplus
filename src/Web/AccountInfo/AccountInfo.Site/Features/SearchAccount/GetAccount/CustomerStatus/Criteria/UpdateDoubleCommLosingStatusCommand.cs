namespace AlphaPlus.AccountInfo.Site.Features.SearchAccount.GetAccount.CustomerStatus.Criteria
{
    public class UpdateDoubleCommLosingStatusCommand : UpdateCustomerStatusCommand
    {
        public UpdateDoubleCommLosingStatusCommand(
            int searchAccountId,
            int actorId,
            int subAccountId,
            string language,
            string ip,
            string statusName,
            bool statusValue) : base(searchAccountId, actorId, subAccountId, language, ip, statusName)
        {
            DoubleCommLosing = statusValue;
        }

        public bool DoubleCommLosing { get; }
    }
}