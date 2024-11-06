using Fanex.Data.Repository;

namespace AlphaPlus.Main.Site.Features.Dashboard.Statistics
{
    public class GetNewMembersCriteria : CriteriaBase
    {
        public GetNewMembersCriteria(int custId)
        {
            CustId = custId;
        }

        public int CustId { get; }

        public override string GetSettingKey() => "Age_CustomerSelNewMembers";

        public override bool IsValid() => CustId > 0;
    }
}