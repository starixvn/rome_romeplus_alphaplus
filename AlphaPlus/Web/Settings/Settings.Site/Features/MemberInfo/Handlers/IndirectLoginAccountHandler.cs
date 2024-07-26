using AlphaPlus.Settings.Site.Features.MemberInfo.Helpers;
using AlphaPlus.Settings.Site.Features.MemberInfo.Requests.IndirectLoginAccount;
using AlphaPlus.Settings.Site.Features.MemberInfo.ViewModels.IndirectLoginAccount;
using AlphaPlus.Utilities.IP;
using MediatR;
using AlphaPlus.Settings.MemberInfo.API.Services.IndirectLoginAccount;
using AlphaPlus.Core.Helpers;
using AlphaPlus.Settings.MemberInfo.Domain.Params.IndirectLoginAccount;
using AlphaPlus.Core.Utils;
using AlphaPlus.Core.Models;

namespace AlphaPlus.Settings.Site.Features.MemberInfo.Handlers
{
    public class IndirectLoginAccountHandler :
        IRequestHandler<ManageAccountQuery, ManageAccountViewModel>,
        IRequestHandler<NewAccountQuery, string>,
        IRequestHandler<ValidateAccountQuery, Result>,
        IRequestHandler<NewAccountCommand, Result>,
        IRequestHandler<UpdatePasswordQuery, UpdatePasswordViewModel>,
        IRequestHandler<UpdatePasswordCommand, Result>,
        IRequestHandler<DisconnectCommand, Result>,
        IRequestHandler<RedirectQuery, string>
    {
        private const string HashKeySessionKey = "AlphaPlus.Settings.Site.Features.MemberInfo.Handlers.IndirectLoginAccountHandler.PasswordHash";

        private readonly IndirectLoginAccountService service;
        private readonly ISessionManager sessionManager;
        private readonly IEncryption encryption;
        private readonly IPService ipService;

        public IndirectLoginAccountHandler(
            IndirectLoginAccountService service,
            ISessionManager sessionManager,
            IEncryption encryption,
            IPService ipService)
        {
            this.service = service;
            this.sessionManager = sessionManager;
            this.encryption = encryption;
            this.ipService = ipService;
        }

        public async Task<ManageAccountViewModel> Handle(ManageAccountQuery message, CancellationToken cancellation)
            => new ManageAccountViewModel(await service.GetIndirectLoginAccountsAsync(message.ViewerId, message.ViewerSubAccId));

        public Task<string> Handle(NewAccountQuery message, CancellationToken cancellation) => Task.FromResult(GetHashKey());

        public async Task<Result> Handle(ValidateAccountQuery message, CancellationToken cancellation)
            => await service.ValidateIndirectLoginAccountAsync(message.ViewerId, message.ViewerSubAccId, message.CustId, message.SubAccId);

        public async Task<Result> Handle(NewAccountCommand message, CancellationToken cancellation)
        {
            var countryCode = GetCountryCode(message.IP);
            var requestInfo = RequestInfoHelper.GetRequestInfo(AlphaContext.Current.Request, message.IP, message.BrowserSize, countryCode);

            return await service.NewAccountAsync(new NewAccountParam(
                message.CustName,
                message.Password,
                sessionManager.Get<string>(HashKeySessionKey),
                message.ViewerId,
                message.ViewerSubAccId,
                message.ViewerLevelId,
                message.LanguageKey,
                requestInfo));
        }

        public Task<UpdatePasswordViewModel> Handle(UpdatePasswordQuery message, CancellationToken cancellation)
            => Task.FromResult(new UpdatePasswordViewModel(message.CustId, message.SubAccId, message.CustName, GetHashKey()));

        public async Task<Result> Handle(UpdatePasswordCommand message, CancellationToken cancellation)
        {
            var countryCode = GetCountryCode(message.IP);
            var requestInfo = RequestInfoHelper.GetRequestInfo(AlphaContext.Current.Request, message.IP, message.BrowserSize, countryCode);

            return await service.UpdatePasswordAsync(new UpdatePasswordParam(
                message.CustId,
                message.SubAccId,
                message.CustName,
                message.CustLevelId,
                message.Password,
                sessionManager.Get<string>(HashKeySessionKey),
                message.ViewerId,
                message.ViewerSubAccId,
                message.ViewerUsername,
                message.LanguageKey,
                requestInfo));
        }

        public async Task<Result> Handle(DisconnectCommand message, CancellationToken cancellation)
            => await service.DisconnectAsync(new DisconnectAccountParam(
                message.CustId,
                message.SubAccId,
                message.CustName,
                message.ViewerId,
                message.ViewerSubAccId,
                message.ViewerUsername,
                message.LanguageKey,
                message.IP));

        public async Task<string> Handle(RedirectQuery message, CancellationToken cancellation)
            => await service.BuildRedirectURLAsync(new RedirectParam(
                message.CustId,
                message.SubAccId,
                message.Language,
                message.ViewerId,
                message.ViewerSubAccId,
                message.ViewerUsername,
                message.BrowserSize,
                message.IBCPKey,
                message.CustName,
                message.DarkModeName,
                message.AssetAppPath));

        private string GetHashKey()
        {
            var hashKey = encryption.RandomString(4);
            sessionManager.Add(HashKeySessionKey, hashKey);

            return hashKey;
        }

        private string GetCountryCode(string ip)
        {
            var countryCode = string.Empty;
            var ipInformation = ipService.GetIPInformation(ip);
            if (ipInformation != null)
            {
                countryCode = ipInformation.CountryLong == "Reserved" ? null : ipInformation.CountryLong;
            }

            return countryCode;
        }
    }
}