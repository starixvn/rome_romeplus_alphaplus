namespace AlphaPlus.Settings.Site.Transfer.Requests.MultipleTransfer
{
    using AlphaPlus.Settings.Transfer.API.Constants;
    using MediatR;
    using Settings.Transfer.API.Models;
    using ViewModels.MultipleTransfer;

    public class CustomerListQuery : TransferBaseQuery, IRequest<CustomerListViewModel>
    {
        public CustomerListQuery(IUser viewer) : base(viewer)
        {
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