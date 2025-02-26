using Fanex.Data.Repository;

namespace AlphaPlus.Main.Site.Features.NovaCasino.Criteria
{
    public class GetUplineInfoCriteria : CriteriaBase
    {
        public GetUplineInfoCriteria(int custId)
        {
            CustId = custId;
        }

        public int CustId { get; }

        public override string GetSettingKey() => "Age_CustomerSelUplineInfo";

        public override bool IsValid() => CustId > 0;
    }
}