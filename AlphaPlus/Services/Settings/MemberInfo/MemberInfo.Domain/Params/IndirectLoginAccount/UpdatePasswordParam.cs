namespace AlphaPlus.Settings.MemberInfo.Domain.Params.IndirectLoginAccount
{

    public class UpdatePasswordParam
    {
        public UpdatePasswordParam(
            int indirectCustId,
            int indirectSubAccId,
            string indirectUsername,
            byte indirectLevelId,
            string password,
            string hashKey,
            int actorId,
            int actorSubAccId,
            string actorUsername,
            string languageKey,
            RequestInfo.Models.RequestInfo requestInfo)
        {
            IndirectCustId = indirectCustId;
            IndirectSubAccId = indirectSubAccId;
            IndirectUsername = indirectUsername;
            IndirectLevelId = indirectLevelId;
            Password = password;
            HashKey = hashKey;
            ActorId = actorId;
            ActorSubAccId = actorSubAccId;
            ActorUsername = actorUsername;
            LanguageKey = languageKey;
            RequestInfo = requestInfo;
        }

        public int IndirectCustId { get; }

        public int IndirectSubAccId { get; }

        public string IndirectUsername { get; }

        public byte IndirectLevelId { get; }

        public string Password { get; set; }

        public string HashKey { get; }

        public bool IsSkipPassword
            => string.IsNullOrWhiteSpace(Password);

        public int ActorId { get; }

        public int ActorSubAccId { get; }

        public string ActorUsername { get; }

        public string LanguageKey { get; }

        public RequestInfo.Models.RequestInfo RequestInfo { get; }
    }
}
