using Fanex.Data.Repository;

namespace AlphaPlus.Settings.MemberInfo.API.Criteria.WinLossLimit
{
    public class GetPotentialWinLimitCriteria : CriteriaBase
    {
        public int AgentId { get; set; }
        public DateTime MinDate { get; set; }
        
        public string Username { get; set; }
        public byte ProblemAccount { get; set; }
        public string Status { get; set; }
        public bool IsGetAll { get; set; }
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
        public int TotalRows { get; set; }

        public override string GetSettingKey()
            => "Age_CustomerSelPotentialWinlimit";

        public override bool IsValid()
            => true;
    }
}

