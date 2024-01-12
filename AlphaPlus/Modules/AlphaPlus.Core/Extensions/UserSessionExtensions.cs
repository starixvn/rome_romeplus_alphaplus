using System.Collections;
using System.Text.RegularExpressions;
using AlphaPlus.Core.Configurations;
using AlphaPlus.Core.Models;

namespace AlphaPlus.Core.Extensions;

public static class UserSessionExtensions
{
    public static void RemoveUserSession(this IUserSession userSession)
    {
        var session = userSession as UserSessionBase;
        session?.Dispose();
    }

    public static void SetUserSessionValues(this IUserSession session, Hashtable values, Action<IUserSession, Hashtable> setUserStatusAction)
    {
        if (session == null)
        {
            return;
        }

        try
        {
            session.CustId = Convert.ToInt32(values["custid"]);
            session.UserName = (values["username"] as string)?.ToUpperInvariant().Trim();
            session.SubAccId = Convert.ToInt32(values["subaccid"]);
            session.SubUserName = values["subaccname"] as string;
            session.SubPageAccess = (Models.PageAccess)(values["subpageaccess"] as string) ?? new Models.PageAccess();
            if (!session.IsSubAcc)
            {
                session.SubPageAccess.SetFullPermission();
            }

            session.SiteName = values["sitename"] as string;
            session.Site = values["site"] as string;
            session.Currency = values["currency"] as string;
            session.CurrencyId = Convert.ToInt32(values["currencyid"]);
            session.RoleId = Convert.ToByte(values["roleid"]);
            session.ServerId = Convert.ToInt32(values["serverid"]);
            session.AdminId = Convert.ToInt32(values["adminid"]);
            session.AdminName = values["accusername"] as string;
            session.IsLogged = session.IsInternal || !Convert.ToBoolean(values["isfirstlogin"]);

            // Security code
            session.IsForcedSecurityCode = (bool)values["isforcedchangesecuritycode"];
            session.SecurityCode = values["securitycode"] as string;
        }
        catch
        {
        }


        try
        {
            DateTime pwdExpiry = (DateTime)values["pwdexpiry"];
            session.IsPasswordExpired = DateTime.Compare(DateTime.Now, pwdExpiry) >= 0;

            var defaultDateSkipForceChangePass = new DateTime(1900, 1, 1);
            session.IsShowSkipForceChangePassword = session.IsLogged && DateTime.Compare(pwdExpiry, defaultDateSkipForceChangePass) > 0;
        }
        catch
        {
            session.IsPasswordExpired = false;
            session.IsShowSkipForceChangePassword = false;
        }

        if (values.Contains("doublecommlosing"))
        {
            session.IsDoubleCommLosing = (bool)values["doublecommlosing"];
        }

        if (values.Contains("nickname"))
        {
            session.NickName = (values["nickname"] + string.Empty).ToUpperInvariant();
        }

        if (values.Contains("nicknameloginonly"))
        {
            session.IsNicknameLoginOnly = !string.IsNullOrEmpty(values["nicknameloginonly"] + string.Empty) && (bool)values["nicknameloginonly"];
        }

        if (values.Contains("isnickname"))
        {
            session.IsNickname = !string.IsNullOrEmpty(values["isnickname"] + string.Empty) && (bool)values["isnickname"];
        }

        session.DisplayNickName = session.IsNickname ? session.NickName : session.UserName;

        if (values.Contains("on_userid") && values["on_userid"] != null && values["on_userid"] != DBNull.Value)
        {
            session.OnUserId = int.Parse(values["on_userid"].ToString());
        }

        SetProductStatus(session, values);
        SetProductStatusToDictionary(session, values);

        setUserStatusAction(session, values);

        try
        {
            session.IsDisabledAutoMPT = (bool)values["disableautompt"];
            session.SRecommendId = Convert.ToInt32(values["srecommend"]);
            session.MRecommendId = Convert.ToInt32(values["mrecommend"]);
            session.ARecommendId = Convert.ToInt32(values["arecommend"]);
            session.IsAdminCanViewVNIP = (bool)values["isadmincanviewvnip"];
            session.IsAllowedTotalBalanceTransfer = (bool)values["isallowedtotalbalancetransfer"];
            session.IsShowConsolidatedReport = (bool)values["isshowconsolidated"];
            session.IsShowBetChoiceLimit = (bool)values["isShowBetChoiceLimit"];
            session.AGIP = values["AGIP"].ToString();
            session.SiteDBGroupId = Convert.ToInt32(values["SiteDBGroupId"]);

            bool isMyanmarOdds = false;
            if (values["ismyanmarodds"] != null && values["ismyanmarodds"] != DBNull.Value)
            {
                _ = bool.TryParse(values["ismyanmarodds"].ToString(), out isMyanmarOdds);
            }

            session.IsMyanmarOdds = isMyanmarOdds;

            if (!string.IsNullOrWhiteSpace(AppSettings.SubAccountPermission_LockAdminAccounts)
                && Regex.IsMatch(string.Format(",{0},", AppSettings.SubAccountPermission_LockAdminAccounts), string.Format(",{0},", session.AdminId), RegexOptions.IgnoreCase)
                && session.IsInternal)
            {
                session.IsLimitedHostPermissionOnSubAccountFunction = true;
            }
            else
            {
                session.IsLimitedHostPermissionOnSubAccountFunction = false;
            }
        }
        catch { }
    }

    private static void SetProductStatus(IUserSession session, Hashtable values)
    {
        session.IsHideSportsbook2 = GetProductStatus("hidemuaythai", values, true);
        session.DisableSportsbook2 = GetProductStatus("disablemuaythai", values, true);

        session.IsDisabledESports = GetProductStatus("disableesports", values, true);

        session.IsDisabledSuperLive = GetProductStatus("disablesuperlive", values, true);
    }

    private static void SetProductStatusToDictionary(IUserSession session, Hashtable values)
    {
        session.SetHideProduct(ProductIdConstants.Sportsbook2, GetProductStatus("hidemuaythai", values, true));
        session.SetDisabledProduct(ProductIdConstants.Sportsbook2, GetProductStatus("disablemuaythai", values, true));
        session.SetSyncedProduct(ProductIdConstants.LiveCasino, GetProductStatus("syncedlivecasino", values, true));

        ProductTransform.TransformProductStatus(session, values);
    }

    private static bool GetProductStatus(string productStatus, Hashtable values, bool defaultValue)
    {
        return values.Contains(productStatus) ? (bool)values[productStatus] : defaultValue;
    }
}