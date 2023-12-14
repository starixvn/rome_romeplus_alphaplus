using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AlphaPlus.Core;

[Authorize]
public class BaseController : Controller
{
}
