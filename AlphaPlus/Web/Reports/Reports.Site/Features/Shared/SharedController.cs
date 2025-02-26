using AlphaPlus.Core.Filters;
using Microsoft.AspNetCore.Mvc;

namespace AlphaPlus.Reports.Site.Features.Shared;

[AllowProtectedMode]
public class SharedController : Controller
{
    public IActionResult InvalidReport() => View();
}
