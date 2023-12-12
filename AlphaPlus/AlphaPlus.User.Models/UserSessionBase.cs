using System;
using System.Globalization;
using System.Reflection;
using AlphaPlus.Configurations;
using AlphaPlus.User.Models.Helpers;

namespace AlphaPlus.User.Models
{
    [Serializable]
    public class UserSessionBase : IUserSession, IDisposable
    {
        private const string DEFAULT_LANGUAGE = "en-US";
        private const string ISBELONGTO9CURRENCY = "ISBELONGTO9CURRENCY";
        /// <summary>
        /// Values: E,J,CT,CS,TH,KO.
        /// </summary>
        private const string LANGKEY = "LANGKEY";
        private const string CAPTCHACODE = "CAPTCHACODE";
        private const string ADMINID = "ADMINID";
        private const string ADMINNAME = "ADMINNAME";
        private const string CUSTID = "CUSTID";
        private const string ROLEID = "ROLEID";
        private const string SERVERID = "SERVERID";
        private const string USERNAME = "USERNAME";
        private const string NICKNAME = "NICKNAME";
        private const string DISPLAYNICKNAME = "DISPLAYNICKNAME";
        private const string ONUSERID = "ONUSERID";
        private const string SUBACCID = "SUBACCID";
        private const string SUBPAGEACCESS = "SUBPAGEACCESS";
        private const string SUBUSERNAME = "SUBUSERNAME";
        private const string SITE = "SITE";
        private const string SITENAME = "SITENAME";
        private const string CURRENCY = "CURRENCY";
        private const string CURRENCYID = "CURRENCYID";
        private const string ISLOGGED = "ISLOGGED";
        private const string ISPASSWORDEXPIRED = "ISPASSWORDEXPIRED";
        private const string LASTCHANGEDPASSWORD = "LASTCHANGEDPASSWORD";
        private const string ISFORCEDSECURITYCODE = "ISFORCEDSECURITYCODE";
        private const string SECURITYCODE = "SECURITYCODE";
        private const string SHOWSECURITYCODEPOPUP = "SHOWSECURITYCODEPOPUP";
        private const string SECURITYCODETOKEN = "SECURITYCODETOKEN";
        private const string ISENABLEDSECURITYCODEINBETLISTS = "ISENABLEDSECURITYCODEINBETLISTS";
        private const string ISENABLEDSECURITYCODEINSTATEMENT = "ISENABLEDSECURITYCODEINSTATEMENT";
        private const string ISNICKNAMELOGINONLY = "ISNICKNAMELOGINONLY";
        private const string ISADMINCANVIEWVNIP = "ISADMINCANVIEWVNIP";
        private const string ISNICKNAME = "ISNICKNAME";
        private const string ISSHOWCONSOLIDATEDREPORT = "ISSHOWCONSOLIDATEDREPORT";
        private const string ISSHOWBETCHOICELIMIT = "ISSHOWBETCHOICELIMIT";
        private const string ISDOUBLECOMMLOSING = "DOUBLECOMMLOSING";
        private const string ISDISABLEDAUTOMPT = "ISDISABLEDAUTOMPT";
        private const string SRECOMMENDID = "SRECOMMENDID";
        private const string MRECOMMENDID = "MRECOMMENDID";
        private const string ARECOMMENDID = "ARECOMMENDID";
        private const string ISALLOWEDTOTALBALANCETRANSFER = "ISALLOWEDTOTALBALANCETRANSFER";
        private const string LANGUAGE = "LANGUAGE";
        private const string ISLIMITHOSTPERMISSIONONSUBACCOUNTFUNCTION = "ISLIMITHOSTPERMISSIONONSUBACCOUNTFUNCTION";
        private const string ISMYANMARODDS = "ISMYANMARODDS";
        private const string DISABLESUPERLIVE = "DISABLESUPERLIVE";
        private const string HIDEPRODUCT = "HIDEPRODUCT";
        private const string DISABLEDPRODUCT = "DISABLEDPRODUCT";
        private const string SYNCEDPRODUCT = "SYNCEDPRODUCT";
        private const string USERLOGINIP = "USERLOGINIP";
        private const string ISSHOWSKIPFORCECHANGPASSWORD = "ISSHOWSKIPFORCECHANGPASSWORD";
        private const string IndirectCustIdSessionKey = "IndirectCustId";
        private const string IndirectSubAccIdSessionKey = "IndirectSubAccId";
        private const string IndirectUsernameSessionKey = "IndirectUsername";
        private const string DARKMODENAMESESSIONKEY = "DARKMODENAME";
        private const string ASSETAPPPATHSESSIONKEY = "ASSETAPPPATH";
        private const string ISOFFERPROTECTEDMODESESSIONKEY = "ISOFFERPROTECTEDMODE";
        private const string ISPROTECTEDMODESESSIONKEY = "ISPROTECTEDMODE";
        private const string AUTOFOLLOWBETISDUMMYAGENTSESSIONKEY = "AUTOFOLLOWBETISDUMMYAGENT";
        private const string ISINTERNALWEBVPN = "ISINTERNALWEBVPN";
        private const string AGIPSESSIONKEY = "AGIP";
        private const string SITEDBGROUPIDSESSIONKEY = "SITEDBGROUPID";
        private const string ISHIDESPORTSBOOK2 = "ISHIDESPORTSBOOK2";
        private const string DISABLESPORTSBOOK2 = "DISABLESPORTSBOOK2";
        private const string ISDISABLEDESPORTS = "ISDISABLEDESPORTS";

