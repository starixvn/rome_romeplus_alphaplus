using AlphaPlus.Core.Models;
using MediatR;

namespace Alpha.Main.Site.Features.Nickname
{
    public class NicknameHandler(
        INicknameService nicknameService,
        IUserSession userSession) : IRequestHandler<CreateNicknameCommand, Result>
    {
        private readonly INicknameService nicknameService = nicknameService;
        private readonly IUserSession userSession = userSession;

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