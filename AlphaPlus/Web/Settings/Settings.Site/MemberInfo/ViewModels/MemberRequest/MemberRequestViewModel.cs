namespace AlphaPlus.Settings.Site.ViewModels.MemberRequest
{
    using System.Collections.Generic;
    using AlphaPlus.Settings.MemberInfo.Domain.Models.MemberRequests;

    public class MemberRequestViewModel
    {
        public MemberRequestViewModel(
            IEnumerable<MemberRequestModel> memberRequestData,
            int selectedProductId,
            bool disabledUserSelectedProduct,
            int actorId,
            bool canUpdateAccount)
        {
            MemberRequestData = memberRequestData;
            SelectedProductId = selectedProductId;
            DisabledUserSelectedProduct = disabledUserSelectedProduct;
            ActorId = actorId;
            CanUpdateAccount = canUpdateAccount;
        }

        public int ActorId { get; }

        public IEnumerable<MemberRequestModel> MemberRequestData { get; }

        public int SelectedProductId { get; }

        public bool CanUpdateAccount { get; }

        public bool DisabledUserSelectedProduct { get; }

        public bool CanEdit => !DisabledUserSelectedProduct && CanUpdateAccount;
    }
}