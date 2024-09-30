using System.Data;
using AlphaPlus.Identity.API.Constants;
using AlphaPlus.Identity.API.Utils;
using Fanex.Data;
using Fanex.Data.Repository;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace AlphaPlus.Identity.API.Features.Auth
{
    public interface IAuthenticateService
    {
        Task<AuthenticateResponse> LoginClaimDownline(AuthenticateRequest request);

        Task<int> CheckIn(CheckInRequest request);

        Task CheckOut(CheckOutRequest request);

        Task<ReCheckInResponse> ReCheckIn(int onlineUserId);

        Task<InternalLoginResponse> InternalLogin(InternalLoginRequest request);

        Task<IndirectLoginResponse> IndirectLogin(IndirectLoginRequest request);
    }

    public class AuthenticateService(
        IEncryption encryption,
        IDynamicRepository dynamicRepository,
        IOptions<AppSettings> appSettings,
        IMemoryCache memoryCache,
        IAdminPermissionService adminPermissionService) : IAuthenticateService
    {
        private readonly IEncryption encryption = encryption;
        private readonly IDynamicRepository dynamicRepository = dynamicRepository;
        private readonly AppSettings appSettings = appSettings.Value;
        private readonly IMemoryCache memoryCache = memoryCache;
        private readonly IAdminPermissionService adminPermissionService = adminPermissionService;

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
                Message = command.ErrMsg,
                LanguageUsage = command.LanguageUsage,
                LoginIp = command.LoginIp
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

                response.OfferedJackpotProducts = await GetOfferedJackpotProducts(response.Site, response.CurrencyId);

                // Auto Follow Bet
                var masterId = response.Status.TryGetValue("mrecommend", out string value) ? Convert.ToInt32(value) : 0;
                response.AutoFollowBet = response.RoleId == RoleIdConstants.Agent
                    && appSettings.AutoFollowBetMasterId == masterId
                    && (await IsAutoFollowBetDummyAgent(response.CustId));

                response.OfferedProductGroupIds = await GetOfferedProductGroupIds(response.CustId, response.RoleId);
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

        private async Task<IList<int>> GetOfferedJackpotProducts(string site, int currency)
        {
            var key = "AlphaPlus.Identity.API.Features.Auth.AuthenticateService.OfferedJackpot";

            if (!memoryCache.TryGetValue(key, out JackpotOffered products))
            {
                var (bySite, byCurrency) = await dynamicRepository.FetchMultipleAsync<JackpotOfferedBySite, JackpotOfferedByCurrency>(new GetEnabledProductsByGameCriteria(appSettings.IsTest));
                products = new JackpotOffered(bySite, byCurrency);

                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromSeconds(7));

                memoryCache.Set(key, products, cacheEntryOptions);
            }

            return products.GetProductBySiteAndCurrency(site, currency);
        }

        public async Task<InternalLoginResponse> InternalLogin(InternalLoginRequest request)
        {
            InternalLoginCriteria criteria = new()
            {
                BrowserName = request.BrowserName,
                CountryCode = request.CountryCode,
                Language = request.Language,
                LoginDomain = request.LoginDomain,
                LoginIp = request.LoginIp,
                LoginTime = request.LoginTime,
                Platform = request.Platform,
                Username = request.Username,
                AdminId = request.AdminId
            };

            await dynamicRepository.ExecuteAsync(criteria);

            var response = new InternalLoginResponse
            {
                AdminId = criteria.AdminId,
                CustId = criteria.CustId,
                ErrCode = criteria.ErrCode,
                ErrMsg = criteria.ErrMsg,
                On_UserId = criteria.On_UserId,
                RoleId = criteria.RoleId,
                Username = criteria.Username,
                AccUsername = criteria.AccUsername,
                Currency = criteria.Currency,
                CurrencyId = criteria.CurrencyId,
                IsProtectedMode = criteria.IsProtectedMode,
                LanguageUsage = criteria.LanguageUsage,
                ServerId = criteria.ServerId,
                Site = criteria.Site,
                LoginIp = criteria.LoginIp
            };

            if (criteria.ErrCode == 0)
            {
                response.Status = await GetCustomerStatus(response.CustId, response.AdminId);
                response.IsShowConsolidatedReport = await IsShowConsolidateReport(response.CustId);
                response.IsShowBetChoiceLimit = await IsShowBetChoiceLimit(response.CustId);
                response.SiteDB = await GetSiteDB(response.CustId);
                response.IsAllowTotalBalanceTransfer = await IsAllowTotalBalanceTransfer(response.CustId);
                response.DateLimit = await GetDateLimit(response.CustId);

                var securityCodeFunction = await GetSecurityCodeOptionalFunction(response.CustId, 0);
                response.IsEnabledSecurityCodeInBetLists = securityCodeFunction.BetList;
                response.IsEnabledSecurityCodeInStatement = securityCodeFunction.Statement;

                response.OfferedJackpotProducts = await GetOfferedJackpotProducts(response.Site, response.CurrencyId);

                // Auto Follow Bet
                var masterId = response.Status.TryGetValue("mrecommend", out string value) ? Convert.ToInt32(value) : 0;
                response.AutoFollowBet = response.RoleId == RoleIdConstants.Agent
                    && appSettings.AutoFollowBetMasterId == masterId
                    && (await IsAutoFollowBetDummyAgent(response.CustId));

                response.IsAdminCanUpdateSubAccount = await adminPermissionService.HasUpdateSubAccountPermission(response.AdminId);
            }

            return response;
        }

        public async Task<IndirectLoginResponse> IndirectLogin(IndirectLoginRequest request)
        {
            IndirectLoginCriteria criteria = new()
            {
                BrowserName = request.BrowserName,
                CountryCode = request.CountryCode,
                CustId = request.CustId,
                Language = request.Language,
                LoginDomain = request.LoginDomain,
                LoginIp = request.LoginIp,
                LoginTime = request.LoginTime,
                Platform = request.Platform,
                Username = request.Username,
                UplineUsername = request.UplineUsername,
                UplineCustId = request.UplineCustId,
                UplineSubAccId = request.UplineSubAccId
            };

            await dynamicRepository.ExecuteAsync(criteria);

            var response = new IndirectLoginResponse
            {
                CustId = criteria.CustId,
                ErrCode = criteria.ErrCode,
                ErrMsg = criteria.ErrMsg,
                SubAccId = criteria.SubAccId,
                UplineCustId = criteria.UplineCustId,
                UplineSubAccId = criteria.UplineSubAccId,
                UplineUsername = criteria.UplineUsername,
                Username = criteria.Username,
                Currency = criteria.Currency,
                CurrencyId = criteria.CurrencyId,
                IsNickname = criteria.IsNickname,
                IsProtectedMode = criteria.IsProtectedMode,
                LanguageUsage = criteria.LanguageUsage,
                NicknameLoginOnly = criteria.NicknameLoginOnly,
                ServerId = criteria.ServerId,
                Site = criteria.Site,
                SubPageAccess = criteria.SubPageAccess,
                RoleId = criteria.RoleId,
                IsFirstLogin = criteria.IsFirstLogin,
                IsForcedChangeSecurityCode = criteria.IsForcedChangeSecurityCode,
                SecurityCode = criteria.SecurityCode,
                PwdExpiry = criteria.PwdExpiry,
                Nickname = criteria.Nickname,
                LoginIp = criteria.LoginIp
            };

            if (criteria.ErrCode == 0)
            {
                response.Status = await GetCustomerStatus(response.CustId, 0);
                response.IsShowConsolidatedReport = await IsShowConsolidateReport(response.CustId);
                response.IsShowBetChoiceLimit = await IsShowBetChoiceLimit(response.CustId);
                response.SiteDB = await GetSiteDB(response.CustId);
                response.IsAllowTotalBalanceTransfer = await IsAllowTotalBalanceTransfer(response.CustId);
                response.DateLimit = await GetDateLimit(response.CustId);

                var securityCodeFunction = await GetSecurityCodeOptionalFunction(response.CustId, response.SubAccId);
                response.IsEnabledSecurityCodeInBetLists = securityCodeFunction.BetList;
                response.IsEnabledSecurityCodeInStatement = securityCodeFunction.Statement;

                response.OfferedJackpotProducts = await GetOfferedJackpotProducts(response.Site, response.CurrencyId);

                // Auto Follow Bet
                var masterId = response.Status.TryGetValue("mrecommend", out string value) ? Convert.ToInt32(value) : 0;
                response.AutoFollowBet = response.RoleId == RoleIdConstants.Agent
                    && appSettings.AutoFollowBetMasterId == masterId
                    && (await IsAutoFollowBetDummyAgent(response.CustId));

                response.OfferedJackpotProducts = await GetOfferedProductGroupIds(response.CustId, response.RoleId);
            }

            return response;
        }

        private async Task<int[]> GetOfferedProductGroupIds(int custId, int roleId)
        {
            var productSettings = await dynamicRepository.FetchAsync<ProductSetting>(new GetProductSettingCriteria(custId, roleId, appSettings.SettingStatus));
            productSettings = productSettings?.Where(x => x.GroupId != null && !x.IsHidden);

            if (productSettings == null || !productSettings.Any())
            {
                return [];
            }

            return productSettings.Select(x => x.GroupId).Distinct().Select(x => x.Value).ToArray();
        }
    }
}
