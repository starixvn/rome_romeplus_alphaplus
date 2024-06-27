using AlphaPlus.Settings.Site.Features.MemberInfo.Requests;
using MediatR;
using AlphaPlus.Settings.Site.Features.MemberInfo.Features.CopyAccount.ViewModels;
using AlphaPlus.Settings.MemberInfo.Domain.Models;

namespace AlphaPlus.Settings.Site.Features.MemberInfo.Features.CopyAccount.Request
{
    public class CopyAccountQuery : MemberInfoBaseQuery, IRequest<CopyAccountViewModel>
    {
        public CopyAccountQuery() : this(null){}
        
        public CopyAccountQuery(IUser viewer)
            : base(viewer)
        {
        }
    }
}