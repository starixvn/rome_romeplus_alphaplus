namespace Alpha.Handbook.Demo.Constants
{
    public class ReferenceURLConsts
    {
        public const string HandbookURL = "/features";
        public const string CustomerSettingInformation = "[CustomerSettingInformation]";

        public static bool IsReference(string refURL)
        {
            return string.Equals(refURL, CustomerSettingInformation, StringComparison.OrdinalIgnoreCase);
        }
    }
}