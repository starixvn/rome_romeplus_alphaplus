namespace AlphaPlus.Features.Handbook.API.Constants
{
    public static class ReferenceURLConsts
    {
        public const string HandbookURL = "/features";
        public const string CustomerSettingInformation = "[CustomerSettingInformation]";

        public static bool IsReference(string refURL)
        {
            return string.Equals(refURL, CustomerSettingInformation, StringComparison.OrdinalIgnoreCase);
        }
    }
}