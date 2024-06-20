using AlphaPlus.Core.Models;
using AlphaPlus.Settings.Site.Features.ProductStatus.Models;
using AlphaPlus.Shared.AppResources.Settings;

namespace AlphaPlus.Settings.Site.Features.ProductStatus.ViewModels
{
    public class ProductStatusViewModel
    {
        public ProductStatusViewModel(
            int productId,
            int custId,
            string custName,
            UserLevel custRole,
            IEnumerable<ProductStatusModel> productStatus,
            UserLevel loggedInUserRole,
            bool isViewOnly,
            bool isGroup)
        {
            ProductId = productId;
            CustId = custId;
            CustName = custName;
            CustRole = custRole;
            ProductStatus = productStatus;
            IsAgentUpdateMember = custRole.IsMemberLevel() && loggedInUserRole.IsAgentLevel();
            IsDirectDownline = custRole.IsDirectDownlineOf(loggedInUserRole);
            IsViewOnly = isViewOnly;
            IsGroup = isGroup;
        }

        public int ProductId { get; }

        public int CustId { get; }

        public string CustName { get; }

        public UserLevel CustRole { get; }

        public IEnumerable<ProductStatusModel> ProductStatus { get; }

        public bool IsAgentUpdateMember { get; }

        public bool IsDirectDownline { get; }

        public bool IsViewOnly { get; }

        public bool IsGroup { get; }

        public bool IsDisabledEditCustStatus(ProductStatusModel productStatus)
        {
            if (ProductId == ProductIdConstants.SabaVirtualSports
                || ProductId == ProductIdConstants.CashOut)
            {
                return !IsDirectDownline;
            }

            return !IsDirectDownline
                || productStatus.IsDisabledUpline
                || (!IsAgentUpdateMember && !productStatus.IsDisabled);
        }

        public bool IsEnabledSubmitButton(ProductStatusModel productStatus)
        {
            if (ProductId == ProductIdConstants.SabaVirtualSports
                || ProductId == ProductIdConstants.CashOut)
            {
                return IsDirectDownline;
            }

            return IsAgentUpdateMember
            ? !productStatus.IsDisabledUpline
            : !productStatus.IsDisabledUpline && productStatus.IsDisabled;
        }

        public bool IsShowMessageWarnning
            => !IsAgentUpdateMember
            && ProductId != ProductIdConstants.SabaVirtualSports
            && ProductId != ProductIdConstants.CashOut;

        public string UserLevelDisplayName
        {
            get
            {
                if (CustRole.IsSuperLevel())
                {
                    return Resources.Member;
                }

                if (CustRole.IsAgentLevel())
                {
                    return Resources.Agent;
                }

                if (CustRole.IsMasterLevel())
                {
                    return Resources.Master;
                }

                if (CustRole.IsSuperLevel())
                {
                    return Resources.Super;
                }

                return string.Empty;
            }
        }
    }
}