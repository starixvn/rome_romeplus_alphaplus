namespace AlphaPlus.Settings.Site.Features.MemberInfo.Features.Shared.Requests
{
    using AlphaPlus.Settings.Site.Features.MemberInfo.Requests;
    using AlphaPlus.Settings.MemberInfo.Domain.Models;

    public class BaseRequest : MemberInfoBaseQuery
    {
        public BaseRequest() : this(null){}

        public BaseRequest(IUser viewer) : base(viewer)
        {
        }
    }
}