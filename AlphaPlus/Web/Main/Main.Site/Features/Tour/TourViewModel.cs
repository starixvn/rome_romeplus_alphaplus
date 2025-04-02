using AlphaPlus.Core.Models;

namespace AlphaPlus.Main.Site.Features.Tour;

public record TourViewModel(string Language, byte RoleId, string SiteLabelHash)
{
    public UserLevel UserLevel { get; } = Enumeration.FromValue<UserLevel>(RoleId);
}
