using AlphaPlus.Core;
using AlphaPlus.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace AlphaPlus.Main.Site.Features.Tour;

public class TourController(IUserSession userSession) : BaseController
{
    private readonly IUserSession userSession = userSession;

    public IActionResult Index()
    {
        // Direct Member Tour
        var actorLevel = Enumeration.FromValue<UserLevel>(userSession.RoleId);

        if ((actorLevel.IsSuperLevel() || actorLevel.IsMasterLevel()) && userSession.SubPageAccess.CreateAccount)
        {
            return View(BuildTourViewModel());
        }

        return new EmptyResult();
    }

    public IActionResult ProfilePicture() => View(BuildTourViewModel());

    public IActionResult Reward() => View(BuildTourViewModel());

    private TourViewModel BuildTourViewModel()
        => new TourViewModel(userSession.Language, userSession.RoleId, Core.Configurations.AppSettings.SiteLabelHash);
}