using AlphaPlus.Settings.MemberInfo.Domain.Constants;

namespace AlphaPlus.Settings.Site.Features.Shared.Helpers;

public static class BackgroundColorHelper
{
    private const int Two = 2;

    public static string GetClassNameByAccountStatus(
        bool enabled,
        bool closed,
        byte closedReason,
        bool suspended,
        byte locked,
        int? rowNumber = -1)
    {
        if (!enabled)
        {
            return "disabled-bg";
        }

        if (closed)
        {
            return GetClassNameByClosedStatus(closedReason);
        }

        if (suspended)
        {
            return "suspended-bg";
        }

        if (locked != 0)
        {
            return GetClassNameByLockedAccess(locked);
        }

        if (rowNumber == -1)
        {
            return string.Empty;
        }

        return rowNumber % Two == 0 ? "even-row" : string.Empty;
    }

    private static string GetClassNameByClosedStatus(byte closedReason)
    {
        switch (closedReason)
        {
            case CloseReasonConstants.ByAdmin:
                return "closed-byadmin";

            case CloseReasonConstants.ByUpline:
                return "closed-byupline";

            case CloseReasonConstants.ByArchive:
                return "closed-byArchive";

            default:
                return string.Empty;
        }
    }

    private static string GetClassNameByLockedAccess(byte locked)
    {
        switch (locked)
        {
            case LockedAccessConstant.FalseOTPSC:
                return "locked-falseOTPSC";

            case LockedAccessConstant.FalsePassword:
                return "locked-falsePassword";

            default:
                return string.Empty;
        }
    }
}