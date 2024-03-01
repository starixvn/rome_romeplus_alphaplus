namespace Alpha.MemberInfo.Site.Requests.ProblemAccount
{
    using Alpha.MemberInfo.Site.Requests;
    using Alpha.MemberInfo.Site.ViewModels.ProblemAccount;
    using AlphaPlus.Settings.MemberInfo.Domain.Models;
    using MediatR;

    public class ConfirmAddToWatchlistQuery : MemberInfoBaseQuery, IRequest<ConfirmAddToWatchlistViewModel>
    {
        public ConfirmAddToWatchlistQuery(IUser viewer) : base(viewer)
        {
        }

        public string CustIds { get; set; }

        public override bool IsValid() => base.IsValid() && !string.IsNullOrWhiteSpace(CustIds);
    }
}