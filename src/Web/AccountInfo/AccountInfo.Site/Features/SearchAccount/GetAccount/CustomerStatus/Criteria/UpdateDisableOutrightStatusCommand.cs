namespace AlphaPlus.AccountInfo.Site.Features.SearchAccount.GetAccount.CustomerStatus.Criteria
{
    public class UpdateDisableOutrightStatusCommand : UpdateCustomerStatusCommand
    {
        public UpdateDisableOutrightStatusCommand(
            int searchAccountId,
            int actorId,
            int subAccountId,
            string language,
            string ip,
            string statusName,
            bool statusValue) : base(searchAccountId, actorId, subAccountId, language, ip, statusName)
        {
            DisableOutright = statusValue;
        }

        public bool DisableOutright { get; }
    }
}