        private readonly IHttpContextHelper _httpContextHelper;

        public UserSessionBase(IHttpContextHelper httpContextHelper)
        {
            _httpContextHelper = httpContextHelper;
        }

        public string NetSessionId => _httpContextHelper.SessionId;

        public bool IsLimitedHostPermisionOnSubAccountFunction
        {
            get => GetProperty(ISLIMITHOSTPERMISSIONONSUBACCOUNTFUNCTION, false);
            set => SetProperty(ISLIMITHOSTPERMISSIONONSUBACCOUNTFUNCTION, value);
        }

        public string Language
        {
            get => GetProperty(LANGUAGE, DEFAULT_LANGUAGE);
            set => SetProperty(LANGUAGE, value);
        }

        public string LangKey
        {
            get => GetProperty(LANGKEY, "E");
            set => SetProperty(LANGKEY, value);
        }

        public CultureInfo Culture => new CultureInfo(Language);

        public string CaptchaCode
        {
            get => GetProperty<string>(CAPTCHACODE);
            set => SetProperty(CAPTCHACODE, value);
        }

        public bool IsBelongTo9Currency
        {
            get => GetProperty(ISBELONGTO9CURRENCY, false);
            set => SetProperty(ISBELONGTO9CURRENCY, value);
        }

        public int AdminId
        {
            get => GetProperty(ADMINID, 0);
            set => SetProperty(ADMINID, value);
        }

        public string AdminName
        {
            get => GetProperty<string>(ADMINNAME);
            set => SetProperty(ADMINNAME, value);
        }

        public int CustId
        {
            get => GetProperty(CUSTID, 0);
            set => SetProperty(CUSTID, value);
        }

        public byte RoleId
        {
            get => GetProperty(ROLEID, byte.MinValue);
            set => SetProperty(ROLEID, value);
        }

        public int ServerId
        {
            get => GetProperty(SERVERID, 0);
            set => SetProperty(SERVERID, value);
        }

        public string UserName
        {
            get => GetProperty<string>(USERNAME);
            set => SetProperty(USERNAME, value);
        }

        public string NickName
        {
            get => GetProperty<string>(NICKNAME);
            set => SetProperty(NICKNAME, value);
        }

        public string DisplayNickName
        {
            get => GetProperty<string>(DISPLAYNICKNAME);
            set => SetProperty(DISPLAYNICKNAME, value);
        }

        public bool IsNicknameLoginOnly
        {
            get => GetProperty(ISNICKNAMELOGINONLY, false);
            set => SetProperty(ISNICKNAMELOGINONLY, value);
        }

        public bool IsNickname
        {
            get => GetProperty(ISNICKNAME, false);
            set => SetProperty(ISNICKNAME, value);
        }

        public int OnUserId
        {
            get => GetProperty(ONUSERID, 0);
            set => SetProperty(ONUSERID, value);
        }

        public int SubAccId
        {
            get => GetProperty(SUBACCID, 0);
            set => SetProperty(SUBACCID, value);
        }

        public bool IsSubAcc => SubAccId != 0;

        public bool IsInternal => AdminId != 0;

        public string Site
        {
            get => GetProperty<string>(SITE);
            set => SetProperty(SITE, value);
        }

        public PageAccess SubPageAccess
        {
            get => GetProperty(SUBPAGEACCESS, new PageAccess());
            set => SetProperty(SUBPAGEACCESS, value);
        }

        public string SubUserName
        {
            get => GetProperty<string>(SUBUSERNAME);
            set => SetProperty(SUBUSERNAME, value);
        }

        public string SiteName
        {
            get => GetProperty<string>(SITENAME);
            set => SetProperty(SITENAME, value);
        }

        public int UserId => IsInternal ? AdminId : CustId;

        public string Currency
        {
            get => GetProperty<string>(CURRENCY);
            set => SetProperty(CURRENCY, value);
        }

