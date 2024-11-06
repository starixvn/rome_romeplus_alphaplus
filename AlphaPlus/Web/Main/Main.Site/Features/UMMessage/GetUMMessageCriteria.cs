using Fanex.Data.Repository;

namespace AlphaPlus.Main.Site.Features.UMMessage
{
    public class GetUMMessageCriteria : CriteriaBase
    {
        public GetUMMessageCriteria(int custId)
        {
            CustId = custId;
        }

        public int CustId { get; }

        public override string GetSettingKey()
            => "Acc_CheckUM";

        public override bool IsValid()
            => CustId > 0;
    }
}
