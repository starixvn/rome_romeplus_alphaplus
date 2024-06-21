namespace AlphaPlus.Settings.Site.Features.ParlayMaxPayout.ViewModels
{
    using AlphaPlus.Shared.AppResources.Settings.MemberInfo;
    using AlphaPlus.Settings.MemberInfo.Domain.Features.ParlayMaxPayout.Models;
    using Requests;
    using System.Collections.Generic;

    public class ParlayMaxPayoutMultiplePageViewModel : ParlayMaxPayoutBasePageViewModel
    {
        private readonly ParlayMaxPayoutMultipleSelectQuery message;

        public ParlayMaxPayoutMultiplePageViewModel(
            ParlayMaxPayoutMultipleSelectQuery message,
            IEnumerable<ParlayMaxPayoutModel> models) : base(models)
        {
            this.message = message;
        }

        public override bool IsMultiple => true;

        public override string UpdateUrl => "UpdateMultiple";

        public override string CustIdList => message.CustIds;

        public override string CustNameList => message.CustNames;

        public override string SubTitle
            => string.Join(", ", message.CustNames.Split(','));

        public override string SubText => Resources.ParlayMaxPayoutMultipleSubText;
    }
}