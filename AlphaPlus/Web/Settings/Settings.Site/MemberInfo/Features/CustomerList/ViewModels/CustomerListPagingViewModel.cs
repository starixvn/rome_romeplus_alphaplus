namespace AlphaPlus.Settings.Site.Features.CustomerList.ViewModels
{
    using AlphaPlus.Settings.Site.Features.CustomerList.Requests;
    using AlphaPlus.Settings.MemberInfo.Domain.Features.CustomerList.Models;
    using AlphaPlus.Settings.MemberInfo.Domain.Site.Features.CustomerList.ViewModels;
    using AlphaPlus.Utilities.IP;
    using System.Collections.Generic;
    using System.Linq;

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

                     if (!message.IsShowMyanmarOdds)
                     {
                         if (message.CustLevel.IsAgentLevel())
                         {
                             var memberList = downline as MemberListModel;
                             memberList.PlayerDiscountMyanmarOdds = 0;
                             memberList.AdiscountMyanmarOdds = 0;
                         }
                         else if (message.CustLevel.IsMasterLevel())    
                         {
                             (downline as AgentListModel).DiscountMyanmarOdds = 0;
                         }
                         else
                         {
                             (downline as MasterListModel).DiscountMyanmarOdds = 0;
                         }
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