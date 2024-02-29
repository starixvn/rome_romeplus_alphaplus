namespace AlphaPlus.Settings.MemberInfo.API.Features.MyWatchlist.MyWatchlistAccounts.Criteria
{
    using Fanex.Data.Repository;

    public class DeletePersonalThresholdCommand : NonQueryCommand
    {
        public DeletePersonalThresholdCommand(
            int custId,
            int userId,
            int subAccId,
            string ip,
            int sporttype = 0)
        {
            CustId = custId;
            UserId = userId;
            SubAccId = subAccId;
            IP = ip;
            Sporttype = sporttype;
        }

        public int CustId { get; }

        public int Sporttype { get; }

        public int UserId { get; }

        public int SubAccId { get; }

        public string IP { get; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public override string GetSettingKey() => "Acc_WLMonitored_Personal_Del";

        public override bool IsValid() => CustId > 0;
    }
}
