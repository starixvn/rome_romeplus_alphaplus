namespace AlphaPlus.Settings.Site.Features.AccountInfo.Criteria
{
    public class UpdateDisableProtectedStatusCommand : UpdateCustomerStatusCommand
    {
        public UpdateDisableProtectedStatusCommand(
            int searchAccountId,
            int actorId,
            int subAccountId,
            string language,
            string ip,
            string statusName,
            bool statusValue) : base(searchAccountId, actorId, subAccountId, language, ip, statusName)
        {
            IsProtectionMode = statusValue;
        }

        public bool IsProtectionMode { get; }
    }
}