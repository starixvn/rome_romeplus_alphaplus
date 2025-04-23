using AlphaPlus.SecurityCode.Services;
using AlphaPlus.Core.Models;
using MediatR;
using ResultModel = AlphaPlus.Core.Models.Result;
using AlphaPlus.Core.Helpers;
using AlphaPlus.Core.Utils;

namespace AlphaPlus.Main.Site.Features.ProtectionCode
{
    public class ProtectionCodeHandler :
        IRequestHandler<ProtectionCodeQuery, ProtectionCodeViewModel>,
        IRequestHandler<UpdateProtectionCodeCommand, ResultModel>
    {
        private const string HashKeySessionKey = "Alpha.Main.ProtectionCodeHandler.ProtectionCodeHash";
        private readonly ISessionManager sessionManager;
        private readonly IProtectionCodeService protectionCodeService;
        private readonly ISecurityCodeChecker securityCodeChecker;
        private readonly IEncryption encryption;
        private const int Length = 4;

        public ProtectionCodeHandler(
            ISessionManager sessionManager,
            IProtectionCodeService protectionCodeService,
            ISecurityCodeChecker securityCodeChecker,
            IEncryption encryption)
        {
            this.sessionManager = sessionManager;
            this.protectionCodeService = protectionCodeService;
            this.securityCodeChecker = securityCodeChecker;
            this.encryption = encryption;
        }

        public async Task<ProtectionCodeViewModel> Handle(ProtectionCodeQuery message, CancellationToken cancellationToken)
        {
            var protectionCode = await protectionCodeService.GetAsync(message.CustId, message.SubAccId);

            return new ProtectionCodeViewModel(
                message.CustId,
                Enumeration.FromValue<UserLevel>(message.RoleId).DisplayName.ToLowerInvariant(),
                message.Language,
                HashKey,
                !string.IsNullOrWhiteSpace(protectionCode),
                securityCodeChecker.IsEnableSecurityCode());
        }

        public async Task<ResultModel> Handle(UpdateProtectionCodeCommand message, CancellationToken cancellationToken)
            => await protectionCodeService.UpdateAsync(new UpdateProtectionCodeParam(
                message.ProtectionCode,
                HashKey,
                message.CustId,
                message.SubAccId,
                message.LangKey,
                message.IP));

        private string HashKey
        {
            get
            {
                var hashKey = sessionManager.Get<string>(HashKeySessionKey);

                if (string.IsNullOrWhiteSpace(hashKey))
                {
                    hashKey = GenerateHashKey();
                    sessionManager.Add(HashKeySessionKey, hashKey);
                }

                return hashKey;
            }
        }

        private string GenerateHashKey()
            => encryption.RandomString(Length);
    }
}