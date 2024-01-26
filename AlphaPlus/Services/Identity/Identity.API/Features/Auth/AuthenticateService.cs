using AlphaPlus.Identity.API.Utils;
using Fanex.Data.Repository;
using Microsoft.Extensions.Options;

namespace AlphaPlus.Identity.API.Features.Auth
{
    public interface IAuthenticateService
    {
        Task<AuthenticateResponse> LoginClaimDownline(AuthenticateRequest request);
    }

    public class AuthenticateService(
        IEncryption encryption,
        IDynamicRepository dynamicRepository,
        IOptions<AppSettings> appSettings) : IAuthenticateService
    {
        private readonly IEncryption encryption = encryption;
        private readonly IDynamicRepository dynamicRepository = dynamicRepository;
        private readonly AppSettings appSettings = appSettings.Value;

        public async Task<AuthenticateResponse> LoginClaimDownline(AuthenticateRequest request)
        {
            var command = new LoginClaimDownlineCommand
            {
                CustId = request.CustId,
                Password = encryption.CfsEnCode(request.Password),
                Nickname = request.Nickname,
                IsNickname = request.IsNickname,
                SubAccId = request.SubAccId,
                LoginTime = request.LoginTime,
                Language = request.Language,
                LoginIp = request.LoginIp,
                CountryCode = request.CountryCode,
                LoginDomain = request.LoginDomain,
                BrowserName = request.BrowserName,
                Platform = request.Platform,
            };

            await dynamicRepository.ExecuteAsync(command);

            var response = new AuthenticateResponse
            {
                CustId = command.CustId,
                UserName = command.UserName,
                SubAccId = command.SubAccId,
                SubUsername = command.SubUsername,
                SubPageAccess = command.SubPageAccess,
                RoleId = command.RoleId,
                Site = command.Site,
                Currency = command.Currency,
                CurrencyId = command.CurrencyId,
                ServerId = command.ServerId,
                IsFirstLogin = command.IsFirstLogin,
                IsForcedChangeSecurityCode = command.IsForcedChangeSecurityCode,
                SecurityCode = command.SecurityCode,
                PwdExpiry = command.PwdExpiry,
                Nickname = command.Nickname,
                IsNickName = command.IsNickname,
                Code = command.ErrCode,
                Message = command.ErrMsg
            };

            if (command.ErrCode == 0)
            {
                response.OnlineUserId = await CheckIn(response.CustId, response.SubAccId, request.SessionId, response.ServerId, request.LoginIp);
                response.Status = await GetCustomerStatus(response.CustId, response.AdminId);
                response.IsShowConsolidatedReport = await IsShowConsolidateReport(response.CustId);
                response.IsShowBetChoiceLimit = await IsShowBetChoiceLimit(response.CustId);
                response.SiteDB = await GetSiteDB(response.CustId);
                response.IsAllowTotalBalanceTransfer = await IsAllowTotalBalanceTransfer(response.CustId);
            }

            return response;

        }

        private async Task<int> CheckIn(int custId, int subAccId, string sessionId, int serverId, string ip)
        {
            var command = new CheckInCommand
            {
                CustId = custId,
                SubAccId = subAccId,
                NetSessionId = sessionId,
                ServerId = serverId,
                IP = ip
            };
            await dynamicRepository.ExecuteAsync(command);
            return command.OnlineUserId;
        }

        private Task<CustomerStatus> GetCustomerStatus(int custId, int adminId, string system = "NAP")
            => dynamicRepository.GetAsync<CustomerStatus>(new GetCustomerStatusCriteria
            {
                CustId = custId,
                AdminId = adminId,
                System = system,
                SettingStatus = appSettings.SettingStatus
            });

        private async Task<bool> IsShowConsolidateReport(int custId)
        {
            var command = new CheckShowConsolidateReportCommand(custId);
            await dynamicRepository.ExecuteAsync(command);
            return command.IsShownMenu;
        }

        private Task<bool> IsShowBetChoiceLimit(int custId)
            => dynamicRepository.GetAsync<bool>(new CheckShowBetChoiceLimitCriteria(custId));

        private Task<SiteDB> GetSiteDB(int custId)
            => dynamicRepository.GetAsync<SiteDB>(new GetSiteDBConfigCriteria(custId));

        private async Task<bool> IsAllowTotalBalanceTransfer(int custId)
        {
            var command = new CheckAllowTotalBalanceTransferCommand(custId, appSettings.AllowedListSuperAccounts);
            await dynamicRepository.ExecuteAsync(command);
            return command.IsAllowed;
        }
    }
}