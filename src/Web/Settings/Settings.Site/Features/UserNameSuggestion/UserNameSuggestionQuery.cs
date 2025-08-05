using AlphaPlus.Settings.Site.Features.MemberInfo.Requests;
using AlphaPlus.Settings.Site.Features.UserNameSuggestion.Models;
using MediatR;

namespace AlphaPlus.Settings.Site.Features.UserNameSuggestion;
public class UserNameSuggestionQuery : MemberInfoBaseQuery, IRequest<IEnumerable<UsernameSuggestion>>
{
    public string Query { get; set; }

    public bool DirectDownlines { get; set; }

    public bool IsAccInfo { get; set; } = true;

    public bool ExcludeMember { get; set; }

    public bool ExcludeCashMember { get; set; }

    public override bool IsValid()
        => !string.IsNullOrWhiteSpace(Query) && Query.Length > 1 && base.IsValid();
}