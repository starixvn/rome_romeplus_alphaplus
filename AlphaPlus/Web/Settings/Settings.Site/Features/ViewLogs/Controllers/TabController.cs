using Microsoft.AspNetCore.Mvc;
using AlphaPlus.Settings.Site.Features.ViewLogs.Constants;
using AlphaPlus.Settings.Site.Features.ViewLogs.ViewModels;

namespace AlphaPlus.Settings.Site.Features.ViewLogs.Controllers
{
    public class TabController : Controller
    {
        public ActionResult Index(string tabName = TabType.Setting)
        {
            return PartialView(new TabViewModel(tabName));
        }
    }
}