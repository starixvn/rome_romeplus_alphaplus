namespace AlphaPlus.Settings.Site.Features.Transfer.Requests.AutoTransfer
{
    using MediatR;
    using AlphaPlus.Settings.Site.Features.Transfer.Models;
    using AlphaPlus.Core.Models;
    using AlphaPlus.Settings.Transfer.API.Models;
    using Result = Settings.Transfer.API.Models.Result;

    public class CancelAllJobsQuery : IRequest<Result>
    {
        public CancelAllJobsQuery() : this(null) {}

        public CancelAllJobsQuery(IUser user)
        {
            user = user ?? new User(UserSession.Current);
            UplineId = user.Id;
            SubAccId = user.SubAccId;
            IP = user.IP;
            LanguageKey = user.LanguageKey;
            AGIP = user.AGIP;
        }

        public int UplineId { get; }

        public int SubAccId { get; }

        public string IP { get; }

        public string LanguageKey { get; }

        public string AGIP { get; }

        
        public string Username { get; set; }

        public string Status { get; set; }

        public bool IsValid() => UplineId > 0;
    }
}