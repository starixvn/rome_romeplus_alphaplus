namespace AlphaPlus.Settings.Site.Requests.IndirectLoginAccount
{
    using AlphaPlus.Core.Helpers;
    using AlphaPlus.Core.Models;
    using AlphaPlus.Settings.MemberInfo.Domain.Models;
    using AlphaPlus.Settings.Site.Models;
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

        public string IBCPKey => "~/IbcKeys/ibcpkey.snk";

        public HttpRequest Request
            => AlphaContext.Current.Request;

        public string DarkModeName { get; }

        public string AssetAppPath { get; }
    }
}