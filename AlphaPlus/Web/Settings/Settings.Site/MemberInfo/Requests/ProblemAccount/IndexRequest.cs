namespace Alpha.MemberInfo.Site.Requests.ProblemAccount
{
    using Alpha.MemberInfo.Site.ViewModels.ProblemAccount;
    using AlphaPlus.Settings.MemberInfo.Domain.Models;
    using MediatR;

    public class IndexRequest : ProblemAccountRequestBase, IRequest<ProblemAccountReportViewModel>
    {
        public IndexRequest(IUser viewer) : base(viewer)
        {
        }
    }
}