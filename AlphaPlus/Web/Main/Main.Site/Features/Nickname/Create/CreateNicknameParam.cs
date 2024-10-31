namespace AlphaPlus.Main.Site.Features.Nickname
{
    public class CreateNicknameParam
    {
        public CreateNicknameParam(
            int custId,
            int subAccountId,
            string nickname,
            string ip,
            string languageKey,
            bool isEnableUpdateNickname,
            int custDBGroupId)
        {
            CustId = custId;
            SubAccountId = subAccountId;
            Nickname = nickname;
            IP = ip;
            LanguageKey = languageKey;
            IsEnableUpdateNickname = isEnableUpdateNickname;
            CustDBGroupId = custDBGroupId;
        }

        public int CustId { get; }

        public int SubAccountId { get; }

        public string Nickname { get; }

        public string IP { get; }

        public string LanguageKey { get; }

        public bool IsEnableUpdateNickname { get; }

        public int CustDBGroupId { get; }

        public bool IsSubAccount => SubAccountId > 0;
    }
}
