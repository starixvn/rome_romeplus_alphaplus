namespace AlphaPlus.Settings.MemberInfo.API.Setting.ViewModels
{
    using System.Collections.Generic;
    using AlphaPlus.Settings.MemberInfo.API.Requests;
    using Base;

    public class SportsbookMinPTViewModel : SettingItemViewModel
    {
        public SportsbookMinPTViewModel(ISearchAccountInfoRequest request, int displayOrder = 5)
            : base(request, displayOrder)
        {
        }

        protected override string BaseAddress
            => $"{OldMainUrl}_MemberInfo/PositionTaking/{Actor.CustomerLevel.DisplayName}/EditMinPT{SearchAccount.CustomerLevel.DisplayName}.aspx";

        public override Dictionary<string, string> QueryStringParamaters
            => new Dictionary<string, string>
            {
                ["custId"] = SearchAccount.Id.ToString(),
                ["username"] = SearchAccount.Username
            };

        public override bool Visible => base.Visible && SearchAccount.IsNotMember;
    }
}