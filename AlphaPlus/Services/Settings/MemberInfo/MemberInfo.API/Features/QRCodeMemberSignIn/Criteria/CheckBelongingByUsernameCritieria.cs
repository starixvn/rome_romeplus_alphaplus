namespace AlphaPlus.Settings.MemberInfo.API.Features.QRCodeMemberSignIn.Criteria
{
    using Fanex.Data;
    using Fanex.Data.Repository;

    public class CheckBelongingByUsernameCritieria : NonQueryCommand
    {
        public CheckBelongingByUsernameCritieria(int actorId, string username, int subAccountId)
        {
            ActorId = actorId;
            Username = username;
            SubAccountId = subAccountId;
        }

        [SpParam(SqlParamName = "uplineID")]
        public int ActorId { get; }

        public string Username { get; }

        public int SubAccountId { get; }

        public bool IsActive { get; set; }

        public int CustId { get; set; }

        public bool IsSubAcc { get; set; }

        public int RoleId { get; set; }

        public override string GetSettingKey() => "Age_CustomerSelCheckBelonging_ByUsername";

        public override bool IsValid() => ActorId > 0 && !string.IsNullOrEmpty(Username);
    }
}
