namespace AlphaPlus.Core.Configurations;

public class AuthorizationOptions
{
    public int CachedExpireTime { get; set; }

    public GroupPageAccess[] GroupPageAccesses { get; set; }

    public int[] IgnoreReCheckInCustIds { get; set; }

    public bool EnableCheckBelonging { get; set; } = true;

    public bool DisabledCheckDuplicationLogin { get; set; }

    public string KickOutUrl { get; set; }

    public string[] CustIdQueryStringParamNames { get; set; }

    public string[] CustIdsQueryStringParamNames { get; set; }

    public string[] CustIdJsonParamNames { get; set; }
}