        public int CurrencyId
        {
            get => GetProperty(CURRENCYID, 0);
            set => SetProperty(CURRENCYID, value);
        }

        public bool IsLogged
        {
            get => GetProperty(ISLOGGED, false);
            set => SetProperty(ISLOGGED, value);
        }

        public bool IsPasswordExpired
        {
            get => GetProperty(ISPASSWORDEXPIRED, false);
            set => SetProperty(ISPASSWORDEXPIRED, value);
        }

        public DateTime LastChangedPassword
        {
            get => GetProperty(LASTCHANGEDPASSWORD, DateTime.MinValue);
            set => SetProperty(LASTCHANGEDPASSWORD, value);
        }

        public bool IsAdminCanViewVNIP
        {
            get => GetProperty(ISADMINCANVIEWVNIP, false);
            set => SetProperty(ISADMINCANVIEWVNIP, value);
        }

        public bool IsAgent => RoleId == (int)UserTypes.Agent;

        public bool IsMaster => RoleId == (int)UserTypes.Master;

        public bool IsSuper => RoleId == (int)UserTypes.Super;

        public bool IsShowConsolidatedReport
        {
            get => GetProperty(ISSHOWCONSOLIDATEDREPORT, false);
            set => SetProperty(ISSHOWCONSOLIDATEDREPORT, value);
        }

        public bool IsForcedSecurityCode
        {
            get => GetProperty(ISFORCEDSECURITYCODE, false);
            set => SetProperty(ISFORCEDSECURITYCODE, value);
        }

        public string SecurityCode
        {
            get => GetProperty<string>(SECURITYCODE);
            set => SetProperty(SECURITYCODE, value);
        }

        public bool ShowSecurityCodePopup
        {
            get => GetProperty(SHOWSECURITYCODEPOPUP, false);
            set => SetProperty(SHOWSECURITYCODEPOPUP, value);
        }

        public string SecurityCodeToken
        {
            get => GetProperty(SECURITYCODETOKEN, string.Empty);
            set => SetProperty(SECURITYCODETOKEN, value);
        }

        public bool IsEnabledSecurityCodeInBetLists
        {
            get => GetProperty(ISENABLEDSECURITYCODEINBETLISTS, false);
            set => SetProperty(ISENABLEDSECURITYCODEINBETLISTS, value);
        }

        public bool IsEnabledSecurityCodeInStatement
        {
            get => GetProperty(ISENABLEDSECURITYCODEINSTATEMENT, false);
            set => SetProperty(ISENABLEDSECURITYCODEINSTATEMENT, value);
        }

        public bool IsDoubleCommLosing
        {
            get => GetProperty(ISDOUBLECOMMLOSING, false);
            set => SetProperty(ISDOUBLECOMMLOSING, value);
        }

        public bool IsDisabledAutoMPT
        {
            get => GetProperty(ISDISABLEDAUTOMPT, false);
            set => SetProperty(ISDISABLEDAUTOMPT, value);
        }

        public int SRecommendId
        {
            get => GetProperty(SRECOMMENDID, 0);
            set => SetProperty(SRECOMMENDID, value);
        }

        public int MRecommendId
        {
            get => GetProperty(MRECOMMENDID, 0);
            set => SetProperty(MRECOMMENDID, value);
        }

        public int ARecommendId
        {
            get => GetProperty(ARECOMMENDID, 0);
            set => SetProperty(ARECOMMENDID, value);
        }

        public bool IsAllowedTotalBalanceTransfer
        {
            get => GetProperty(ISALLOWEDTOTALBALANCETRANSFER, false);
            set => SetProperty(ISALLOWEDTOTALBALANCETRANSFER, value);
        }

        public bool IsLimitedHostPermissionOnSubAccountFunction
        {
            get => GetProperty(ISLIMITHOSTPERMISSIONONSUBACCOUNTFUNCTION, false);
            set => SetProperty(ISLIMITHOSTPERMISSIONONSUBACCOUNTFUNCTION, value);
        }

        public bool IsShowBetChoiceLimit
        {
            get => GetProperty(ISSHOWBETCHOICELIMIT, false);
            set => SetProperty(ISSHOWBETCHOICELIMIT, value);
        }

        public bool IsMyanmarOdds
        {
            get => GetProperty(ISMYANMARODDS, false);
            set => SetProperty(ISMYANMARODDS, value);
        }

        public bool IsDisabledSuperLive
        {
            get => GetProperty(DISABLESUPERLIVE, false);
            set => SetProperty(DISABLESUPERLIVE, value);
        }

        public string UserLoginIp
        {
            get => GetProperty<string>(USERLOGINIP);
            set => SetProperty(USERLOGINIP, value);
        }

