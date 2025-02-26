namespace AlphaPlus.Settings.Site.Features.CustomerList.Criteria
{
    public class UpdateSuspendedStatusCommand : UpdateCustomerStatusCommand
    {
        public UpdateSuspendedStatusCommand(
            int searchAccountId,
            int actorId,
            int subAccountId,
            string language,
            string ip,
            string statusName,
            bool statusValue) : base(searchAccountId, actorId, subAccountId, language, ip, statusName)
        {
            Suspended = statusValue;
        }

        public bool Suspended { get; }
    }
}