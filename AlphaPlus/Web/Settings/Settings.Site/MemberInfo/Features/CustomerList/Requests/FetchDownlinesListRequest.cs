namespace AlphaPlus.Settings.Site.Features.CustomerList.Requests
{
    using AlphaPlus.Settings.Site.Features.CustomerList.ViewModels;
    using AlphaPlus.Settings.MemberInfo.Domain.Models;
    using MediatR;

    public class FetchDownlinesListRequest : FetchDownlinesListRequestBase, IRequest<CustomerListPagingViewModel>
    {
        public FetchDownlinesListRequest() : this(null){}
        public FetchDownlinesListRequest(IUser viewer) : base(viewer)
        {
        }
    }
}