using System.Data;
using AlphaPlus.Identity.API.Constants;
using AlphaPlus.Identity.API.Utils;
using Fanex.Data;
using Fanex.Data.Repository;
using Microsoft.Extensions.Options;

namespace AlphaPlus.Identity.API.Features.Auth
{
    public interface IAuthenticateService
    {
        Task<AuthenticateResponse> LoginClaimDownline(AuthenticateRequest request);

        Task<int> CheckIn(CheckInRequest request);

        Task CheckOut(CheckOutRequest request);

        Task<ReCheckInResponse> ReCheckIn(int onlineUserId);
    }

    public class AuthenticateService(
        IEncryption encryption,
        IDynamicRepository dynamicRepository,
        IOptions<AppSettings> appSettings) : IAuthenticateService
    {
        private readonly IEncryption encryption = encryption;
        private readonly IDynamicRepository dynamicRepository = dynamicRepository;
        private readonly AppSettings appSettings = appSettings.Value;

        public async Task<ReCheckInResponse> ReCheckIn(int onlineUserId)
        {
            var command = new ReCheckInCommand(onlineUserId);
            await dynamicRepository.ExecuteAsync(command);
            return new ReCheckInResponse
            {
                IsKickedOut = command.IsKickedOut,
                KickOutReason = command.KickOutReason
            };
        }

        public Task CheckOut(CheckOutRequest request)
            => dynamicRepository.ExecuteAsync(new CheckOutCommand(
                request.OnlineUserId,
                request.Username,
                request.RoleId,
                request.ServerId,
                request.Nickname,
                request.Reason));

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
                response.Status = await GetCustomerStatus(response.CustId, response.AdminId);
                response.IsShowConsolidatedReport = await IsShowConsolidateReport(response.CustId);
                response.IsShowBetChoiceLimit = await IsShowBetChoiceLimit(response.CustId);
                response.SiteDB = await GetSiteDB(response.CustId);
                response.IsAllowTotalBalanceTransfer = await IsAllowTotalBalanceTransfer(response.CustId);
                response.DateLimit = await GetDateLimit(response.CustId);

                var securityCodeFunction = await GetSecurityCodeOptionalFunction(response.CustId, response.SubAccId);
                response.IsEnabledSecurityCodeInBetLists = securityCodeFunction.BetList;
                response.IsEnabledSecurityCodeInStatement = securityCodeFunction.Statement;

                // Auto Follow Bet
                var masterId = response.Status.TryGetValue("mrecommend", out string value) ? Convert.ToInt32(value) : 0;
                response.AutoFollowBet = response.RoleId == RoleIdConstants.Agent
                    && appSettings.AutoFollowBetMasterId == masterId
                    && (await IsAutoFollowBetDummyAgent(response.CustId));
            }

            return response;

        }

        public async Task<int> CheckIn(CheckInRequest request)
        {
            var command = new CheckInCommand
            {
                CustId = request.CustId,
                SubAccId = request.SubAccId,
                NetSessionId = request.SessionId,
                ServerId = request.ServerId,
                IP = request.IP
            };
            await dynamicRepository.ExecuteAsync(command);
            return command.OnlineUserId;
        }

        private async Task<IDictionary<string, string>> GetCustomerStatus(int custId, int adminId, string system = "NAP")
        {
            var status = new Dictionary<string, string>();
            var criteria = new GetCustomerStatusCriteria
            {
                CustId = custId,
                AdminId = adminId,
                System = system,
                SettingStatus = appSettings.SettingStatus
            };

            using var objectDb = ObjectDbFactory.CreateInstance(criteria.GetSettingKey());
            var dataSet = await objectDb.ExecuteDataSetAsync(criteria);
            if (dataSet.Tables != null && dataSet.Tables.Count > 0)
            {
                var table = dataSet.Tables[0];
                if (table != null && table.Rows.Count > 0)
                {
                    var row = table.Rows[0];
                    foreach (DataColumn column in table.Columns)
                    {
                        status.Add(column.ColumnName.ToLowerInvariant(), row[column].ToString());
                    }
                }
            }

            return status;
        }


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

        private Task<DateTime> GetDateLimit(int custId)
            => dynamicRepository.GetAsync<DateTime>(new GetCustomerDateLimitCriteria(custId));

        private Task<SecurityCodeFunction> GetSecurityCodeOptionalFunction(int custId, int subAccId)
            => dynamicRepository.GetAsync<SecurityCodeFunction>(new GetSecurityCodeFunctionCriteria(custId, subAccId));

        private async Task<bool> IsAutoFollowBetDummyAgent(int custId)
        {
            var dummyIds = await dynamicRepository.FetchAsync<int>(new GetAutoFollowBetDummyAgentCriteria());
            return dummyIds.Any(id => id == custId);
        }
    }
}