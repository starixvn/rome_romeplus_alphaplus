namespace AlphaPlus.Settings.MemberInfo.Domain.Features.QRCodeMemberSignIn.Models
{
    public class PasswordModel
    {
        public PasswordModel(string rawPassword, string encodePassword)
        {
            RawPassword = rawPassword;
            EncodePassword = encodePassword;
        }

        public string RawPassword { get; }

        public string EncodePassword { get; }
    }
}
