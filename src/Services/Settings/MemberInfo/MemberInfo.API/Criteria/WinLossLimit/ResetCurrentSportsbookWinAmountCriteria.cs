using AlphaPlus.Settings.MemberInfo.Domain.Params;
using Fanex.Data;
using Fanex.Data.Repository;

namespace AlphaPlus.Settings.MemberInfo.API.Criteria.WinLossLimit
{
    public class ResetCurrentSportsbookWinAmountCriteria : NonQueryCommand
    {
        public ResetCurrentSportsbookWinAmountCriteria(ResetCurrentSportsbookWinAmountParam param)
        {
            CustId = param.CustId;
            ProductId = param.ProductId;
            SportType = param.SportType;
            BetType = param.BetType;
            SubAccId = param.SubAccId;
            SubAccName = param.SubAccName;
            UserId = param.UserId;
            IP = param.IP;
            SettingStatus = param.SettingStatus;
            SiteDB = param.SiteDB;
        }

        public int CustId { get; }

        [SpParam(Ignored= true)]
        public int ProductId { get; }

        [SpParam(Ignored= true)]
        public int SportType { get; }
        
        [SpParam(Ignored= true)]
        public int BetType { get; }

        public int SubAccId { get; }

        public string SubAccName { get; }

        public int UserId { get; }

        public string IP { get; }

        public int SettingStatus { get; }

        [SpParam(Ignored= true)]
        public short SiteDB {get;}

        public override string GetSettingKey()
            => $"SiteDB_{SiteDB}_Acc_CustWinLimitBetReset_CashMember";

        public override bool IsValid() => CustId > 0;
    }
} 