using Microsoft.AspNetCore.Mvc.Rendering;
using AlphaPlus.Shared.AppResources.Settings.Transfer;
using AlphaPlus.Settings.Site.Features.Transfer.ViewModels.Components;
using AlphaPlus.Core.Helpers;
using AlphaPlus.Settings.Transfer.API.Configurations;
using AlphaPlus.Core.Models;
using AlphaPlus.Core.Utils;

namespace AlphaPlus.Settings.Site.Features.Transfer.ViewModels.AutoTransfer
{
    public class HistoryViewModel
    {
        private const int reportMonth = -3;

        public HistoryViewModel(IUserSession userSession)
        {
            var minDate = SystemTime.Yesterday.AddMonths(reportMonth);
            MinDate = minDate;
            DateRangePickerViewModel = new DateRangePickerViewModel(MinDate, MaxDate, FromDate, ToDate, 1);
            DateRangePickerViewModel.RangeOptions = DateRangePickerViewModel.RangeOptions.Where(x => x.Value != "0");

            ViewerId = userSession.CustId;
            SubAccId = userSession.SubAccId;
        }

        public DateTime FromDate => SystemTime.Yesterday;

        public DateTime ToDate => SystemTime.Yesterday;

        public static DateTime MinDate { get; set; }

        public static DateTime MaxDate => SystemTime.Yesterday;

        public DateRangePickerViewModel DateRangePickerViewModel { get; }

        public int ViewerId { get; set; }

        public int SubAccId { get; set; }

        public IEnumerable<SelectListItem> Columns
            => [
                new SelectListItem(Resources.First_LastName, "3" ),
                new SelectListItem(Resources.TransferAmount, "4" ),
                new SelectListItem(Resources.Status, "5" ),
                new SelectListItem(Resources.Reason, "6" ),
                new SelectListItem(Resources.CompletedTime, "7" )
            ];

        public string UserGuideLink
        {
            get
            {
                if (AlphaPlus.Core.Configurations.AppSettings.SupportedNewHandbook)
                {
                    return MicroSites.FeatureUrl($"~/HandbookDefault/GetBySlug?slug=auto-transfer");
                }

                return AlphaPlus.Core.Configurations.AppSettings.IsSiteLabel789y
                    ? MicroSites.FeatureUrl("~/StaticMessage/Guide?id=184099")
                    : MicroSites.FeatureUrl("~/Index.aspx?linktomessagesite=184099");
            }
        }
    }
}