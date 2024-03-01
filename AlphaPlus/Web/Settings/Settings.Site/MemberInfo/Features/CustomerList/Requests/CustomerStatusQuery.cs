namespace Alpha.MemberInfo.Site.Features.CustomerList.Requests
{
    using Alpha.MemberInfo.Site.Features.CustomerList.ViewModels;
    using AlphaPlus.Settings.MemberInfo.Domain.Models;
    using MediatR;

    public class CustomerStatusQuery : CustomerStatusQueryBase, IRequest<CustomerStatusViewModel>
    {
        public CustomerStatusQuery(IUser viewer) : base(viewer)
        {
        }
    }
}