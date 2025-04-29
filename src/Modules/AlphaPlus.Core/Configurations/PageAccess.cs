namespace AlphaPlus.Core.Configurations;

public class PageAccess
{
    public string Permission { get; set; } = string.Empty;

    public string Url { get; set; } = string.Empty;

    public bool IsCheckBelonging { get; set; }

    public bool IsAllowToDoByThemselves { get; set; }

    public bool IsViewedByGroupHeadline { get; set; }

    public bool IsAllowedCrossLevel { get; set; } = true;

    public bool IsSupportedAutoFollowBetTool { get; set; }
}
