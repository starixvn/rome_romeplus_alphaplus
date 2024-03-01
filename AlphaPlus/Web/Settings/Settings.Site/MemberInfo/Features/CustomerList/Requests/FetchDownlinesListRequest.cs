namespace Alpha.MemberInfo.Site.Features.CustomerList.Requests
{
    using Alpha.MemberInfo.Site.Features.CustomerList.ViewModels;
    using AlphaPlus.Settings.MemberInfo.Domain.Models;
    using MediatR;

    public class FetchDownlinesListRequest : FetchDownlinesListRequesBase, IRequest<CustomerListPagingViewModel>
    {
        public FetchDownlinesListRequest(IUser viewer) : base(viewer)
        {
        }
    }
}