        public bool IsInternalWebVPN
        {
            get => GetProperty(ISINTERNALWEBVPN, false);
            set => SetProperty(ISINTERNALWEBVPN, value);
        }

        public bool IsShowSkipForceChangePassword
        {
            get => GetProperty<bool>(ISSHOWSKIPFORCECHANGPASSWORD);
            set => SetProperty(ISSHOWSKIPFORCECHANGPASSWORD, value);
        }

        public bool IsIndirectLoggedIn => IndirectCustId > 0 || IndirectSubAccId > 0;

        public int IndirectCustId
        {
            get => GetProperty<int>(IndirectCustIdSessionKey);
            set => SetProperty(IndirectCustIdSessionKey, value);
        }

        public int IndirectSubAccId
        {
            get => GetProperty<int>(IndirectSubAccIdSessionKey);
            set => SetProperty(IndirectSubAccIdSessionKey, value);
        }

        public string IndirectUsername
        {
            get => GetProperty<string>(IndirectUsernameSessionKey);
            set => SetProperty(IndirectUsernameSessionKey, value);
        }

        public bool IsOfferProtectedMode
        {
            get => GetProperty<bool>(ISOFFERPROTECTEDMODESESSIONKEY);
            set => SetProperty(ISOFFERPROTECTEDMODESESSIONKEY, value);
        }

        public bool IsProtectedMode
        {
            get => GetProperty<bool>(ISPROTECTEDMODESESSIONKEY);
            set => SetProperty(ISPROTECTEDMODESESSIONKEY, value);
        }

        public string DarkModeName
        {
            get => GetProperty<string>(DARKMODENAMESESSIONKEY) ?? string.Empty;
            set => SetProperty(DARKMODENAMESESSIONKEY, value);
        }

        public string AssetAppPath
        {
            get => GetProperty<string>(ASSETAPPPATHSESSIONKEY) ?? string.Empty;
            set => SetProperty(ASSETAPPPATHSESSIONKEY, value);
        }

        public bool AutoFollowBetIsDummyAgent
        {
            get => GetProperty<bool>(AUTOFOLLOWBETISDUMMYAGENTSESSIONKEY);
            set => SetProperty(AUTOFOLLOWBETISDUMMYAGENTSESSIONKEY, value);
        }

        public string AGIP
        {
            get => GetProperty<string>(AGIPSESSIONKEY) ?? AppSettings.DBTXN_IP;
            set => SetProperty(AGIPSESSIONKEY, value);
        }

        public int SiteDBGroupId
        {
            get => GetProperty<int>(SITEDBGROUPIDSESSIONKEY);
            set => SetProperty(SITEDBGROUPIDSESSIONKEY, value);
        }

        public bool IsHideSportsbook2
        {
            get => GetProperty(ISHIDESPORTSBOOK2, true);
            set => SetProperty(ISHIDESPORTSBOOK2, value);
        }

        public bool DisableSportsbook2
        {
            get => GetProperty(DISABLESPORTSBOOK2, true);
            set => SetProperty(DISABLESPORTSBOOK2, value);
        }

        public bool IsDisabledESports
        {
            get => GetProperty(ISDISABLEDESPORTS, false);
            set => SetProperty(ISDISABLEDESPORTS, value);
        }

        public bool GetDisabledProduct(int productId) => GetProperty(DISABLEDPRODUCT + "_" + productId, true);

        public void SetDisabledProduct(int productId, bool value) => SetProperty(DISABLEDPRODUCT + "_" + productId, value);

        public bool GetHideProduct(int productId) => GetProperty(HIDEPRODUCT + "_" + productId, true);

        public void SetHideProduct(int productId, bool value) => SetProperty(HIDEPRODUCT + "_" + productId, value);

        public bool GetSyncedProduct(int productId) => GetProperty(SYNCEDPRODUCT + "_" + productId, false);

        public void SetSyncedProduct(int productId, bool value) => SetProperty(SYNCEDPRODUCT + "_" + productId, value);

        public T GetProperty<T>(string propertyName, T defaultValue = default)
        {
            var value = _httpContextHelper.GetSession(propertyName);
            return value == null ? defaultValue : (T)value;
        }

        public bool HasProperty(string propertyName)
        {
            var value = _httpContextHelper.GetSession(propertyName);
            return value != null;
        }

        public void SetProperty(string propertyName, object value)
        {
            _httpContextHelper.SetSession(propertyName, value);
        }

        public void Dispose()
        {
            Type type = GetType();
            PropertyInfo[] p = type.GetProperties(BindingFlags.Public | BindingFlags.SetProperty);

            int c = p.Length;
            for (int i = 0; i < c; i++)
            {
                SetProperty(p[i].Name.ToUpper(), null);
            }
        }
    }
}