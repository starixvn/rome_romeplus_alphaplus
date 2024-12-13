using Microsoft.AspNetCore.Mvc;
using AlphaPlus.Core.Filters;

namespace AlphaPlus.Tickets.Site.Features.Shared;

[AllowProtectedMode]
public class SharedController : Controller
{
        public IActionResult Invalid() => View("~/Features/Shared/Views/Invalid.cshtml");
}
