using Microsoft.AspNetCore.Mvc;
using AlphaPlus.Core;

namespace AlphaPlus.Main.Site.Features.ContactUs
{
    public class ContactUsController : BaseController
    {
        public IActionResult Index()
            => View(new ContactUsViewModel(Request.Headers.Host));
    }
}