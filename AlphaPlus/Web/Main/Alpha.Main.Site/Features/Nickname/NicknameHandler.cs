using AlphaPlus.Core.Models;
using MediatR;
using Alpha.Main.Params.Nickname;
using Alpha.Main.Services;

namespace Alpha.Main.Site.Features.Nickname
{
    public class NicknameHandler : IRequestHandler<CreateNicknameCommand, Result>
    {
        private readonly NicknameService nicknameService;
        private readonly IUserSession userSession;

        public NicknameHandler(
            NicknameService nicknameService,
            IUserSession userSession)
        {
            this.nicknameService = nicknameService;
            this.userSession = userSession;
        }

        public async Task<Result> Handle(CreateNicknameCommand message, CancellationToken cancellationToken)
        {
            var isEnableUpdateNickname = !message.IsDisableUpdateNickname;
            var nickname = message.Nickname == null ? string.Empty : message.Nickname.Trim();
            var isNicknameLoginOnly = true;

            var result = await nicknameService.CreateNickname(
                new CreateNicknameParam(
                    message.ActorId,
                    message.SubAccountId,
                    nickname,
                    message.IP,
                    message.LanguageKey,
                    isEnableUpdateNickname),
                isNicknameLoginOnly);

            if (result.IsSuccessful)
            {
                userSession.IsNicknameLoginOnly = isNicknameLoginOnly;

                if (isEnableUpdateNickname)
                {
                    userSession.NickName = nickname.ToUpperInvariant();
                }
            }

            return result;
        }
    }
}