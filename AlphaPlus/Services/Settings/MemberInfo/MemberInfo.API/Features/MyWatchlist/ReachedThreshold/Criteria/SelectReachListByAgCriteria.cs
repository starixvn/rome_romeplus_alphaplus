namespace AlphaPlus.Settings.MemberInfo.API.Features.MyWatchlist.ReachedThreshold.Criteria
{
    using Fanex.Data.Repository;
    using System;

    public class SelectReachListByAgCriteria : NonQueryCommand
    {
        public SelectReachListByAgCriteria(int agentId, DateTime fromDate, DateTime toDate, byte limitType, int sporttype = 0)
        {
            AgentId = agentId;
            FromDate = fromDate;
            ToDate = toDate;
            LimitType = limitType;
            Sporttype = sporttype;
        }

        public int AgentId { get; }

        public DateTime FromDate { get; }

        public DateTime ToDate { get; }

        public byte LimitType { get; }

        public int Sporttype { get; }

        public string XMLReachList { get; set; }

        public override string GetSettingKey() => "Acc_WLMonitored_Player_ReachList_ByAg";

        public override bool IsValid() => AgentId > 0 && FromDate <= ToDate;
    }
}
