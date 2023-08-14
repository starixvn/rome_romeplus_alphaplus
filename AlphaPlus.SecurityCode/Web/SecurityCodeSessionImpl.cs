using AlphaPlus.User.Models;

namespace AlphaPlus.SecurityCode.Web
{
    public class SecurityCodeSessionImpl : SecurityCodeSession
    {

        //// security code
        private const string IsAlreadyInputSecurityCodeKey = "SecurityCodeSession.IsAlreadyInputSecurityCodeKey";
        private const string IsEnabledOtpKey = "SecurityCodeSession.IsEnabledOtp";
        private const string MainDomainKey = "MainDomain";

        public bool IsAlreadyInputSecurityCode
        {
            get => UserSession.Current.GetProperty<bool>(IsAlreadyInputSecurityCodeKey);
            set => UserSession.Current.SetProperty(IsAlreadyInputSecurityCodeKey, value);
        }

        public string SecurityCode
        {
            get
            {
                return UserSession.Current.SecurityCode;
            }
        }

        public bool IsForcedChangeSecurityCode
        {
            get
            {
                return UserSession.Current.IsForcedSecurityCode;
            }
        }

        public bool IsInternal
        {
            get
            {
                return UserSession.Current.IsInternal;
            }
        }

        public string UserSite
        {
            get
            {
                return UserSession.Current.Site;
            }
        }

        public bool IsEnabledOtp
        {
            get => UserSession.Current.GetProperty<bool>(IsEnabledOtpKey);
            set => UserSession.Current.SetProperty(IsEnabledOtpKey, value);
        }

        public bool IsEnabledSecurityCodeInBetLists
            => UserSession.Current.IsEnabledSecurityCodeInBetLists;

        public bool IsEnabledSecurityCodeInStatement
            => UserSession.Current.IsEnabledSecurityCodeInStatement;

        public string MainDomain => UserSession.Current.GetProperty<string>(MainDomainKey);
    }
}