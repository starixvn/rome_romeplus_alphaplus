namespace AlphaPlus.AccountInfo.Site.Features.Shared.Helpers;

using Ganss.Xss;

public static class InputCleaner
{
    private static readonly HtmlSanitizer Sanitizer = new();

    static InputCleaner()
    {
        Sanitizer.AllowedTags.Clear();
        Sanitizer.AllowedAttributes.Clear();
        Sanitizer.AllowedCssProperties.Clear();
    }

    public static string CleanRemark(string input) 
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            return string.Empty;
        }

        return Sanitizer.Sanitize(input);
    }
}