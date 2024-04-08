namespace AlphaPlus.Reports.Site.Features.Shared.Requests;

public class ProductDetailDateRangeQuery : DefaultDateRangeQuery
{
    public string UserSelectedProductIds { get; set; } = "*";

    public string UserSelectedResourceIds { get; set; } = "*";

    public bool IsStackMode { get; set; }

    public string SortPropertyName { get; set; }

    public byte DescendingSort { get; set; }

    public bool IsSetDefaultStackMode => "*" == UserSelectedProductIds;
}