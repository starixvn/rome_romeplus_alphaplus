namespace AlphaPlus.Settings.MemberInfo.API.Features.QRCodeMemberSignIn.Criteria
{
    using Fanex.Data;
    using Fanex.Data.Repository;

    public class SelectRnGStatusCriteria : NonQueryCommand
    {
        public SelectRnGStatusCriteria(string username)
        {
            Username = username;
        }

        [SpParam(SqlParamName = "strUserName")]
        public string Username { get; }

        public override string GetSettingKey() => "Acc_CustomerSelRnGStatus";

        public override bool IsValid() => !string.IsNullOrEmpty(Username);
    }
}
