using Alpha.Main.Params.ProtectionCode;
using Alpha.Main.Services;
using Alpha.Main.Site.Requests.ProtectionCode;
using Alpha.Main.Site.ViewModels.ProtectionCode;
using AlphaPlus.SecurityCode.Services;
using AlphaPlus.Core.Models;
using MediatR;
using ResultModel = AlphaPlus.Core.Models.Result;
using AlphaPlus.Core.Helpers;

namespace Alpha.Main.Site.Handlers
{
    public class ProtectionCodeHandler :
        IRequestHandler<ProtectionCodeQuery, ProtectionCodeViewModel>,
        IRequestHandler<UpdateProtectionCodeCommand, ResultModel>
    {
        private const string HashKeySessionKey = "Alpha.Main.ProtectionCodeHandler.ProtectionCodeHash";

        private readonly ISessionManager sessionManager;
        private readonly INicknameChecker nicknameChecker;
        private readonly ProtectionCodeService protectionCodeService;
        private readonly ISecurityCodeChecker securityCodeChecker;

        public ProtectionCodeHandler(
            ISessionManager sessionManager,
            INicknameChecker nicknameChecker,
            ProtectionCodeService protectionCodeService,
            ISecurityCodeChecker securityCodeChecker)
        {
            this.sessionManager = sessionManager;
            this.nicknameChecker = nicknameChecker;
            this.protectionCodeService = protectionCodeService;
            this.securityCodeChecker = securityCodeChecker;
        }

        public async Task<ProtectionCodeViewModel> Handle(ProtectionCodeQuery message, CancellationToken cancellationToken)
        {
            var isShowNicknameLink = !nicknameChecker.IsOfferForceLoginAndOverDueDate;
            var protectionCode = await protectionCodeService.GetAsync(message.CustId, message.SubAccId);

            return new ProtectionCodeViewModel(
                message.CustId,
                Enumeration.FromValue<UserLevel>(message.RoleId).DisplayName.ToLowerInvariant(),
                message.Language,
                HashKey,
                isShowNicknameLink,
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
                    hashKey = protectionCodeService.GenerateHashKey();
                    sessionManager.Add(HashKeySessionKey, hashKey);
                }

                return hashKey;
            }
        }
    }
}