using AlphaPlus.Auth.Criteria;
using AlphaPlus.Core.Configurations;
using AlphaPlus.Core.Criteria;
using AlphaPlus.Core.Models;
using Fanex.Caching;
using Fanex.Data.Repository;
using Microsoft.Extensions.Options;

namespace AlphaPlus.Core.Services;

public interface IAuthorizationService
{
    Task<AccountStatus> GetAccountStatus();

    Task<bool> IsAccountActive();

    bool IsAuthorized(string url = null);

    Task<bool> IsBelonging();

    Task<KickOutInfo> IsKickedOut();
}

public class AuthorizationService(
    IOptions<AuthorizationOptions> options,
    IRequestContext requestContext,
    ICacheService cacheService,
    IDynamicRepository dynamicRepository)
    : IAuthorizationService
{
    private const string AccountStatusCacheKey = "AlphaPlus.Auth.CustContext.AccountStatusCacheKey_{0}_{1}";
    private const string RecheckInCacheKey = "AlphaPlus.Auth.CustContext.RecheckInCacheKey_{0}_{1}_{2}";

    private readonly AuthorizationOptions options = options.Value;
    private readonly IRequestContext requestContext = requestContext;
    private readonly ICacheService cacheService = cacheService;
    private readonly IDynamicRepository dynamicRepository = dynamicRepository;

    public Task<AccountStatus> GetAccountStatus()
    {
        if (requestContext.UserSession.IsInternal)
        {
            return Task.FromResult(new AccountStatus(true, 0));
        }

        var userSession = requestContext.UserSession;
        var command = new GetAccountStatusCommand(userSession.CustId, userSession.SubAccId);
        return cacheService.GetOrSetAsync(
            string.Format(AccountStatusCacheKey, userSession.CustId, userSession.SubAccId),
            () => GetAccountStatus(userSession.CustId, userSession.SubAccId),
            new CacheItemOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(options.CachedExpireTime)));
    }

    private async Task<AccountStatus> GetAccountStatus(int custId, int subAccId)
    {
        var command = new GetAccountStatusCommand(custId, subAccId);
        await dynamicRepository.ExecuteAsync(command);
        return new AccountStatus(command.IsActive, command.Locked);
    }

    public async Task<bool> IsAccountActive()
    {
        var accountStatus = await GetAccountStatus();
        return accountStatus.IsActive;
    }

    public bool IsAuthorized(string url = null)
    {
        url ??= requestContext.Url;

        if (string.IsNullOrEmpty(url))
        {
            return true;
        }

        var subAccHasPermissionToViewUrl = true;
        if (requestContext.UserSession.SubAccId > 0)
        {
            subAccHasPermissionToViewUrl = CheckSubAccHasPermissionToViewUrl(
                requestContext.UserSession.SubPageAccess
                , url);
        }

        return HasPermissionToViewUrl(url) && subAccHasPermissionToViewUrl;
    }

    public async Task<bool> IsBelonging()
    {
        var isBelonging = true;
        var currentUrl = requestContext.Url;

        foreach (var groupPageAccess in options.GroupPageAccesses)
        {
            if (groupPageAccess.RoleId <= requestContext.UserSession.RoleId)
            {
                foreach (var pageAccess in groupPageAccess.PageAccesses)
                {
                    if (currentUrl.Contains(pageAccess.Url, StringComparison.OrdinalIgnoreCase) && pageAccess.IsCheckBelonging)
                    {
                        if (requestContext.CustId != 0)
                        {
                            return pageAccess.IsSupportedAutoFollowBetTool
                                && requestContext.UserSession.AutoFollowBetIsDummyAgent
                                ? true
                                : await IsBelongingOnPage(pageAccess);
                        }

                        if (!string.IsNullOrEmpty(requestContext.CustIds))
                        {
                            return (await IsMultipleCustomersBelonging(requestContext)).IsActive;
                        }
                    }
                }
            }
        }

        return isBelonging;
    }

    public async Task<KickOutInfo> IsKickedOut()
    {
        if (requestContext.UserSession.IsInternal)
        {
            return new KickOutInfo(false);
        }

        if (options.IgnoreReCheckInCustIds.Contains(requestContext.UserSession.CustId))
        {
            return new KickOutInfo(false);
        }

        var onlineUserId = requestContext.UserSession.OnUserId;

        if (onlineUserId <= 0)
        {
            return new KickOutInfo(true);
        }

        var key = string.Format(RecheckInCacheKey, requestContext.UserSession.CustId, requestContext.UserSession.SubAccId, onlineUserId);
        var kickOutInfo = cacheService.Get<KickOutInfo>(key);

        if (kickOutInfo == null)
        {
            kickOutInfo = await IsKickedOut(onlineUserId);
            if (!kickOutInfo.IsKickedOut)
            {
                cacheService.Set(key, kickOutInfo, new CacheItemOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(options.CachedExpireTime)));
            }
        }

        return kickOutInfo;
    }

    private bool CheckSubAccHasPermissionToViewUrl(string permissions, string url)
    {
        foreach (var groupPageAccess in options.GroupPageAccesses)
        {
            if (groupPageAccess.RoleId <= requestContext.UserSession.RoleId)
            {
                foreach (var pageAccess in groupPageAccess.PageAccesses)
                {
                    if (url.Contains(pageAccess.Url, StringComparison.OrdinalIgnoreCase))
                    {
                        var isAuthorized = IsAuthorizedOnPage(permissions, pageAccess);

                        if (!isAuthorized)
                        {
                            return false;
                        }
                    }
                }
            }
        }

        return true;
    }

    private static bool IsAuthorizedOnPage(string permissions, Configurations.PageAccess pageElement)
    {
        var requiredPermission = pageElement.Permission.Split(',');

        foreach (var permission in requiredPermission)
        {
            if (permissions.Contains(permission))
            {
                return true;
            }
        }

        return false;
    }

    private bool HasPermissionToViewUrl(string currentUrlAccess)
    {
        var deniedPages = GetDeniedPages(requestContext.UserSession.RoleId);

        return !deniedPages.Any(url => currentUrlAccess.Contains(url, StringComparison.OrdinalIgnoreCase));
    }

    private string[] GetDeniedPages(int roleId)
    {
        var groupPageAccess = options?.GroupPageAccesses?.FirstOrDefault(i => i.RoleId == roleId);
        if (groupPageAccess != null)
        {
            return groupPageAccess.DeniedPages;
        }

        return [];
    }

    private async Task<bool> IsBelongingOnPage(Configurations.PageAccess pageAccess)
    {
        // Check belonging when:
        // - downline id not equal 0
        // - and current requesting url need to check belonging
        // - and current requesting url not in special list: allow customers to update by themselves
        if (requestContext.UserSession.CustId == requestContext.CustId)
        {
            return pageAccess.IsAllowToDoByThemselves;
        }

        if (pageAccess.IsViewedByGroupHeadline)
        {
            var belongingInfoGroupHeadline = await IsCustomerConsolidatedGroupBelonging(requestContext);

            if (belongingInfoGroupHeadline.IsCrossLevel(requestContext.UserSession.RoleId) && !pageAccess.IsAllowedCrossLevel)
            {
                return false;
            }

            return belongingInfoGroupHeadline.IsActive;
        }

        return requestContext.IsDataStoreInDataWarehouse
            ? await IsBelongingInDataWarehouse(requestContext)
            : await IsBelonging(requestContext, pageAccess);
    }

    internal async Task<bool> IsBelonging(IRequestContext requestContext, Configurations.PageAccess pageAccess)
    {
        var belongingInfo = await IsBelonging(requestContext);

        if (belongingInfo.RoleId == 0 || belongingInfo.RoleId > requestContext.UserSession.RoleId)
        {
            return false;
        }

        if (belongingInfo.RoleId == requestContext.UserSession.RoleId && !pageAccess.IsAllowToDoByThemselves)
        {
            return false;
        }

        if (belongingInfo.IsCrossLevel(requestContext.UserSession.RoleId) && !pageAccess.IsAllowedCrossLevel)
        {
            return false;
        }

        return belongingInfo.IsActive;
    }

    private async Task<BelongingInfo> IsBelonging(IRequestContext requestContext)
    {
        var command = new CheckBelongingCommand(
            requestContext.UserSession.CustId,
            requestContext.UserSession.SubAccId,
            requestContext.TransId,
            requestContext.CustId);
        await dynamicRepository.ExecuteAsync(command);
        return new BelongingInfo(command.RoleId, requestContext.UserSession.IsInternal || command.RoleId > 0 && command.IsActive);
    }

    private async Task<bool> IsBelongingInDataWarehouse(IRequestContext requestContext)
    {
        var command = new CheckBelongingInDataWarehouseCommand(requestContext.UserSession.CustId, requestContext.TransId, requestContext.CustId);
        await dynamicRepository.ExecuteAsync(command);
        return command.RoleId > 0;
    }

    private async Task<BelongingInfo> IsCustomerConsolidatedGroupBelonging(IRequestContext requestContext)
    {
        var command = new CheckConsolidateGroupBelongingCommand(
            requestContext.UserSession.CustId,
            requestContext.UserSession.SubAccId,
            requestContext.TransId,
            requestContext.CustId);
        await dynamicRepository.ExecuteAsync(command);
        return new BelongingInfo(command.RoleId, requestContext.UserSession.IsInternal || command.RoleId > 0 && command.IsActive);
    }

    private async Task<BelongingInfo> IsMultipleCustomersBelonging(IRequestContext requestContext)
    {
        var command = new CheckMultipleCustomerBelongingCommand(
            requestContext.UserSession.CustId,
            requestContext.UserSession.SubAccId,
            requestContext.CustIds);
        await dynamicRepository.ExecuteAsync(command);
        return new BelongingInfo(command.RoleId, requestContext.UserSession.IsInternal || command.RoleId > 0 && command.IsActive);
    }

    private async Task<KickOutInfo> IsKickedOut(int onlineUserId)
    {
        var command = new ReCheckInCommand(onlineUserId);
        await dynamicRepository.ExecuteAsync(command);
        return new KickOutInfo(command.IsKickedOut, command.KickOutReason);
    }
}
