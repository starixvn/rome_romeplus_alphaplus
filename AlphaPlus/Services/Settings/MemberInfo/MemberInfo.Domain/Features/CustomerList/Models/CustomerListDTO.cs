namespace AlphaPlus.Settings.MemberInfo.Domain.Site.Features.CustomerList.ViewModels
{
    using AlphaPlus.Settings.MemberInfo.Domain.Features.CustomerList.Models;
    using System.Collections.Generic;

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