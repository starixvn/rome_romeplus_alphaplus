using AlphaPlus.Identity.API.Utils;
using Fanex.Data.Repository;

namespace AlphaPlus.Identity.API.Features.Auth
{
    public interface IAuthenticateService
    {
        Task<AuthenticateResponse> LoginClaimDownline(AuthenticateRequest request);
    }

    public class AuthenticateService(IEncryption encryption, IDynamicRepository dynamicRepository) : IAuthenticateService
    {
        private readonly IEncryption encryption = encryption;
        private readonly IDynamicRepository dynamicRepository = dynamicRepository;

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

            return new AuthenticateResponse(request.CorrelationId())
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

            };

        }
    }
}