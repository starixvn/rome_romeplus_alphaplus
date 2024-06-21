namespace AlphaPlus.Settings.Site.Features.ParlayMaxPayout.ViewModels
{
    using AlphaPlus.Shared.AppResources.Settings.MemberInfo;
    using AlphaPlus.Settings.MemberInfo.Domain.Features.ParlayMaxPayout.Models;
    using Requests;
    using System.Collections.Generic;

    public class ParlayMaxPayoutSinglePageViewModel : ParlayMaxPayoutBasePageViewModel
    {
        private readonly ParlayMaxPayoutSingleSelectQuery message;

        public ParlayMaxPayoutSinglePageViewModel(
            ParlayMaxPayoutSingleSelectQuery message,
            IEnumerable<ParlayMaxPayoutModel> models) : base(models)
        {
            this.message = message;
        }

        public override string UpdateUrl => "UpdateSingle";

        public override string CustIdList => message.CustId.ToString();

        public override string CustNameList => message.CustName;

        public override string SubTitle
            => string.Format(Resources.ParlayMaxPayoutSubTitle, message.CustName);

        public override string SubText
            => string.Format(Resources.ParlayMaxPayoutSingleSubText, message.CustName);
    }
}