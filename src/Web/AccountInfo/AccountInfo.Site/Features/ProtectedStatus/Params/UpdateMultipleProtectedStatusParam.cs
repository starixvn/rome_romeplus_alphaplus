namespace AlphaPlus.AccountInfo.Site.Features.ProtectedStatus.Params
{
    public class UpdateMultipleProtectedStatusParam
    {
        public UpdateMultipleProtectedStatusParam(
            CustomerProtectedStatus[] protectedStatus,
            int actorId,
            string actorName,
            int subAccId,
            string langKey,
            string ip,
            bool isInternal,
            int serverId)
        {
            ProtectedStatus = protectedStatus;
            ActorId = actorId;
            ActorName = actorName;
            SubAccId = subAccId;
            LangKey = langKey;
            IP = ip;
            IsInternal = isInternal;
            ServerId = serverId;
        }

        public CustomerProtectedStatus[] ProtectedStatus { get; }

        public int ActorId { get; }

        public string ActorName { get; }

        public int SubAccId { get; }

        public string LangKey { get; }

        public string IP { get; }

        public bool IsInternal { get; }

        public int ServerId { get; }
    }
}