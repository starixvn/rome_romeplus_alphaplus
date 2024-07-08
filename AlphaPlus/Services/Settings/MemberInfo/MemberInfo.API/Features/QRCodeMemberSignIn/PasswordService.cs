namespace AlphaPlus.Settings.MemberInfo.API.Features.QRCodeMemberSignIn
{
    using AlphaPlus.Core.Utils;
    using AlphaPlus.Settings.MemberInfo.Domain.Features.QRCodeMemberSignIn.Models;
    using System;
    using System.Text.RegularExpressions;

    public interface PasswordService
    {
        PasswordModel CreateMemberPasswordRandom();
    }

    public class PasswordServiceImpl : PasswordService
    {
        private readonly IEncryption encryption;
        private static readonly Regex MemberPasswordRule = new Regex(@"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{6,10}$"); private static readonly Regex NoWhiteSpace = new Regex(@"^\S*$");

        public PasswordServiceImpl(IEncryption encryption)
        {
            this.encryption = encryption;
        }

        public PasswordModel CreateMemberPasswordRandom()
        {
            var rawPassword = CreateRandomPassword();
            return new PasswordModel(rawPassword, encryption.CfsEnCode(rawPassword));
        }

        internal bool IsMatchedRules(string password)
            => MemberPasswordRule.Match(password).Success &&
               NoWhiteSpace.Match(password).Success;

        internal string CreateRandomPassword(int length = 8)
        {
            const string validChars = "ABCDEFGHJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            Random random = new Random();

            var isValidPass = false;

            char[] chars = new char[length];

            string password = string.Empty;

            while (!isValidPass)
            {
                for (int i = 0; i < length; i++)
                {
                    chars[i] = validChars[random.Next(0, validChars.Length)];
                }

                password = new string(chars);

                if (!IsMatchedRules(password) ||
                    string.Compare(password, "59D7!4$h", StringComparison.OrdinalIgnoreCase) == 0
                    || string.Compare(password, "493abcDE", StringComparison.OrdinalIgnoreCase) == 0)
                {
                    isValidPass = false;
                    Array.Clear(chars, 0, length);
                }
                else
                {
                    isValidPass = true;
                }
            }

            return password;
        }
    }
}
