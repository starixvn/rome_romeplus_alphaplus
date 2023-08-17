using System;
using System.Globalization;

namespace AlphaPlus.User.Models
{
    public interface IUserSession
    {
        string NetSessionId { get; }

        bool IsLimitedHostPermisionOnSubAccountFunction { get; set; }

        string Language { get; set; }

        string LangKey { get; set; }

        CultureInfo Culture { get; }

        string CaptchaCode { get; set; }

        bool IsBelongTo9Currency { get; set; }

        int AdminId { get; set; }

        string AdminName { get; set; }

        int CustId { get; set; }

        byte RoleId { get; set; }

        int ServerId { get; set; }

        string UserName { get; set; }

        string NickName { get; set; }

        string DisplayNickName { get; set; }

        bool IsNicknameLoginOnly { get; set; }

        bool IsNickname { get; set; }

        int OnUserId { get; set; }

        int SubAccId { get; set; }

        bool IsSubAcc { get; }

        bool IsInternal { get; }

        string Site { get; set; }

        PageAccess SubPageAccess { get; set; }

        string SubUserName { get; set; }

        string SiteName { get; set; }

        int UserId { get; }

        string Currency { get; set; }

        int CurrencyId { get; set; }

        bool IsLogged { get; set; }

        bool IsPasswordExpired { get; set; }

        DateTime LastChangedPassword { get; set; }

        bool IsAdminCanViewVNIP { get; set; }

        bool IsAgent { get; }

        bool IsMaster { get; }

        bool IsSuper { get; }

        bool IsShowConsolidatedReport { get; set; }

        bool IsForcedSecurityCode { get; set; }

        string SecurityCode { get; set; }

        bool ShowSecurityCodePopup { get; set; }

        string SecurityCodeToken { get; set; }

        bool IsEnabledSecurityCodeInBetLists { get; set; }

        bool IsEnabledSecurityCodeInStatement { get; set; }

        bool IsDoubleCommLosing { get; set; }

        bool IsDisabledAutoMPT { get; set; }

        int SRecommendId { get; set; }

        int MRecommendId { get; set; }

        int ARecommendId { get; set; }

        bool IsAllowedTotalBalanceTransfer { get; set; }

        bool IsLimitedHostPermissionOnSubAccountFunction { get; set; }

        bool IsShowBetChoiceLimit { get; set; }

        bool IsMyanmarOdds { get; set; }

        bool IsDisabledSuperLive { get; set; }

        bool GetHideProduct(int productId);

        void SetHideProduct(int productId, bool value);

        bool GetSyncedProduct(int productId);

        void SetSyncedProduct(int productId, bool value);

        bool GetDisabledProduct(int productId);

        void SetDisabledProduct(int productId, bool value);

        string UserLoginIp { get; set; }

        bool IsInternalWebVPN { get; set; }

        bool IsShowSkipForceChangePassword { get; set; }

        bool IsIndirectLoggedIn { get; }

        int IndirectCustId { get; set; }

        int IndirectSubAccId { get; set; }

        string IndirectUsername { get; set; }

        string DarkModeName { get; set; }

        string AssetAppPath { get; set; }

        bool IsOfferProtectedMode { get; set; }

        bool IsProtectedMode { get; set; }

        bool AutoFollowBetIsDummyAgent { get; set; }

        string AGIP { get; set; }

        int SiteDBGroupId { get; set; }

        bool IsHideSportsbook2 { get; set; }

        bool DisableSportsbook2 { get; set; }

        bool IsDisabledESports { get; set; }

        T GetProperty<T>(string propertyName, T defaultValue = default);

        void SetProperty(string propertyName, object value);
    }
}