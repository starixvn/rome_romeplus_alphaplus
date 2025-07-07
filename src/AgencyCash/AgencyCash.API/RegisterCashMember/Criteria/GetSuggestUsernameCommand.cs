using Fanex.Data;
using Fanex.Data.Repository;

namespace AgencyCash.API.RegisterCashMember.Criteria
{
    public class GetSuggestUsernameCommand(string actorName, byte roleId) : NonQueryCommand
    {
        [SpParam(SqlParamName = "Prefix")]
        public string ActorName { get; } = actorName;
        public byte RoleId { get; } = roleId;
        public string SeqName { get; set; }
        public int ErrCode { get; set; }
        public string ErrMsg { get; set; }

        public override string GetSettingKey() => "Age_CustomerSelSuggestUsernameNew";
        public override bool IsValid() => !string.IsNullOrEmpty(ActorName);
    }
}
