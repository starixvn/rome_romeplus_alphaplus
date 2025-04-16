using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Hosting;

namespace AlphaPlus.CashService.Site.Features.MemberSites
{
    public class MemberSitesController : Controller
    {
        private readonly ILogger<MemberSitesController> _logger;
        private readonly IWebHostEnvironment _environment;

        public MemberSitesController(ILogger<MemberSitesController> logger, IWebHostEnvironment environment)
        {
            _logger = logger;
            _environment = environment;
        }

        public IActionResult Index()
        {
            ViewData["IsDevelopment"] = false;
            return View();
        }
    }
}