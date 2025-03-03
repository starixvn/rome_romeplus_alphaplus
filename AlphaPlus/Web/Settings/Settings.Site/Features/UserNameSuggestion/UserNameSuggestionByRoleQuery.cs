using AlphaPlus.Settings.Site.Features.Shared;
using AlphaPlus.Settings.Site.Features.UserNameSuggestion.Models;
using MediatR;

namespace AlphaPlus.Settings.Site.Features.UserNameSuggestion;

public class UserNameSuggestionByRoleQuery : BaseQuery, IRequest<IEnumerable<UsernameSuggestion>>
{
    public string SuggestionQuery { get; set; }

    public byte RoleId { get; set; }

    public override bool IsValid()
        => !string.IsNullOrWhiteSpace(SuggestionQuery) && SuggestionQuery.Length > 1 && base.IsValid();
}