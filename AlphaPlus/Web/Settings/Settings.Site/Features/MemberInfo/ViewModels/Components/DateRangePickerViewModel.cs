namespace AlphaPlus.Settings.Site.Features.MemberInfo.ViewModels.Components
{
    public class DateRangePickerViewModel
    {
        public DateRangePickerViewModel(
            DateTime minDate,
            DateTime maxDate,
            DateTime fromDate,
            DateTime toDate,
            bool isRenderSubmitButton = true)
        {
            MinDate = minDate;
            MaxDate = maxDate;
            FromDate = fromDate;
            ToDate = toDate;
        }

        public DateTime MinDate { get; }

        public DateTime MaxDate { get; }

        public DateTime FromDate { get; }

        public DateTime ToDate { get; }
    }
}