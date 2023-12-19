using System.Collections;
using Alpha.Main.Criteria.SignIn;
using Alpha.Main.Models.Authorization;
using Alpha.Main.Params.Authorization;
using AlphaPlus.Core.Extensions;
using AlphaPlus.Core.Models;
using AlphaPlus.Core.Utils;
using Fanex.Data.Repository;

namespace Alpha.Main.Services;

public interface IAuthorizationService
{
    Task<bool> AfterSignIn(SignInParam param);
}

public class AuthorizationService(IEncryption encryption, IDynamicRepository dynamicRepository, IUserSessionService userSessionService) : IAuthorizationService
{
    private readonly IEncryption encryption = encryption;
    private readonly IDynamicRepository dynamicRepository = dynamicRepository;
    private readonly IUserSessionService userSessionService = userSessionService;

    public async Task<bool> AfterSignIn(SignInParam param)
    {
        var command = new LoginClaimDownlineCommand
        {
            CustId = param.CustId,
            Password = encryption.CfsEnCode(param.Password),
            Nickname = param.Nickname,
            IsNickname = param.IsNickname,
            SubAccId = param.SubAccId,
            LoginTime = param.LoginTime,
            Language = param.Language,
            LoginIp = param.RequestInfo.LoginIp,
            CountryCode = param.RequestInfo.CountryCode,
            LoginDomain = param.RequestInfo.LoginDomain,
            BrowserName = param.RequestInfo.BrowserName,
            Platform = param.RequestInfo.Platform
        };

        await dynamicRepository.ExecuteAsync(command);

        if (command.ErrCode != 0)
        {
            return false;
        }

        var outputs = new Hashtable
        {
            { "custid", command.CustId },
            { "roleid", command.RoleId },
            { "sitename", GetSiteName(command.RoleId) },
            { "subaccname", param.IsSubAcc ? command.SubUsername: string.Empty },
            { "adminid", 0 },
            { "isshowconsolidated", await IsShowConsolidateReport(command.CustId) },
            { "isShowBetChoiceLimit", await IsShownBetChoiceLimit(command.CustId) },
        };

        // TODO: DuplicationLogin
        // if (!isDisabledDuplicationLogin)
        // {
        //     int serverId = (int)outputs["serverid"];
        //     this.dao.Acc_Checkin(custId, subAccId, userSession.NetSessionId, serverId, requestInfo["loginip"].ToString(), out int onUserId);
        //     outputs.Add("on_userid", onUserId);
        // }

        // this.GetCustomerStatus(custId, adminId, ref outputs, systemLogin);

        // var siteDBConfig = await GetSiteDBConfig(command.CustId);
        // outputs.Add("AGIP", siteDBConfig.AGIP);
        // outputs.Add("SiteDBGroupId", siteDBConfig.SiteDBGroupId);
        UserSession.Current.SetUserSessionValues(outputs, userSessionService.SetUserStatusToSession);

        return true;

    }

    private static string GetSiteName(int roleId)
    {
        var userLevel = Enumeration.FromValue<UserLevel>((byte)roleId);
        return userLevel.DisplayName;
    }

    private Task<bool> IsShowConsolidateReport(int custId)
    {
        return Task.FromResult(true);
        // var command = new CheckIsShowConsolidatedGroupMenuCommand { CustId = custId };
        // await dynamicRepository.ExecuteAsync(command);
        // return command.IsShownMenu;
    }

    private static Task<bool> IsShownBetChoiceLimit(int custId)
    {
        // TODO: IsShownBetChoiceLimit
        return Task.FromResult(false);
    }

    private Task<SiteDBConfig> GetSiteDBConfig(int custId)
        => dynamicRepository.GetAsync<SiteDBConfig>(new GetSiteDBConfigCriteria(custId));
}