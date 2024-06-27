
namespace AlphaPlus.Settings.Site.Features.MemberInfo.Requests.ProblemAccount
{
    using System.Collections.Generic;
    using AlphaPlus.Settings.MemberInfo.Domain.Enumerations;
    using AlphaPlus.Settings.MemberInfo.Domain.Models;
    using AlphaPlus.Settings.MemberInfo.Domain.Models.ProblemAccount;
    using MediatR;

    public class GetProblemAccountRequest : ProblemAccountRequestBase, IRequest<IEnumerable<ProblemAccountModel>>
    {
        public GetProblemAccountRequest() : this(null){}
        
        public GetProblemAccountRequest(IUser viewer) : base(viewer)
        {
        }

        public UserLevel SearchRole => Enumeration.FromValue<UserLevel>(SearchRoleId);
    }
}