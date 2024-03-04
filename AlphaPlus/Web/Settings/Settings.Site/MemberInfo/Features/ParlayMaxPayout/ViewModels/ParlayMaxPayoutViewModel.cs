namespace AlphaPlus.Settings.Site.Features.ParlayMaxPayout.ViewModels
{
    using AlphaPlus.Settings.MemberInfo.Domain.AppResources;
    using AlphaPlus.Settings.MemberInfo.Domain.Constants;
    using AlphaPlus.Settings.MemberInfo.Domain.Features.ParlayMaxPayout.Models;
    using System.Collections.Generic;
    using AlphaPlus.Settings.MemberInfo.Domain.Extensions;

    public class ParlayMaxPayoutViewModel
    {
        private readonly Dictionary<byte, string> sportNameDictionary
            = new Dictionary<byte, string>
            {
                [SportTypeConstants.SoccerParlay] = Resources.soccerParlay,
                [SportTypeConstants.MixedSportsParlay] = Resources.mixsports
            };

        public ParlayMaxPayoutViewModel(ParlayMaxPayoutModel model)
        {
            BetTypeId = model.BetTypeId;
            SportTypeId = model.SportTypeId;

            BetTypeName = sportNameDictionary[SportTypeId];
            SportTypeName = sportNameDictionary[SportTypeId];

            Combo2 = ToMoneyFormat(model.Combo2);
            UplineCombo2 = ToMoneyFormat(model.UplineCombo2);

            Combo35 = ToMoneyFormat(model.Combo35);
            UplineCombo35 = ToMoneyFormat(model.UplineCombo35);

            ComboOver6 = ToMoneyFormat(model.ComboOver6);
            UplineComboOver6 = ToMoneyFormat(model.UplineComboOver6);

            SingleMatchParlay = ToMoneyFormat(model.SingleMatchParlay);
            UplineSingleMatchParlay = ToMoneyFormat(model.UplineSingleMatchParlay);

            IsAllowUnlimitCombo2 = model.IsAllowUnlimitCombo2;
            IsAllowUnlimitCombo35 = model.IsAllowUnlimitCombo35;
            IsAllowUnlimitComboOver6 = model.IsAllowUnlimitComboOver6;
            IsAllowUnlimitSingleMatchParlay = model.IsAllowUnlimitSingleMatchParlay;
        }

        public byte BetTypeId { get; }

        public string BetTypeName { get; }

        public byte SportTypeId { get; }

        public string SportTypeName { get; }

        public string SingleMatchParlay { get; }

        public string Combo2 { get; }

        public string Combo35 { get; }

        public string ComboOver6 { get; }

        public string UplineSingleMatchParlay { get; }

        public string UplineCombo2 { get; }

        public string UplineCombo35 { get; }

        public string UplineComboOver6 { get; }

        public bool IsAllowUnlimitSingleMatchParlay { get; }

        public bool IsAllowUnlimitCombo2 { get; }

        public bool IsAllowUnlimitCombo35 { get; }

        public bool IsAllowUnlimitComboOver6 { get; }

        private static string ToMoneyFormat(decimal value)
            => value > 0
            ? value.ToMoneyFormat()
            : "Unlimited";
    }
}