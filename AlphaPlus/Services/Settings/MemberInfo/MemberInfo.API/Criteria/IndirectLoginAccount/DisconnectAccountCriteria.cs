namespace AlphaPlus.Settings.MemberInfo.API.Criteria.IndirectLoginAccount
{
    using Fanex.Data;
    using Fanex.Data.Repository;

    public class DisconnectAccountCriteria : NonQueryCommand
    {
        public DisconnectAccountCriteria(
            int indirectCustId,
            int indirectSubAccId,
            string indirectUsername,
            int actorId,
            int actorSubAccId,
            string actorUsername,
            string language,
            string ip)
        {
            IndirectCustId = indirectCustId;
            IndirectSubAccId = indirectSubAccId;
            IndirectUsername = indirectUsername;
            ActorId = actorId;
            ActorSubAccId = actorSubAccId;
            ActorUsername = actorUsername;
            Language = language;
            IP = ip;
        }

        [SpParam(SqlParamName = "custidindirect")]
        public int IndirectCustId { get; }

        [SpParam(SqlParamName = "subaccidindirect")]
        public int IndirectSubAccId { get; }

        [SpParam(SqlParamName = "downlineusername")]
        public string IndirectUsername { get; }

        [SpParam(SqlParamName = "custid")]
        public int ActorId { get; }

        [SpParam(SqlParamName = "subaccid")]
        public int ActorSubAccId { get; }

        [SpParam(SqlParamName = "uplineusername")]
        public string ActorUsername { get; }

        public string IP { get; }

        public string Language { get; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public override string GetSettingKey()
            => "Age_CustomerIndirectLoginAccountDel";

        public override bool IsValid()
            => IndirectCustId > 0
            && IndirectSubAccId >= 0
            && !string.IsNullOrWhiteSpace(IndirectUsername);
    }
}
