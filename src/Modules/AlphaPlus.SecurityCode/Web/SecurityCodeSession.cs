using AlphaPlus.Core.Models;

namespace AlphaPlus.SecurityCode.Web;

public interface ISecurityCodeSession
{
    bool IsAlreadyInputSecurityCode { get; set; }

    string SecurityCode { get; }

    bool IsForcedChangeSecurityCode { get; }

    bool IsInternal { get; }

    string UserSite { get; }

    bool IsEnabledOtp { get; set; }

    bool IsEnabledSecurityCodeInBetLists { get; }

    bool IsEnabledSecurityCodeInStatement { get; }

    string MainDomain { get; }
}

public class SecurityCodeSession : ISecurityCodeSession
{
    private const string IsAlreadyInputSecurityCodeKey = "SecurityCodeSession.IsAlreadyInputSecurityCodeKey";
    private const string IsEnabledOtpKey = "SecurityCodeSession.IsEnabledOtp";
    private const string MainDomainKey = "MainDomain";

    public bool IsAlreadyInputSecurityCode
    {
        get => UserSession.Current.GetProperty<bool>(IsAlreadyInputSecurityCodeKey);
        set => UserSession.Current.SetProperty(IsAlreadyInputSecurityCodeKey, value);
    }

    public string SecurityCode => UserSession.Current.SecurityCode;

    public bool IsForcedChangeSecurityCode => UserSession.Current.IsForcedSecurityCode;

    public bool IsInternal => UserSession.Current.IsInternal;

    public string UserSite => UserSession.Current.Site;

    public bool IsEnabledOtp
    {
        get => UserSession.Current.GetProperty<bool>(IsEnabledOtpKey);
        set => UserSession.Current.SetProperty(IsEnabledOtpKey, value);
    }

    public bool IsEnabledSecurityCodeInBetLists => UserSession.Current.IsEnabledSecurityCodeInBetLists;

    public bool IsEnabledSecurityCodeInStatement => UserSession.Current.IsEnabledSecurityCodeInStatement;

    public string MainDomain => UserSession.Current.GetProperty<string>(MainDomainKey);
}
