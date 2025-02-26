using AlphaPlus.Core.Models;

namespace AlphaPlus.Settings.Site.Features.ProductStatus.Params
{
    public class ProductStatusCommandParam(
        bool isGroup,
        int productId,
        int custId,
        UserLevel custRole,
        int uplineId,
        UserLevel uplineLevel,
        int actorId,
        string actorName,
        int subAccId,
        string langKey,
        string ip,
        IEnumerable<BetTypeStatusParam> productStatusParams,
        string language,
        int[] offeredProductGroupIds)
    {
        public bool IsGroup { get; } = isGroup;

        public int ProductId { get; } = productId;

        public int CustId { get; } = custId;

        public UserLevel CustRole { get; } = custRole;

        public int UplineId { get; } = uplineId;

        public UserLevel UplineLevel { get; } = uplineLevel;

        public int ActorId { get; } = actorId;

        public string ActorName { get; } = actorName;

        public int SubAccId { get; } = subAccId;

        public string LangKey { get; } = langKey;

        public string IP { get; } = ip;

        public IEnumerable<BetTypeStatusParam> ProductStatusParams { get; } = productStatusParams;

        public string Language { get; } = language;

        public int[] OfferedProductGroupIds { get; } = offeredProductGroupIds;
    }
}