using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AlphaPlus.CashService.Site.Features.MemberSites
{
    [Route("[controller]")]
    public class MemberSitesController : Controller
    {
        private readonly ILogger<MemberSitesController> _logger;

        public MemberSitesController(ILogger<MemberSitesController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return Ok("Member Sites");
        }
    }
}