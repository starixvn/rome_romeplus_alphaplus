using AlphaPlus.Core.Models;
using AlphaPlus.Settings.MemberInfo.Domain.Constants;
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
            bool isGroup,
            int agentId)
        {
            ProductId = productId;
            CustId = custId;
            CustName = custName;
            CustRole = custRole;
            ProductStatus = productStatus;
            var isDirectMember = (loggedInUserRole.IsSuperLevel() && agentId == DirectMemberAgentIds.Super)
                    || (loggedInUserRole.IsMasterLevel() && agentId == DirectMemberAgentIds.Master);
            IsDirectUplineUpdateMember = custRole.IsMemberLevel() && (loggedInUserRole.IsAgentLevel() || isDirectMember);
            IsDirectDownline = custRole.IsDirectDownlineOf(loggedInUserRole) || isDirectMember;
            IsViewOnly = isViewOnly;
            IsGroup = isGroup;
        }

        public int ProductId { get; }

        public int CustId { get; }

        public string CustName { get; }

        public UserLevel CustRole { get; }

        public IEnumerable<ProductStatusModel> ProductStatus { get; }

        public bool IsDirectUplineUpdateMember { get; }

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
                || (!IsDirectUplineUpdateMember && !productStatus.IsDisabled);
        }

        public bool IsEnabledSubmitButton(ProductStatusModel productStatus)
        {
            if (ProductId == ProductIdConstants.SabaVirtualSports
                || ProductId == ProductIdConstants.CashOut)
            {
                return IsDirectDownline;
            }

            return IsDirectUplineUpdateMember
            ? !productStatus.IsDisabledUpline
            : !productStatus.IsDisabledUpline && productStatus.IsDisabled;
        }

        public bool IsShowMessageWarnning
            => !IsDirectUplineUpdateMember
            && ProductId != ProductIdConstants.SabaVirtualSports
            && ProductId != ProductIdConstants.CashOut;

        public string UserLevelDisplayName
        {
            get
            {
                if (CustRole.IsMemberLevel())
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