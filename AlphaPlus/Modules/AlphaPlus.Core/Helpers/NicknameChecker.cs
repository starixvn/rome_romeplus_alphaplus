using AlphaPlus.Core.Models;

namespace AlphaPlus.Core.Helpers;

public interface INicknameChecker
{
    bool IsOfferForceLoginFlow { get; }

    bool IsForceLoginByNickname { get; }
}

public class NicknameChecker(IUserSession userSession) : INicknameChecker
{
    private readonly IUserSession userSession = userSession;

    public bool IsOfferForceLoginFlow => !userSession.IsInternal && string.IsNullOrWhiteSpace(userSession.NickName);

    public bool IsForceLoginByNickname => IsOfferForceLoginFlow;
}
