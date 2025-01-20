using Microsoft.AspNetCore.Mvc;
using AlphaPlus.Core;
using AlphaPlus.Core.Models;

namespace AlphaPlus.Main.Site.Features.ContactUs
{
    public class ContactUsController : BaseController
    {
        private readonly IUserSession userSession;

        public ContactUsController(IUserSession userSession)
        {
            this.userSession = userSession;
        }

        public IActionResult Index()
            => View(new ContactUsViewModel(Request.Headers.Host, userSession));
    }
}