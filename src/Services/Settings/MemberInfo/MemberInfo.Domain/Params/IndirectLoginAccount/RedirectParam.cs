namespace AlphaPlus.Settings.MemberInfo.Domain.Params.IndirectLoginAccount
{
    public class RedirectParam
    {
        public RedirectParam(
            int indirectCustId,
            int indirectSubAccId,
            string language,
            int actorId,
            int actorSubAccId,
            string actorUsername,
            string indirectUsername,
            string darkModeName,
            string assetAppPath)
        {
            IndirectCustId = indirectCustId;
            IndirectSubAccId = indirectSubAccId;
            Language = language;
            ActorId = actorId;
            ActorSubAccId = actorSubAccId;
            ActorUsername = actorUsername;
            IndirectUsername = indirectUsername;
            DarkModeName = darkModeName;
            AssetAppPath = assetAppPath;
        }

        public int IndirectCustId { get; }

        public int IndirectSubAccId { get; }

        public string Language { get; }

        public int ActorId { get; }

        public int ActorSubAccId { get; }

        public string ActorUsername { get; }

        public string IndirectUsername { get; }

        public string DarkModeName { get; }

        public string AssetAppPath { get; }
    }
}
