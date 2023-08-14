namespace Fanex.Razorbill.Security.Services.Authentication
{
    using System;
    using System.Configuration;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Web;
    using AlphaPlus.User.Models;
    using Fanex.Common;
    using Fanex.Razorbill.Security.Configurations;
    using Fanex.Razorbill.Security.Configurations.AuthenticationSection;
    using Fanex.Razorbill.Security.Entities;
    using Fanex.Razorbill.Security.Repositories.Authentication;

    public class AuthenticationService : IAuthenticationService
    {
        private string _customerClosedOrLockedSessionKey = "Fanex.Razorbill.Security.Services.Authentication.CheckClosedOrLockedStatus";
        private IUserSession _currentUserSession;
        private IAuthenticationRepository _authenticationRepsitory;

        private string _urlRequest;
        private object _blockStatus = new object();

        public AuthenticationService(string urlRequest)
        {
            _currentUserSession = UserSession.Current;
            _authenticationRepsitory = new AuthenticationRepository();
            _urlRequest = urlRequest;
        }

        public DateTime? CustomerClosedOrLockedStatus
        {
            get
            {
                return (DateTime?)HttpContext.Current.Session[string.Format("{0}_{1}", HttpContext.Current.Session.SessionID, _customerClosedOrLockedSessionKey)];
            }

            set
            {
                HttpContext.Current.Session[string.Format("{0}_{1}", HttpContext.Current.Session.SessionID, _customerClosedOrLockedSessionKey)] = value;
            }
        }

        public ClosedLockedStatus CheckClosedOrLockedStatus()
        {
            lock (_blockStatus)
            {
                if (HttpContext.Current.Session == null)
                {
                    return new ClosedLockedStatus(isActive: false);
                }

                if (IgnoreSpecialFileTypes())
                {
                    return new ClosedLockedStatus();
                }

                var closedOrLockedStatusInSesssion = CustomerClosedOrLockedStatus;

                if (closedOrLockedStatusInSesssion != null && closedOrLockedStatusInSesssion >= DateTime.Now)
                {
                    return new ClosedLockedStatus();
                }

                var closedOrLockedStatus = _authenticationRepsitory.CheckClosedOrLockedStatus(_currentUserSession.CustId, _currentUserSession.SubAccId);

                if (closedOrLockedStatus.IsClosed || closedOrLockedStatus.IsLocked)
                {
                    return closedOrLockedStatus;
                }

                CustomerClosedOrLockedStatus = DateTime.Now.AddSeconds(Converter.ToInt(ConfigurationManager.AppSettings["CustomerStatusExpireTime"]));

                return new ClosedLockedStatus();
            }
        }

        public bool IsAuthorized()
        {
            if (HttpContext.Current.Session == null)
            {
                return false;
            }

            var currentUrlAccess = GetUrlAccessByCustomerWithoutDirectoryPath();

            if (IgnoreSpecialFileTypes() ||
                currentUrlAccess.Equals("index.aspx", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            var isAuthorization = CheckAuthorizationAgainstConfigFile(currentUrlAccess.ToUpperInvariant());

            return isAuthorization;

        }

        public bool CheckBelonging(int custId, int uplineId, byte uplineRoleId, long transId, bool isDataWarehouse)
        {
            if (HttpContext.Current.Session == null)
            {
                return false;
            }

            var currentUrlAccess = GetUrlAccessByCustomerWithoutDirectoryPath();

            if (IgnoreSpecialFileTypes() ||
                currentUrlAccess.Equals("index.aspx", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            var isBelonging = VerifyRequestingUrlAgainstConfigFile(custId, uplineId, uplineRoleId, transId, isDataWarehouse, currentUrlAccess);

            return isBelonging;
        }

        private bool CheckAuthorizationAgainstConfigFile(string currentUrlAccess)
        {
            var deniedpages = DeniedPageHelper.GetDeniedPages(UserSession.Current.RoleId);

            return !deniedpages.Any(i => currentUrlAccess.Contains(i.Url.ToUpperInvariant()));
        }

        private bool IgnoreSpecialFileTypes()
        {
            var pattern = "([^\\s]+(\\.(?i)(jpg|png|gif|bmp|css|js))$)";

            Regex urlRegex = new Regex(pattern);

            return urlRegex.IsMatch(_urlRequest, (int)RegexOptions.IgnoreCase);
        }

        private string GetUrlAccessByCustomerWithoutDirectoryPath()
        {
            var virtualDirectory = WebAppSettings.VirtualDirectory;
            if (!string.IsNullOrWhiteSpace(virtualDirectory))
            {
                var regex = new Regex(string.Format("{0}/", virtualDirectory), RegexOptions.IgnoreCase);
                var urlWithOutVirtualDirectory = regex.Replace(_urlRequest, string.Empty);

                return urlWithOutVirtualDirectory;
            }

            return _urlRequest;
        }

        private bool VerifyRequestingUrlAgainstConfigFile(int custId, int uplineId, byte uplineRoleId, long transId, bool isDataWarehouse, string currentUrlAccess)
        {
            var isBelonging = true;
            var authenticationSection = AuthenticationSection.Instance;
            var permission = authenticationSection.FullPermission;

            // get all urls that allow he/she access by his/her level
            var levels = from q in authenticationSection.GroupLevel.Cast<GroupLevelElement>() where q.RoleId <= _currentUserSession.RoleId select q;
            if (levels.Any())
            {
                foreach (GroupLevelElement level in levels)
                {
                    // get setting of current url against config file
                    if (level != null && level.GroupLevel != null && level.GroupLevel.Count > 0)
                    {
                        var pageAccess = from q in level.GroupLevel.Cast<PageAccessElement>()
                                         where q.Url.Equals(currentUrlAccess, StringComparison.OrdinalIgnoreCase)
                                         select q;

                        if (pageAccess.Any())
                        {
                            foreach (PageAccessElement pageElement in pageAccess)
                            {
                                if (pageElement.IsCheckUpline
                                    && (_currentUserSession.CustId != uplineId
                                    || (_currentUserSession.RoleId != uplineRoleId && uplineRoleId > 0)))
                                {
                                    return false;
                                }

                                // Check belonging when:
                                // - downline id not equal 0
                                // - and current requesting url need to check belonging
                                // - and current requesting url not in special list: allow customers to update by themselves
                                if (custId != 0 && pageElement.IsCheckBelonging)
                                {
                                    if (!pageElement.IsAllowToDoByThemselves && _currentUserSession.CustId == custId)
                                    {
                                        return false;
                                    }

                                    if (pageElement.IsAllowToDoByThemselves && _currentUserSession.CustId == custId)
                                    {
                                        return true;
                                    }

                                    if (pageElement.IsViewedByGroupHeadline)
                                    {
                                        var belongingResultConsolidation = _authenticationRepsitory.Age_Rpt_CustomerConsolidatedGroup_CheckBelonging(custId, _currentUserSession.CustId, _currentUserSession.SubAccId, transId, _currentUserSession.IsInternal);
                                        if (belongingResultConsolidation.IsCrossLevel(_currentUserSession.RoleId) && !pageElement.IsAllowedCrossLevel)
                                        {
                                            return false;
                                        }

                                        return belongingResultConsolidation.IsActive;
                                    }

                                    if (isDataWarehouse)
                                    {
                                        return _authenticationRepsitory.Age_CustomerSelCheckBelonging_OLD(custId, _currentUserSession.CustId, transId);
                                    }

                                    var belongingResult = _authenticationRepsitory.Age_CustomerSelCheckBelonging(custId, _currentUserSession.CustId, _currentUserSession.SubAccId, transId, _currentUserSession.IsInternal);

                                    if (belongingResult.IsCrossLevel(_currentUserSession.RoleId) && !pageElement.IsAllowedCrossLevel)
                                    {
                                        return false;
                                    }

                                    return belongingResult.IsActive;
                                }
                            }

                            break;
                        }
                    }
                }
            }

            return isBelonging;
        }

        public bool IsMultipleCustomersBelonging(string custIds)
        {
            return _authenticationRepsitory
                .IsMultipleCustomersBelonging(custIds, _currentUserSession.CustId, _currentUserSession.SubAccId, _currentUserSession.IsInternal)
                .IsActive;
        }
    }
}