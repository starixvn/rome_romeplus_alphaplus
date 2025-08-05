namespace AlphaPlus.Settings.MemberInfo.Domain.Params
{
    public class ResetCurrentSportsbookWinAmountParam
    {
        public ResetCurrentSportsbookWinAmountParam(
            int custId,
            int productId,
            int sportType,
            int betType,
            int subAccId,
            string subAccName,
            int userId,
            string ip,
            int settingStatus,
            string username,
            string language,
            short siteDB)
        {
            CustId = custId;
            ProductId = productId;
            SportType = sportType;
            BetType = betType;
            SubAccId = subAccId;
            SubAccName = subAccName;
            UserId = userId;
            IP = ip;
            SettingStatus = settingStatus;
            Username = username;
            Language = language;
            SiteDB = siteDB;
        }

        public int CustId { get; set; }

        public int ProductId { get; set; }

        public int SportType { get; set; }

        public int BetType { get; set; }

        public int SubAccId { get; set; }

        public string SubAccName { get; set; }

        public int UserId { get; set; }

        public string IP { get; set; }

        public int SettingStatus { get; set; }

        public string Username { get; set; }

        public string Language { get; set; }

        public short SiteDB { get; set; }
    }
} 