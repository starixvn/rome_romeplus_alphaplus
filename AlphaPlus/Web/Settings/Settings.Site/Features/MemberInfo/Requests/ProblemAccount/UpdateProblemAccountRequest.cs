namespace AlphaPlus.Settings.Site.Features.MemberInfo.Requests.ProblemAccount
{
    using AlphaPlus.Settings.Site.Features.MemberInfo.ViewModels.ProblemAccount;
    using AlphaPlus.Settings.MemberInfo.Domain.Models;
    using MediatR;

    public class UpdateProblemAccountRequest : MemberInfoBaseQuery, IRequest<UpdateReviewStatusViewModel>
    {
        public UpdateProblemAccountRequest() : this(null){}
        
        public UpdateProblemAccountRequest(IUser viewer):base(viewer)
        {            
        }

        public string UserNames { get; set; }

        public string Ids { get; set; }

        public byte RoleId { get; set; }
    }
}