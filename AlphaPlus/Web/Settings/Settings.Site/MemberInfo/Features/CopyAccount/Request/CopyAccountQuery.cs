using AlphaPlus.Settings.Site.Requests;
using MediatR;
using AlphaPlus.Settings.Site.Features.CopyAccount.ViewModels;
using AlphaPlus.Settings.MemberInfo.Domain.Models;

namespace AlphaPlus.Settings.Site.Features.CopyAccount.Request
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