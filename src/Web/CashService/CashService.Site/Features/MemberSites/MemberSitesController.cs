using Microsoft.AspNetCore.Mvc;
using MediatR;
using Microsoft.Extensions.Logging;
using AlphaPlus.CashService.Site.Features.MemberSites.Models;
using AlphaPlus.CashService.Site.Features.MemberSites.Queries;
using AlphaPlus.CashService.Site.Features.MemberSites.Commands;

namespace AlphaPlus.CashService.Site.Features.MemberSites;

public class MemberSitesController : Controller
{
    private readonly IMediator _mediator;
    private readonly ILogger<MemberSitesController> _logger;

    public MemberSitesController(IMediator mediator, ILogger<MemberSitesController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpGet]   
    public IActionResult Index()
    {
        return View();
    }

    [HttpGet]
    public async Task<ActionResult<MemberSiteManagementModel>> MemberSiteManagement(CancellationToken cancellationToken)
    {
        try
        {
            var result = await _mediator.Send(new GetMemberSiteManagementQuery(), cancellationToken);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting member site management data");
            return StatusCode(500, new { error = "Failed to fetch member site management data" });
        }
    }

    [HttpPut]
    public async Task<ActionResult<MemberSiteManagementModel>> MemberSiteManagement([FromBody] MemberSiteManagementModel model, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var result = await _mediator.Send(
                new UpdateMemberSiteManagementCommand(model),
                cancellationToken);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating member site management data");
            return StatusCode(500, new { error = "Failed to update member site management data" });
        }
    }
}