using Fanex.Data;
using Fanex.Data.Repository;

namespace AlphaPlus.Settings.MemberInfo.API.Criteria.WinLossLimit
{
    public class ResetCurrentSportsbookWinAmountCriteria : NonQueryCommand
    {
        public ResetCurrentSportsbookWinAmountCriteria(short siteDB, int custId, int userId, int settingStatus)
        {
            SiteDB = siteDB;
            CustId = custId;
            UserId = userId;
            SettingStatus = settingStatus;
        }

        [SpParam(Ignored= true)]
        public short SiteDB {get;}

        public int CustId { get; }  

        public int UserId { get; }

        public int SettingStatus { get; }

        public override string GetSettingKey()
            => $"SiteDB_{SiteDB}_Acc_CustWinLimitBetReset_CashMember";

        public override bool IsValid() => CustId > 0;
    }
} 