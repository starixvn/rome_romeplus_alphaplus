using Fanex.Data.Repository;

namespace AlphaPlus.Settings.MemberInfo.API.Features.MyWatchlist.MyWatchlistAccounts.Criteria
{
    public class UpdatePersonalThresholdCommand : NonQueryCommand
    {
        public UpdatePersonalThresholdCommand(
            int custId,
            decimal? winningThreshold,
            decimal? losingThreshold,
            int userId,
            int subAccId,
            string ip,
            int sporttype = 0)
        {
            CustId = custId;
            WinningThreshold = winningThreshold;
            LosingThreshold = losingThreshold;
            UserId = userId;
            SubAccId = subAccId;
            IP = ip;
            Sporttype = sporttype;
        }

        public int CustId { get; }

        public int Sporttype { get; }

        public decimal? WinningThreshold { get; }

        public decimal? LosingThreshold { get; }

        public int UserId { get; }

        public int SubAccId { get; }

        public string IP { get; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public override string GetSettingKey() => "Acc_WLMonitored_Personal_Upd";

        public override bool IsValid() => CustId > 0;
    }
}