namespace AlphaPlus.Settings.Site.Features.Transfer.Requests.MultipleTransfer
{
    using AlphaPlus.Core.Models;
    using AlphaPlus.Settings.Site.Features.Transfer.Models;
    using AlphaPlus.Settings.Transfer.API.Constants;
    using MediatR;
    using Settings.Transfer.API.Models;
    using ViewModels.MultipleTransfer;

    public class CustomerListQuery : TransferBaseQuery, IRequest<CustomerListViewModel>
    {
        public CustomerListQuery() : this(null){}

        public CustomerListQuery(IUser viewer) : base(viewer)
        {
            viewer = viewer ?? new User(UserSession.Current);
            AllowTotalBalanceTransfer = viewer.AllowTotalBalanceTransfer;
        }

        public string Username { get; set; }

        public int Status { get; set; } = StatusConstant.Open;

        public bool AllYesterdayBalance { get; set; }

        public bool AllowTotalBalanceTransfer { get; }

        public int PageSize { get; set; } = PagerConstant.DefaultPageSize;

        public int PageIndex { get; set; } = PagerConstant.DefaultPageIndex;
    }
}