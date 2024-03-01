namespace AlphaPlus.Settings.Site.Requests.IndirectLoginAccount
{
    using AlphaPlus.Core.Helpers;
    using AlphaPlus.Settings.MemberInfo.Domain.Models;
    using MediatR;

    public class RedirectQuery : MemberInfoBaseQuery, IRequest<string>
    {
        public RedirectQuery(IUser viewer) : base(viewer)
        {
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