using Microsoft.AspNetCore.Mvc;
using Alpha.Main.Site.ViewModels.ContactUs;
using AlphaPlus.Core;

namespace Alpha.Main.Site.Controllers
{
    public class ContactUsController : BaseController
    {
        public IActionResult Index()
            => View(new ContactUsViewModel(Request.Headers.Host));
    }
}