namespace AlphaPlus.Settings.MemberInfo.API.Features.QRCodeMemberSignIn.Criteria
{
    using Fanex.Data.Repository;

    public class CheckExistedCustomerCritieria : NonQueryCommand
    {
        public CheckExistedCustomerCritieria(string username)
        {
            Username = username;
        }

        public string Username { get; }

        public bool IsExists { get; set; }

        public override string GetSettingKey() => "Age_CustomerCheckExists";

        public override bool IsValid() => !string.IsNullOrEmpty(Username);
    }
}
