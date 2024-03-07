namespace AlphaPlus.Settings.Site.Requests.ProblemAccount
{
    using AlphaPlus.Settings.Site.Requests;
    using AlphaPlus.Settings.Site.ViewModels.ProblemAccount;
    using AlphaPlus.Settings.MemberInfo.Domain.Models;
    using MediatR;

    public class ConfirmAddToWatchlistQuery : MemberInfoBaseQuery, IRequest<ConfirmAddToWatchlistViewModel>
    {
        public ConfirmAddToWatchlistQuery() : this(null){}
        
        public ConfirmAddToWatchlistQuery(IUser viewer) : base(viewer)
        {
        }

        public string CustIds { get; set; }

        public override bool IsValid() => base.IsValid() && !string.IsNullOrWhiteSpace(CustIds);
    }
}