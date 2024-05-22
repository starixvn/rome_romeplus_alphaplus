namespace AlphaPlus.Settings.Site.Features.CustomerList.Criteria
{
    public class UpdateUnlockedCommand : UpdateCustomerStatusCommand
    {
        public UpdateUnlockedCommand(
            int searchAccountId,
            int actorId,
            int subAccountId,
            string language,
            string ip,
            string statusName) : base(searchAccountId, actorId, subAccountId, language, ip, statusName)
        {
        }
    }
}