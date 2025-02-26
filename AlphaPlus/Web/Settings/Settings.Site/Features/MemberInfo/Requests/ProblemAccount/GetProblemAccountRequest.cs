using AlphaPlus.Core.Models;
using AlphaPlus.Settings.MemberInfo.Domain.Models;
using AlphaPlus.Settings.MemberInfo.Domain.Models.ProblemAccount;
using MediatR;

namespace AlphaPlus.Settings.Site.Features.MemberInfo.Requests.ProblemAccount
{
    public class GetProblemAccountRequest : ProblemAccountRequestBase, IRequest<IEnumerable<ProblemAccountModel>>
    {
        public GetProblemAccountRequest() : this(null) { }

        public GetProblemAccountRequest(IUser viewer) : base(viewer)
        {
        }

        public UserLevel SearchRole => Enumeration.FromValue<UserLevel>(SearchRoleId);
    }
}