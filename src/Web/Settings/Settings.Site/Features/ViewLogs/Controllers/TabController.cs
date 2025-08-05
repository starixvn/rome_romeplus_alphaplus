using Microsoft.AspNetCore.Mvc;
using AlphaPlus.Settings.Site.Features.ViewLogs.Constants;
using AlphaPlus.Settings.Site.Features.ViewLogs.ViewModels;
using AlphaPlus.Core.Models;

namespace AlphaPlus.Settings.Site.Features.ViewLogs.Controllers
{
    public class TabController : Controller
    {
        private readonly IUserSession _userSession;

        public TabController(IUserSession userSession)
        {
            _userSession = userSession;
        }

        public ActionResult Index(string tabName = TabType.Setting)
        {
            var isOfferedCashService = AlphaPlus.Core.Configurations.AppSettings.IsOfferedCashService(_userSession.Site);
            
            return PartialView(new TabViewModel(tabName, isOfferedCashService));
        }
    }
}