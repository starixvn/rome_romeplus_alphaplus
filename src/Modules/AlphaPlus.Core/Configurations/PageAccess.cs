namespace AlphaPlus.Core.Configurations;

public class PageAccess
{
    public string Permission { get; set; } = string.Empty;

    public string Url { get; set; } = string.Empty;

    public bool IsCheckBelonging { get; set; } = false;

    public bool IsAllowToDoByThemselves { get; set; } = false;

    public bool IsViewedByGroupHeadline { get; set; } = false;

    public bool IsAllowedCrossLevel { get; set; } = true;

    public bool IsSupportedAutoFollowBetTool { get; set; } = false;
}
