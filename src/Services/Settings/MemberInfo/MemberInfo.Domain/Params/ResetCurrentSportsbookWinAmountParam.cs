namespace AlphaPlus.Settings.MemberInfo.Domain.Params
{
    public class ResetCurrentSportsbookWinAmountParam
    {
        public ResetCurrentSportsbookWinAmountParam(
            short siteDB,
            int custId,
            int userId,
            string username,
            string language,
            int settingStatus)
        {
            SiteDB = siteDB;
            CustId = custId;
            UserId = userId;
            Username = username;
            Language = language;
            SettingStatus = settingStatus;
        }

        public short SiteDB { get; set; }

        public int CustId { get; set; }

        public int UserId { get; set; }

        public string Username { get; set; }

        public string Language { get; set; }

        public int SettingStatus { get; set; }
    }
} 