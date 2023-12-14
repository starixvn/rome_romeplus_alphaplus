using Microsoft.AspNetCore.Mvc;
using AlphaPlus.Core.Filters;
using AlphaPlus.Core;

namespace Alpha.Main.Site.Controllers
{
    public class PartialUMController : BaseController
    {
        [AllowProtectedMode]
        public IActionResult Index() => View();
    }
}