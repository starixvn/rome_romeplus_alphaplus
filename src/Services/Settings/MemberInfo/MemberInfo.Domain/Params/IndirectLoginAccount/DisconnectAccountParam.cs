namespace AlphaPlus.Settings.MemberInfo.Domain.Params.IndirectLoginAccount
{
    public class DisconnectAccountParam
    {
        public DisconnectAccountParam(
            int indirectCustId,
            int indirectSubAccId,
            string indirectUsername,
            int actorId,
            int actorSubAccId,
            string actorUsername,
            string languageKey,
            string ip)
        {
            IndirectCustId = indirectCustId;
            IndirectSubAccId = indirectSubAccId;
            IndirectUsername = indirectUsername;
            ActorId = actorId;
            ActorSubAccId = actorSubAccId;
            ActorUsername = actorUsername;
            LanguageKey = languageKey;
            IP = ip;
        }

        public int IndirectCustId { get; }

        public int IndirectSubAccId { get; }

        public string IndirectUsername { get; }

        public int ActorId { get; }

        public int ActorSubAccId { get; }

        public string ActorUsername { get; }

        public string LanguageKey { get; }

        public string IP { get; }
    }
}
