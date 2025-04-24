namespace AlphaPlus.Settings.Site.Features.ProductStatus.Models;

public class ProductSetting
{

    public int ProductId { get; set; }

    public int? GroupId { get; set; }

    public int SportType { get; set; }

    public int BetType { get; set; }

    public bool IsHidden { get; set; }

    public bool IsDisabled { get; set; }
}