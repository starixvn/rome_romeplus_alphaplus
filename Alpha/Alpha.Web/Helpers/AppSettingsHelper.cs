namespace Alpha.Web.Helpers
{
    using System;
    using AlphaPlus.Configurations;

    public static class AppSettingsHelper
    {
        public static bool GetUpdateCreditReplicateServer() => AppSettings.UpdateCreditBothDBCustAndDBAge;

        public static int GetInsertProfilePictureSiteMode() => AppSettings.InsertProfilePictureSiteMode;

        public static bool HideProductStatusBySites(string site) =>
             Array.Exists(
                 AppSettings.HideProductStatusSites,
                 disallowedSite => disallowedSite.Equals(site, StringComparison.OrdinalIgnoreCase));

        public static string GetProductConfigsFileRelativePath() => AppSettings.ProductConfigsFileRelativePath;

        public static string GetQRCodeMemberSiteLoginConfigsPath() => AppSettings.QRCodeMemberSiteLoginConfigsPath;
    }
}
