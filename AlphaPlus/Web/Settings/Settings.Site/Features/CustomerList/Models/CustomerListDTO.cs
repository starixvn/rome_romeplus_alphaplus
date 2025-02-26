namespace AlphaPlus.Settings.Site.Features.CustomerList.Models
{
    public class CustomerListDTO
    {
        public CustomerListDTO(
            IEnumerable<CustomerListModelBase> downlinesList,
            int totalDownlines,
            int totalPages)
        {
            DownlinesList = downlinesList;
            TotalDownlines = totalDownlines;
            TotalPages = totalPages;
        }

        public IEnumerable<CustomerListModelBase> DownlinesList { get; }

        public int TotalDownlines { get; }

        public int TotalPages { get; }
    }
}