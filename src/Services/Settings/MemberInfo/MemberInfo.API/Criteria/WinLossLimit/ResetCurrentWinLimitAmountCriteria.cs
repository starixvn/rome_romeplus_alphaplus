using Fanex.Data;
using Fanex.Data.Repository;

namespace AlphaPlus.Settings.MemberInfo.API.Criteria.WinLossLimit
{
    public class ResetCurrentWinLimitAmountCriteria : NonQueryCommand
    {
        public ResetCurrentWinLimitAmountCriteria(
            int custId,
            string username,
            int productId,
            int sportType,
            int betType,
            string language)
        {
            CustId = custId;
            Username = username;
            ProductId = productId;
            SportType = sportType;
            BetType = betType;
            Language = language;
        }

        public int CustId { get; }

        public string Username { get; }

        public int ProductId { get; }

        public int SportType { get; }

        public int BetType { get; }

        public string Language { get; }

        public int ErrCode { get; }

        public string ErrMsg { get; }

        public override string GetSettingKey()
            => "SiteDB_Age_CustomerResetCurrentWinlimitAmount";

        public override bool IsValid()
            => CustId > 0 && ProductId > 0;
    }
} 