using Fanex.Data.Repository;

namespace AlphaPlus.Reports.Features.Customer;

public class UsernameSuggestionCriteria(
    int viewerId,
    string username,
    bool isInternal,
    bool directDownline,
    bool isAccInfo) : CriteriaBase
{
    public int UserId { get; } = viewerId;

    public int CustId { get; } = viewerId;

    public string Username { get; } = username;

    public bool IsInternalAgent { get; } = isInternal;

    public bool DirectDownline { get; } = directDownline;

    public bool IsAccInfo { get; } = isAccInfo;

    public override string GetSettingKey() => "Age_CustomerSelUserSearch";

    public override bool IsValid() => CustId > 0 && !string.IsNullOrWhiteSpace(Username);
}