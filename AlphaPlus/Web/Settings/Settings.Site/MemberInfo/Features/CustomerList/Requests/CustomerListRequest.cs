namespace AlphaPlus.Settings.Site.Features.CustomerList.Requests
{
    using AlphaPlus.Settings.Site.Features.CustomerList.ViewModels;
    using AlphaPlus.Settings.MemberInfo.Domain.Models;
    using MediatR;
    using AlphaPlus.Settings.Site.Models;
    using AlphaPlus.Core.Models;

    public class CustomerListRequest : FetchDownlinesListRequesBase, IRequest<CustomerListViewModel>
    {
        public CustomerListRequest() : this(null)
        {

        }

        public CustomerListRequest(IUser viewer) : base(viewer)
        {
            Viewer = viewer ?? new User(UserSession.Current);
        }

        public IUser Viewer { get; }
    }
}