namespace AlphaPlus.Settings.Site.Features.MemberInfo.Requests.IndirectLoginAccount
{
    using AlphaPlus.Core.Helpers;
    using AlphaPlus.Core.Models;
    using AlphaPlus.Settings.MemberInfo.Domain.Models;
    using AlphaPlus.Settings.Site.Features.MemberInfo.Models;
    using MediatR;

    public class RedirectQuery : MemberInfoBaseQuery, IRequest<string>
    {
        public RedirectQuery() : this(null){}

        public RedirectQuery(IUser viewer) : base(viewer)
        {
            viewer = viewer ?? new User(UserSession.Current);
            DarkModeName = viewer.DarkModeName;
            AssetAppPath = viewer.AssetAppPath;
        }

        public string BrowserSize { get; set; }

        public string IBCPKey => "~/Features/MemberInfo/IbcKeys/ibcpkey.snk";

        public HttpRequest Request
            => AlphaContext.Current.Request;

        public string DarkModeName { get; }

        public string AssetAppPath { get; }
    }
}