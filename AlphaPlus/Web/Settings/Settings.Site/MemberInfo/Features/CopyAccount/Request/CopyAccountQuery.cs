using Alpha.MemberInfo.Site.Requests;
using MediatR;
using Alpha.MemberInfo.Site.Features.CopyAccount.ViewModels;
using AlphaPlus.Settings.MemberInfo.Domain.Models;

namespace Alpha.MemberInfo.Site.Features.CopyAccount.Request
{
    public class CopyAccountQuery : MemberInfoBaseQuery, IRequest<CopyAccountViewModel>
    {
        public CopyAccountQuery(IUser viewer)
            : base(viewer)
        {
        }
    }
}