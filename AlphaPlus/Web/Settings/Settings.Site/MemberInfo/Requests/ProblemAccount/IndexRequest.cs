namespace AlphaPlus.Settings.Site.Requests.ProblemAccount
{
    using AlphaPlus.Settings.Site.ViewModels.ProblemAccount;
    using AlphaPlus.Settings.MemberInfo.Domain.Models;
    using MediatR;

    public class IndexRequest : ProblemAccountRequestBase, IRequest<ProblemAccountReportViewModel>
    {
        public IndexRequest() : this(null){}
        
        public IndexRequest(IUser viewer) : base(viewer)
        {
        }
    }
}