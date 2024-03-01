namespace AlphaPlus.Settings.Site.Features.CustomerList.Requests
{
    using AlphaPlus.Settings.Site.Features.CustomerList.ViewModels;
    using AlphaPlus.Settings.MemberInfo.Domain.Models;
    using MediatR;

    public class CustomerListRequest : FetchDownlinesListRequesBase, IRequest<CustomerListViewModel>
    {
        public CustomerListRequest(IUser viewer) : base(viewer)
        {
            Viewer = viewer;
        }

        public IUser Viewer { get; }
    }
}