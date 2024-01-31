using AlphaPlus.Core.Models;

namespace AlphaPlus.Core.Helpers;

public interface INicknameChecker
{
    bool IsOfferForceLoginFlow { get; }

    bool IsRecommendLoginByNickname { get; }

    bool IsForceLoginByNickname { get; }

}

public class NicknameChecker(IUserSession userSession) : INicknameChecker
{
    private readonly IUserSession userSession = userSession;

    private bool? isRecommendLoginByNickname;
    private bool? isForceLoginByNickname;

    public bool IsOfferForceLoginFlow => !userSession.IsInternal || string.IsNullOrWhiteSpace(userSession.NickName);

    public bool IsRecommendLoginByNickname
    {
        get
        {
            isRecommendLoginByNickname ??= IsOfferForceLoginFlow;

            return isRecommendLoginByNickname.Value;
        }
    }

    public bool IsForceLoginByNickname
    {
        get
        {
            isForceLoginByNickname ??= IsOfferForceLoginFlow;

            return isForceLoginByNickname.Value;
        }
    }
}
