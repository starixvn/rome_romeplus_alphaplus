using Fanex.Data.Repository;

namespace AlphaPlus.Reports.Features.Customer;

internal class UpdateUserSearchCriteria(
    int userId,
    string search,
    string firstName,
    string lastName,
    string nickname) : NonQueryCommand
{
    public int UserId { get; } = userId;

    public string Search { get; } = search;

    public string FirstName { get; } = firstName;

    public string LastName { get; } = lastName;

    public string Username2 { get; } = nickname;

    public string System { get; } = "ACCOUNTING";

    public override string GetSettingKey() => "Acc_CustomerUpdUserSearch";

    public override bool IsValid() => UserId > 0 && !string.IsNullOrWhiteSpace(Search);
}