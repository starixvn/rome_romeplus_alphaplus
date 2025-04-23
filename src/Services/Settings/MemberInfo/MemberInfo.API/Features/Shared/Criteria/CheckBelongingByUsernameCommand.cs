using Fanex.Data;
using Fanex.Data.Repository;

namespace AlphaPlus.Settings.MemberInfo.API.Features.Shared.Criteria
{
    public class CheckBelongingByUsernameCommand(int actorId, string username, int subAccountId) : NonQueryCommand
    {
        [SpParam(SqlParamName = "uplineID")]
        public int ActorId { get; } = actorId;

        public string Username { get; } = username;

        public int SubAccountId { get; } = subAccountId;

        public bool IsActive { get; set; }

        public int CustId { get; set; }

        public bool IsSubAcc { get; set; }

        public byte RoleId { get; set; }

        public override string GetSettingKey() => "Age_CustomerSelCheckBelonging_ByUsername";

        public override bool IsValid() => ActorId > 0 && !string.IsNullOrEmpty(Username);
    }
}