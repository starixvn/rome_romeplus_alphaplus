namespace AlphaPlus.Settings.MemberInfo.Domain.Features.ParlayMaxPayout.Params
{
    public class ParlayMaxPayoutSelectParam
    {
        public ParlayMaxPayoutSelectParam(int custId)
        {
            CustId = custId;
        }

        public int CustId { get; }
    }
}