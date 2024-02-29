namespace AlphaPlus.Settings.MemberInfo.Domain.Params.IndirectLoginAccount
{
    using AlphaPlus.Settings.MemberInfo.Domain.Models.IndirectLoginAccount;

    public class NewAccountParam
    {
        public NewAccountParam(
            string custName,
            string password,
            string hashKey,
            int actorId,
            int actorSubAccId,
            byte actorLevelId,
            string languageKey,
            RequestInfo requestInfo)
        {
            CustName = custName;
            Password = password;
            HashKey = hashKey;
            ActorId = actorId;
            ActorSubAccId = actorSubAccId;
            ActorLevelId = actorLevelId;
            LanguageKey = languageKey;
            RequestInfo = requestInfo;
        }

        public string CustName { get; }

        public string Password { get; set; }

        public string HashKey { get; }

        public int ActorId { get; }

        public int ActorSubAccId { get; }

        public byte ActorLevelId { get; }

        public string LanguageKey { get; }

        public RequestInfo RequestInfo { get; }
    }
}
