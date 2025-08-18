namespace AlphaPlus.CashService.Site.Features.Promotion.Helpers;

using Ganss.Xss;

public static class InputCleaner
{
    private static readonly HtmlSanitizer Sanitizer = new();

    static InputCleaner()
    {
        Sanitizer.AllowedTags.Remove("script");
        var attrsToRemove = Sanitizer.AllowedAttributes
            .Where(a => a.StartsWith("on", StringComparison.OrdinalIgnoreCase))
            .ToList();

        foreach (var attr in attrsToRemove)
            Sanitizer.AllowedAttributes.Remove(attr);
    }

    public static string Clean(string input) => string.IsNullOrWhiteSpace(input) ? input : Sanitizer.Sanitize(input);
}