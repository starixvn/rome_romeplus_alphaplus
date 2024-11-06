using Fanex.Data.Repository;

namespace AlphaPlus.Settings.MemberInfo.API.Criteria.WinLossLimit
{
    public class MapPlayerId3rdToCustIdCriteria : CriteriaBase
    {
        public MapPlayerId3rdToCustIdCriteria(int productId, string playerId, int custId, int mrecommend = 0, int recommend = 0)
        {
            ProductId = productId;
            PlayerId = playerId ?? string.Empty;
            CustId = custId;
            Mrecommend = mrecommend;
            Recommend = recommend;
        }

        public int ProductId { get; }

        public string PlayerId { get; }

        public int CustId { get; }

        public int Mrecommend { get; }

        public int Recommend { get; }

        public override string GetSettingKey()
            => "Age_CustomerSelByPlayerId";

        public override bool IsValid()
            => true;
    }
}
