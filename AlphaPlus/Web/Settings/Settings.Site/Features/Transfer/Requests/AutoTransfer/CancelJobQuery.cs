namespace AlphaPlus.Settings.Site.Features.Transfer.Requests.AutoTransfer
{
    using AlphaPlus.Core.Models;
    using AlphaPlus.Settings.Site.Features.Transfer.Models;
    using MediatR;
    using Settings.Transfer.API.Models;

    public class CancelJobQuery : IRequest<Result>
    {
        public CancelJobQuery() : this(null) { }

        public CancelJobQuery(IUser user)
        {
            user = user ?? new User(UserSession.Current);

            UplineId = user.Id;
            SubAccId = user.SubAccId;
            IP = user.IP;
            LanguageKey = user.LanguageKey;
            AGIP = user.AGIP;
        }

        public int CustId { get; set; }

        public string ViewStatus { get; set; }

        public int UplineId { get; }

        public int SubAccId { get; }

        public string IP { get; }

        public string LanguageKey { get; }

        public string AGIP { get; }

        public bool IsValid() => CustId > 0 && UplineId > 0;
    }
}