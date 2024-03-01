namespace Alpha.MemberInfo.Site.Requests.IndirectLoginAccount
{
    using AlphaPlus.Core.Helpers;
    using AlphaPlus.Settings.MemberInfo.Domain.Models;
    using MediatR;

    public class NewAccountCommand : MemberInfoBaseQuery, IRequest<Result>
    {
        public NewAccountCommand(IUser viewer) : base(viewer)
        {
        }

        public string Password { get; set; }

        public string BrowserSize { get; set; }

        public HttpRequest Request => AlphaContext.Current.Request;

        public override bool IsValid()
            => base.IsValid() && !string.IsNullOrWhiteSpace(Password) && !IsInternal && IsAllowedIndirectLogin;
    }
}