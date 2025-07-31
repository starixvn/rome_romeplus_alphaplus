using AlphaPlus.Features.Messages.API.Configurations;
using AlphaPlus.Features.Messages.API.Models.Constants;

namespace AlphaPlus.Features.Messages.API.Constants
{
    public static class MessageConstants
    {
        public static readonly Dictionary<byte, string> ParentSitesByLevel = new()
        {
            [UserLevel.Super] = AppSettings.MessageSuper,
            [UserLevel.Master] = AppSettings.MessageMaster,
            [UserLevel.Agent] = AppSettings.MessageAgent
        };

        public static readonly Dictionary<int, int> SpecialTypes = new()
        {
            [MessageType.Normal] = 0,
            [MessageType.Special] = 1,
            [MessageType.Personal] = 2
        };

        public const string ContentFontStartMarkup = "<font color=red>";
        public const string ContentFontEndMarkup = "</font>";
        public const int DefaultTotalMessageHistory = 100;
        public const int DefaultTotalSpecialMessageHistory = 500;
        public const string SiteMessageTag = "{features}";
        public const string ProductsTag = "{products}";
        public const int PageSize = 100;
        public const int PageIndex = 1;

    }
}
