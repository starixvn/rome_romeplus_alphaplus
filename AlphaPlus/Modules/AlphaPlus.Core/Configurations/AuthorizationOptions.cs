namespace AlphaPlus.Core.Configurations;

public class AuthorizationOptions
{
    public const string Authorization = "Authorization";

    public int CachedExpireTime { get; set; }

    public GroupPageAccess[] GroupPageAccesses { get; set; }

    public int[] IgnoreReCheckInCustIds { get; set; } = [];

    public bool EnableCheckBelonging { get; set; } = true;

    public bool DisabledCheckDuplicationLogin { get; set; }

    public string KickOutUrl { get; set; }

    public string[] CustIdQueryStringParamNames { get; set; } = ["custid", "CustId", "custId", "targetCustId", "id", "cid", "recommend", "mrecommend", "idagent", "masterId", "MasterId", "AgentId", "agentId", "idAgent", "idMaster", "ibccustid", "ACustID"];

    public string[] CustIdsQueryStringParamNames { get; set; } = ["SearchAccountIds", "custids", "CustIds", "custIds", "targetCustIds", "ids", "cids", "recommends", "mrecommends", "idagents", "masterIds", "agentIds", "idAgents", "idMasters", "ibccustids", "ACustIDs"];

    public string[] CustIdJsonParamNames { get; set; }
}
