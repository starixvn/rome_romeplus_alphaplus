using AlphaPlus.Core.Models;

namespace AlphaPlus.Settings.Site.Features.ProductStatus.Params
{
    public class ProductStatusAuthorizationParam(
        bool isGroup,
        int productId,
        int custId,
        UserLevel custRole,
        int uplineId,
        UserLevel uplineLevel,
        int[] offeredProductGroupIds)
    {
        public bool IsGroup { get; } = isGroup;

        public int ProductId { get; } = productId;

        public int CustId { get; } = custId;

        public UserLevel CustRole { get; } = custRole;

        public int UplineId { get; } = uplineId;

        public UserLevel UplineLevel { get; } = uplineLevel;

        public int[] OfferedProductGroupIds { get; } = offeredProductGroupIds;
    }
}