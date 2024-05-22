using AlphaPlus.Settings.Site.Features.CustomerList.Requests;
using AlphaPlus.Settings.Site.Features.CustomerList.ViewModels.Status.Base;

namespace AlphaPlus.Settings.Site.Features.CustomerList.ViewModels.Status
{
    public class ConfirmTransferBalanceDailyViewModel : ConfirmStatusItemViewModelBase
    {
        public ConfirmTransferBalanceDailyViewModel(ISearchAccountInfoRequest request, int displayOrder = 8)
           : base(request, displayOrder)
        {
        }

        public override bool Checked => Request.GetData<bool>(DataKey.SearchAccount.IsAllDayBalTransfer);

        internal override string SuffixMessage => "Message";

        public override string ConfirmMessage
        {
            get
            {
                var confirmMessage = string.Format(Request.GetResource($"{GetType().Name}{SuffixMessage}"), SearchAccount.Username);

                if (Checked)
                {
                    return confirmMessage;
                }

                if (Request.AutoTransferAffect)
                {
                    confirmMessage += string.Format(AppResources.Lang.TurnOnTransferBalanceDailyNoteMessage, SearchAccount.Username);
                }

                return confirmMessage;
            }
        }

        internal override string StatusName => DataKey.StatusMapping.IsAllDayBalTransfer;

        internal override bool StatusValue => !Checked;

        public override bool Visible => Request.SearchAccount.IsMaster && Request.GetData<bool>(DataKey.SearchAccount.AllowedAllDayBalTransfer);

        public override string Url => "UpdateAllDayBalanceTransfer";

        public string Tooltip => Request.GetResource("ConfirmTransferBalanceDailyViewModelTooltip");
    }
}