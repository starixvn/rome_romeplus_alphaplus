namespace AlphaPlus.Identity.API.Features.Authorization;

public record DownlineInfo(bool IsActive, int CustId, bool IsSubAcc, int RoleId, int AgentId);
