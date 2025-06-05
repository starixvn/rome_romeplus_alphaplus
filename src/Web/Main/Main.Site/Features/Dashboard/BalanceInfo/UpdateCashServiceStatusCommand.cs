using MediatR;
using Fanex.Customer.Client.Services;
using Fanex.Customer.Client.Models;
using AlphaPlus.Core.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace AlphaPlus.Main.Site.Features.Dashboard.BalanceInfo;

public class UpdateCashServiceStatusCommand : IRequest<IActionResult>
{
    public const byte SERVICE_TYPE_ENABLED = 1;
    public const byte SERVICE_TYPE_DISABLED = 0;

    public UpdateCashServiceStatusCommand(IUserSession userSession, bool isEnableCash)
    {
        CustId = userSession.CustId;
        IsEnableCash = isEnableCash;
        Language = userSession.Language;
        UserLoginIp = userSession.UserLoginIp;
        SubAccId = userSession.SubAccId;
        Site = userSession.Site;
    }

    public string Site { get; }
    public int SubAccId { get; }
    public string UserLoginIp { get; }
    public string Language { get; }
    public int CustId { get; }
    public bool IsEnableCash { get; }
}

public class UpdateCashServiceStatusCommandHandler : IRequestHandler<UpdateCashServiceStatusCommand, IActionResult>
{
    private readonly ICustomerService _customerService;
    private readonly IUserSession _userSession;
    private readonly ILogger<UpdateCashServiceStatusCommandHandler> _logger;

    public UpdateCashServiceStatusCommandHandler(
        ICustomerService customerService, 
        IUserSession userSession,
        ILogger<UpdateCashServiceStatusCommandHandler> logger)
    {
        _customerService = customerService ?? throw new ArgumentNullException(nameof(customerService));
        _userSession = userSession ?? throw new ArgumentNullException(nameof(userSession));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<IActionResult> Handle(UpdateCashServiceStatusCommand request, CancellationToken cancellationToken)
    {
        try
        {

            var status = request.IsEnableCash ? UpdateCashServiceStatusCommand.SERVICE_TYPE_ENABLED : UpdateCashServiceStatusCommand.SERVICE_TYPE_DISABLED;
            
            var updateRequest = new UpdateCashServiceStatusRequest
            {
                CustId = request.CustId,
                ServiceType = status,
                Language = request.Language,
                IP = request.UserLoginIp,
                SubAccId = request.SubAccId,
                Site = request.Site
            };

            var result = await _customerService.UpdateCashServiceStatus(updateRequest);
            
            if (result.IsSuccessful && status == UpdateCashServiceStatusCommand.SERVICE_TYPE_ENABLED)
            {
                _userSession.IsCashServiceEnabled = true;
            }
            else if (result.IsSuccessful && status == UpdateCashServiceStatusCommand.SERVICE_TYPE_DISABLED)
            {
                _userSession.IsCashServiceEnabled = false;
            }
            else
            {
                _logger.LogWarning("Failed to update cash service status for customer {CustId}", request.CustId);
            }
            
            return new JsonResult(result, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating cash service status for customer {CustId}", request.CustId);
            return new BadRequestObjectResult(new { error = "Failed to update cash service status. Please try again later." });
        }
    }
}
