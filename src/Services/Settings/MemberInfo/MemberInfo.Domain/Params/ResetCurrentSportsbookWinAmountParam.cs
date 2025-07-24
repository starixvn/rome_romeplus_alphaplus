namespace AlphaPlus.Settings.MemberInfo.Domain.Params
{
    public class ResetCurrentSportsbookWinAmountParam
    {
        public ResetCurrentSportsbookWinAmountParam(
            short siteDB,
            int custId,
            string username,
            string language,
            int settingStatus)
        {
            SiteDB = siteDB;
            CustId = custId;
            Username = username;
            Language = language;
            SettingStatus = settingStatus;
        }

        public short SiteDB { get; set; }

        public int CustId { get; set; }

        public string Username { get; set; }

        public string Language { get; set; }

        public int SettingStatus { get; set; }
    }
} 