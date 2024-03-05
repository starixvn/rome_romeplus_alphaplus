namespace AlphaPlus.Settings.Site.Features.Shared.Requests
{
    using AlphaPlus.Settings.Site.Requests;
    using AlphaPlus.Settings.MemberInfo.Domain.Models;

    public class BaseRequest : MemberInfoBaseQuery
    {
        public BaseRequest() : this(null){}

        public BaseRequest(IUser viewer) : base(viewer)
        {
        }
    }
}