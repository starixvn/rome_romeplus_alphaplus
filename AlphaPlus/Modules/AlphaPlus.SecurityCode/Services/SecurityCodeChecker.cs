using AlphaPlus.SecurityCode.Configurations;
using AlphaPlus.SecurityCode.Web;
using Microsoft.Extensions.Options;

namespace AlphaPlus.SecurityCode.Services;

public interface ISecurityCodeChecker
{
    bool IsRequiredSecurityCode(bool isBetLists, bool isStatement);

    bool IsRequiredSecurityCode(string relativePath);

    bool IsEnableSecurityCode();

    bool IsForcedChangeSecurityCode();
}

public class SecurityCodeChecker(IOptions<SecurityCodeOptions> options) : ISecurityCodeChecker
{
    private readonly SecurityCodeOptions options = options.Value;

    public bool IsEnableSecurityCode()
    {
        if (!options.IsActive)
        {
            return false;
        }

        //// If forced change, then user have been offered security code
        if (IsForcedChangeSecurityCode())
        {
            return true;
        }

        //// User is not belong to site needed check security code
        if (options.Sites.Any()
            && !string.IsNullOrWhiteSpace(SecurityCodeSessionManager.SecurityCodeSession.UserSite)
            && !options.Sites.Contains(SecurityCodeSessionManager.SecurityCodeSession.UserSite, StringComparer.OrdinalIgnoreCase))
        {
            return false;
        }

        //// If user do not have security code, then user are not offered SC
        if (string.IsNullOrWhiteSpace(SecurityCodeSessionManager.SecurityCodeSession.SecurityCode))
        {
            return false;
        }

        return true;
    }

    public bool IsForcedChangeSecurityCode()
        => !SecurityCodeSessionManager.SecurityCodeSession.IsInternal
            && SecurityCodeSessionManager.SecurityCodeSession.IsForcedChangeSecurityCode;

    public bool IsRequiredSecurityCode(bool isBetLists, bool isStatement)
    {
        if (ByPassSecurity())
        {
            return false;
        }

        if (!SecurityCodeSessionManager.SecurityCodeSession.IsEnabledOtp
            && !IsEnableSecurityCode())
        {
            return false;
        }

        if (isBetLists && SecurityCodeSessionManager.SecurityCodeSession.IsEnabledSecurityCodeInBetLists)
        {
            return true;
        }

        if (isStatement && SecurityCodeSessionManager.SecurityCodeSession.IsEnabledSecurityCodeInStatement)
        {
            return true;
        }

        return false;
    }

    public bool IsRequiredSecurityCode(string relativePath)
    {
        if (ByPassSecurity())
        {
            return false;
        }

        if (!SecurityCodeSessionManager.SecurityCodeSession.IsEnabledOtp && !IsEnableSecurityCode())
        {
            return false;
        }

        if (string.IsNullOrWhiteSpace(relativePath))
        {
            return false;
        }

        return IsRequiredSecurity(relativePath)
            || IsRequiredSecurityCodeInBetLists(relativePath)
            || IsRequiredSecurityCodeInStatement(relativePath);
    }

    private static bool ByPassSecurity()
        => SecurityCodeSessionManager.SecurityCodeSession.IsAlreadyInputSecurityCode
            || SecurityCodeSessionManager.SecurityCodeSession.IsInternal;

    private bool IsRequiredSecurity(string relativePath)
    {
        foreach (var url in options.Paths)
        {
            if (relativePath.Contains(url, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
        }

        return false;
    }

    private bool IsRequiredSecurityCodeInBetLists(string relativePath)
        => SecurityCodeSessionManager.SecurityCodeSession.IsEnabledSecurityCodeInBetLists
            && options.BetListsPaths != null
            && options.BetListsPaths.Any(url => relativePath.Contains(url, StringComparison.OrdinalIgnoreCase));

    private bool IsRequiredSecurityCodeInStatement(string relativePath)
        => SecurityCodeSessionManager.SecurityCodeSession.IsEnabledSecurityCodeInStatement
            && options.BetListsPaths != null
            && options.StatementPaths.Any(url => relativePath.Contains(url, StringComparison.OrdinalIgnoreCase));

}
