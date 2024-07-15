namespace AlphaPlus.AccountInfo.Site.Features.SearchAccount.GetAccount.CustomerStatus.Criteria
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