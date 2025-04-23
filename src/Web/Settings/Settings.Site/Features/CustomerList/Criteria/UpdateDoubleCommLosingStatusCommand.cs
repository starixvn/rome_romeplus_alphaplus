namespace AlphaPlus.Settings.Site.Features.CustomerList.Criteria
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