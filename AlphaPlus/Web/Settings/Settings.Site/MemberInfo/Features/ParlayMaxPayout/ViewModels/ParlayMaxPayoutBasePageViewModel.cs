using AlphaPlus.Settings.MemberInfo.Domain.AppResources;
using AlphaPlus.Settings.MemberInfo.Domain.Constants;
using AlphaPlus.Settings.MemberInfo.Domain.Features.ParlayMaxPayout.Models;
using AlphaPlus.Shared.AppResources.Settings.ProductResources;

namespace AlphaPlus.Settings.Site.Features.ParlayMaxPayout.ViewModels
{
    public class ParlayMaxPayoutBasePageViewModel
    {
        private readonly Dictionary<byte, string> classNameDictionary
            = new Dictionary<byte, string>
            {
                [SportTypeConstants.SoccerParlay] = "soccer-parlay",
                [SportTypeConstants.MixedSportsParlay] = "mixed-sports-parlay"
            };

        private const string parlayMaxPayoutSettingPartialViewPath
            = "~/MemberInfo/Features/ParlayMaxPayout/Views/ParlayMaxPayoutSetting.cshtml";

        private readonly IEnumerable<ParlayMaxPayoutModel> data;

        public ParlayMaxPayoutBasePageViewModel(
            IEnumerable<ParlayMaxPayoutModel> data)
        {
            this.data = data;

            Combo2FieldText = Resources.Combo2;
            Combo35FieldText = Resources.Combo35;
            ComboOver6FieldText = Resources.ComboOver6;
            SingleMatchParlayFieldText = Resources.SingleMatchParlay;
            UpdateSuccessfullyText = Resources.ParlayMaxPayoutUpdateSuccessfully;
            InvalidMaxPayoutValueText = Resources.InvalidMaxPayoutValueText;
            GreaterThanMaxValueText = Resources.GreaterThanMaxValueText;
            HasNoChangeText = ProductResources.NoDataChange;
            NotAuthorizedText = Settings.MemberInfo.Domain.AppResources.Lang.NotAuthorized;
            ParlayMaxPayoutSettingPartialViewPath = parlayMaxPayoutSettingPartialViewPath;
        }

        public string CheckDownlineUrl { get; } = "CheckDownline";

        public virtual bool IsMultiple { get; }

        public virtual string UpdateUrl { get; }

        public virtual string CustIdList { get; }

        public virtual string CustNameList { get; }

        public virtual string SubTitle { get; }

        public virtual string SubText { get; }

        public string Combo2FieldText { get; }

        public string Combo35FieldText { get; }

        public string ComboOver6FieldText { get; }

        public string SingleMatchParlayFieldText { get; }

        public string UpdateSuccessfullyText { get; }

        public string InvalidMaxPayoutValueText { get; }

        public string GreaterThanMaxValueText { get; }

        public string HasNoChangeText { get; }

        public string NotAuthorizedText { get; }

        public IEnumerable<ParlayMaxPayoutViewModel> Data
        {
            get
            {
                foreach (var model in data)
                {
                    yield return new ParlayMaxPayoutViewModel(model);
                }
            }
        }

        public string ParlayMaxPayoutSettingPartialViewPath { get; }

        public string GetClassName(byte sportTypeId)
        {
            if (classNameDictionary.ContainsKey(sportTypeId))
            {
                return classNameDictionary[sportTypeId];
            }

            return string.Empty;
        }
    }
}