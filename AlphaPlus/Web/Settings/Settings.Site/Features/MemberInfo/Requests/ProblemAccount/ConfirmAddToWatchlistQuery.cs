namespace AlphaPlus.Settings.Site.Features.MemberInfo.Requests.ProblemAccount
{
    using AlphaPlus.Settings.Site.Features.MemberInfo.Requests;
    using AlphaPlus.Settings.Site.Features.MemberInfo.ViewModels.ProblemAccount;
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