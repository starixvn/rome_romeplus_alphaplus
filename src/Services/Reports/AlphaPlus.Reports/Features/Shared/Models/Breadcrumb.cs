namespace AlphaPlus.Reports.Features.Shared.Models;

public record Breadcrumb(int CustId, string CustName, byte CustLevelId, bool IsConsolidation = false);
