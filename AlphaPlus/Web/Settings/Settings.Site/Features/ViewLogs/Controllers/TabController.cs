namespace AlphaPlus.Settings.Site.Features.ViewLogs.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Constants;
    using ViewModels;

    public class TabController : Controller
    {
        public ActionResult Index(string tabName = TabType.Setting)
        {
            return PartialView(new TabViewModel(tabName));
        }
    }
}