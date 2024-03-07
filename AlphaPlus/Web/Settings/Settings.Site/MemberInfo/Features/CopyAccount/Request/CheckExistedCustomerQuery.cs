using AlphaPlus.Settings.Site.Requests;
using AlphaPlus.Settings.MemberInfo.Domain.Models;
using MediatR;

namespace AlphaPlus.Settings.Site.Features.CopyAccount.Request
{
    public class CheckExistedCustomerQuery : MemberInfoBaseQuery, IRequest<bool>
    {
        public CheckExistedCustomerQuery() : this(null){}
        public CheckExistedCustomerQuery(IUser viewer) 
            : base(viewer)
        {
        }
    }
}