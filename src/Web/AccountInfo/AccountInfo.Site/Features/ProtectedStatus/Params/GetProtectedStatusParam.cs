namespace AlphaPlus.AccountInfo.Site.Features.ProtectedStatus.Params
{
    public class GetProtectedStatusParam
    {
        public GetProtectedStatusParam(int viewerId, byte viewerLevelId, string custIds)
        {
            ViewerId = viewerId;
            ViewerLevelId = viewerLevelId;
            CustIds = custIds;
        }

        public int ViewerId { get; }

        public byte ViewerLevelId { get; }

        public string CustIds { get; }
    }
}