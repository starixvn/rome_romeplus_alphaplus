using AlphaPlus.Settings.Site.Features.MemberInfo.Requests;
using AlphaPlus.Settings.MemberInfo.Domain.Models;
using MediatR;
using AlphaPlus.Settings.Site.Features.UserNameSuggestion.Models;

namespace AlphaPlus.Settings.Site.Features.UserNameSuggestion
{
    public class UserNameSuggestionQuery : MemberInfoBaseQuery, IRequest<IEnumerable<UsernameSuggestion>>
    {
        public UserNameSuggestionQuery() : this(null) { }

        public UserNameSuggestionQuery(IUser viewer) : base(viewer)
        {
            DirectDownlines = false;
        }

        public string Query { get; set; }

        public bool DirectDownlines { get; set; }

        public bool IsAccInfo { get; set; } = true;

        public override bool IsValid() =>
            base.IsValid() &&
            !string.IsNullOrEmpty(Query);
    }
}