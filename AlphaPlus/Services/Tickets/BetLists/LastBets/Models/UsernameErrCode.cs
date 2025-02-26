using AlphaPlus.Shared.AppResources.Betlist.Services;

namespace AlphaPlus.BetLists.LastBets;

public class UsernameErrCode
{
    public int CustId { get; init; }

    public string Username { get; init; }

    public string Username2 { get; init; }

    public string FirstName { get; init; }

    public string LastName { get; init; }

    public int ErrCode { get; init; }

    public string ErrMsg => ErrCode == 0 ? string.Empty : Resources.NotAMember;
}