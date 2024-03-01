namespace Alpha.MemberInfo.Site.Features.Shared.Requests
{
    using Alpha.MemberInfo.Site.Requests;
    using AlphaPlus.Settings.MemberInfo.Domain.Models;

    public class BaseRequest : MemberInfoBaseQuery
    {
        public BaseRequest(IUser viewer) : base(viewer)
        {
        }
    }
}