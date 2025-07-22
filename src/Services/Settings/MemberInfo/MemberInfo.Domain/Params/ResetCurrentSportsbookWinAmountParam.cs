namespace AlphaPlus.Settings.MemberInfo.Domain.Params
{
    public class ResetCurrentSportsbookWinAmountParam
    {
        public ResetCurrentSportsbookWinAmountParam(
            int custId,
            string username,
            int productId,
            int sportType,
            int betType,
            string language,
            int settingStatus)
        {
            CustId = custId;
            Username = username;
            ProductId = productId;
            SportType = sportType;
            BetType = betType;
            Language = language;
            SettingStatus = settingStatus;
        }

        public int CustId { get; set; }

        public string Username { get; set; }

        public int ProductId { get; set; }

        public int SportType { get; set; }

        public int BetType { get; set; }

        public string Language { get; set; }

        public int SettingStatus { get; set; }
    }
} 