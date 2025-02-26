namespace AlphaPlus.AccountInfo.Site.Features.SearchAccount.GetAccount.CustomerStatus
{
    public class UpdateCustomerStatusParam
    {
        public UpdateCustomerStatusParam(
            int searchAccountId,
            int actorId,
            int subAccountId,
            string language,
            string ip,
            bool statusValue,
            string statusName,
            string actorName)
        {
            SearchAccountId = searchAccountId;
            ActorId = actorId;
            SubAccountId = subAccountId;
            Language = language;
            IP = ip;
            StatusValue = statusValue;
            StatusName = statusName;
            ActorName = actorName;
        }

        public int SearchAccountId { get; }

        public int ActorId { get; }

        public int SubAccountId { get; }

        public string Language { get; }

        public string IP { get; }

        public bool StatusValue { get; }

        public string StatusName { get; }

        public string ActorName { get; }
    }
}