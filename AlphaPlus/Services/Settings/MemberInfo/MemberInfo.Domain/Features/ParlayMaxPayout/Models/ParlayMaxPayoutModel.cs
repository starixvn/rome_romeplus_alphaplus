namespace AlphaPlus.Settings.MemberInfo.Domain.Features.ParlayMaxPayout.Models
{
    public class ParlayMaxPayoutModel
    {
        public byte SportTypeId { get; set; }

        public byte BetTypeId { get; set; }

        public decimal Combo2 { get; set; }

        public decimal Combo35 { get; set; }

        public decimal ComboOver6 { get; set; }

        public decimal SingleMatchParlay { get; set; }

        public decimal UplineCombo2 { get; set; }

        public decimal UplineCombo35 { get; set; }

        public decimal UplineComboOver6 { get; set; }

        public decimal UplineSingleMatchParlay { get; set; }

        public bool IsAllowUnlimitCombo2 { get; set; }

        public bool IsAllowUnlimitCombo35 { get; set; }

        public bool IsAllowUnlimitComboOver6 { get; set; }

        public bool IsAllowUnlimitSingleMatchParlay { get; set; }
    }
}