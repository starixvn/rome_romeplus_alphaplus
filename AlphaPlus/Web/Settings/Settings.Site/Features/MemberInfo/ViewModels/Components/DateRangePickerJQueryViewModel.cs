namespace AlphaPlus.Settings.Site.Features.MemberInfo.ViewModels.Components
{
    public class DateRangePickerJQueryViewModel
    {
        public DateRangePickerJQueryViewModel(
            string dateRangeText,
            string language,
            DateTime fromDate,
            DateTime toDate,
            DateTime? minDate = null,
            DateTime? maxDate = null)
        {
            DateRangeText = dateRangeText;
            Language = language;
            FromDate = fromDate;
            ToDate = toDate;
            MinDate = minDate;
            MaxDate = maxDate;
        }

        public string DateRangeText { get; }

        public string Language { get; }

        public DateTime FromDate { get; }

        public DateTime ToDate { get; }

        public DateTime? MinDate { get; }

        public DateTime? MaxDate { get; }
    }
}