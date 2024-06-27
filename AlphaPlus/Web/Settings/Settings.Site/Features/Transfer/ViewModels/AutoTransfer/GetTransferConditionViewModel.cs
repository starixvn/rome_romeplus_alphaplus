using AlphaPlus.Shared.AppResources.Settings.Transfer;
using AlphaPlus.Settings.Transfer.API.Models;

namespace AlphaPlus.Settings.Site.Features.Transfer.ViewModels.AutoTransfer
{
    public class GetTransferConditionViewModel
    {
        public string SearchAccountIds { get; set; }

        public string Usernames { get; set; }

        public string TransferCondition { get; set; }

        public byte ViewerLevel { get; set; }

        public string AutoTransfer_InfoAffectTime
        {
            get
            {
                if (ViewerLevel == UserLevel.Super.Value)
                {
                    return Resources.AutoTransfer_InfoAffectTimeSuper;
                }

                return string.Format(Resources.AutoTransfer_InfoAffectTime, TransferCondition);
            }
        }
    }
}