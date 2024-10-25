using AlphaPlus.Settings.Site.Features.CustomerList.Models;
using AlphaPlus.Settings.Site.Features.CustomerList.Requests;
using AlphaPlus.Utilities.IP;

namespace AlphaPlus.Settings.Site.Features.CustomerList.ViewModels
{
    public class CustomerListPagingViewModel
    {
        public CustomerListPagingViewModel(CustomerListDTO customerListDTO, FetchDownlinesListRequest message, IPService ipService)
        {
            DownlinesList = customerListDTO.DownlinesList.Select(
                 downline =>
                 {
                     downline.CreateDateFormat = message.LimitedDateToShowInfo ? string.Empty : downline.CreateDate.ToString("MM/dd/yyyy hh:mm:ss tt");

                     if (!message.ShowNickname)
                     {
                         downline.UserName2 = string.Empty;
                     }

                     var formatedIP = ipService.FormatIP(downline.LastLoginIP, message.IsAdminCanViewVNIP);

                     downline.LastLoginIP = formatedIP.IP;
                     downline.AllowOpenInfoPopup = formatedIP.AllowOpenInfoPopup;

                     return downline;
                 });

            TotalDownlines = customerListDTO.TotalDownlines;
            TotalPages = customerListDTO.TotalPages;
        }

        public IEnumerable<CustomerListModelBase> DownlinesList { get; }

        public int TotalDownlines { get; }

        public int TotalPages { get; }
    }
}