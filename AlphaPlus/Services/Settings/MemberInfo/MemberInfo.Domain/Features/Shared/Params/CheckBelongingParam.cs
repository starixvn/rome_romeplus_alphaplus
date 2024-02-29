namespace AlphaPlus.Settings.MemberInfo.Domain.Features.Shared.Params
{
    public class CheckBelongingParam
    {
        public CheckBelongingParam(
            int viewerId,
            int custId,
            int subAccId,
            long transId = 0)
        {
            ViewerId = viewerId;
            CustId = custId;
            SubAccId = subAccId;
            TransId = transId;
        }

        public int ViewerId { get; }

        public int CustId { get; }

        public int SubAccId { get; }

        public long TransId { get; }
    }
}
