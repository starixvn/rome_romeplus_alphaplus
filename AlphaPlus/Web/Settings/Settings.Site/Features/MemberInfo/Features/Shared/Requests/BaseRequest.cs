using AlphaPlus.Settings.Site.Features.MemberInfo.Requests;
using AlphaPlus.Settings.MemberInfo.Domain.Models;

namespace AlphaPlus.Settings.Site.Features.MemberInfo.Features.Shared.Requests
{
    public class BaseRequest : MemberInfoBaseQuery
    {
        public BaseRequest() : this(null) { }

        public BaseRequest(IUser viewer) : base(viewer)
        {
        }
    }
}