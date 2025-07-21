using MediatR;
using Fanex.Customer.Client.Services;
using Fanex.Customer.Client.Models;
using AlphaPlus.Core.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using AlphaPlus.Shared.AppResources.Common;

namespace AlphaPlus.Main.Site.Features.Dashboard.CashService;

public class UpdateCashServiceStatusCommand : IRequest<IActionResult>
{
    public const byte SERVICE_TYPE_ENABLED = 1;
    public const byte SERVICE_TYPE_DISABLED = 0;

    public UpdateCashServiceStatusCommand(IUserSession userSession, bool isEnableCash)
    {
        IsEnableCash = isEnableCash;
        UserSession = userSession;
    }
    public IUserSession UserSession { get; }
    public bool IsEnableCash { get; }
}

public class GetCashServiceStatusQuery(IUserSession userSession) : IRequest<CashServiceInfoViewModel>
{
    public IUserSession UserSession { get; } = userSession;
}

public class CashServiceStatusHandler :
    IRequestHandler<UpdateCashServiceStatusCommand, IActionResult>,
    IRequestHandler<GetCashServiceStatusQuery, CashServiceInfoViewModel>
{
    private readonly ICustomerService _customerService;
    private readonly IUserSession _userSession;
    private readonly ILogger<CashServiceStatusHandler> _logger;


    public CashServiceStatusHandler(
        ICustomerService customerService,
        IUserSession userSession,
        ILogger<CashServiceStatusHandler> logger)
    {
        _customerService = customerService ?? throw new ArgumentNullException(nameof(customerService));
        _userSession = userSession ?? throw new ArgumentNullException(nameof(userSession));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    }
    private string CDNUri => AlphaPlus.Core.Configurations.ConfigurationManager.GetValue<string>("Fanex.Customer:CdnUri");

    public async Task<IActionResult> Handle(UpdateCashServiceStatusCommand request, CancellationToken cancellationToken)
    {
        var isOfferedCashService = AlphaPlus.Core.Configurations.AppSettings.IsOfferedCashService(request.UserSession.Site) && request.UserSession.IsAgent;

        if (request.UserSession.IsInternal || !isOfferedCashService || request.UserSession.IsSubAcc)
        {
            return new JsonResult(AlphaPlus.Core.Models.Result.CreateErrorResult(CommonResx.NotAuthorized));
        }

        var status = request.IsEnableCash
            ? UpdateCashServiceStatusCommand.SERVICE_TYPE_ENABLED
            : UpdateCashServiceStatusCommand.SERVICE_TYPE_DISABLED;

        var updateRequest = new UpdateCashServiceStatusRequest
        {
            CustId = request.UserSession.CustId,
            ServiceType = status,
            Language = request.UserSession.Language,
            IP = request.UserSession.UserLoginIp,
            Site = request.UserSession.Site
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
            _logger.LogWarning("Failed to update cash service status for customer {NickName}", request.UserSession.NickName);
        }

        return new JsonResult(result, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

    }

    public async Task<CashServiceInfoViewModel> Handle(GetCashServiceStatusQuery request, CancellationToken cancellationToken)
    {
        var isSiteOfferedCashService = AlphaPlus.Core.Configurations.AppSettings.IsOfferedCashService(request.UserSession.Site);
        var isOfferedCashService = isSiteOfferedCashService && request.UserSession.IsAgent;
       
        if (!isOfferedCashService)
        {
            return new CashServiceInfoViewModel();
        }

        var isCanUpdateCashServiceStatus = isOfferedCashService && !request.UserSession.IsInternal && !request.UserSession.IsSubAcc;
        var isCashServiceEnabled = await _customerService.GetCashServiceStatus(request.UserSession.CustId);
        var totalCashMember = await _customerService.GetCashServiceTotalDownline(request.UserSession.CustId);
        
        var isCanEditCashServicePermission = isOfferedCashService && !request.UserSession.IsInternal &&
            (!request.UserSession.IsSubAcc || request.UserSession.SubPageAccess.CashService);

        return new CashServiceInfoViewModel
        {
            IsCashServiceEnabled = isCashServiceEnabled == 1,
            TotalCashMember = totalCashMember ?? new TotalCashMember(),
            IsOfferedCashService = true,
            IsCanUpdateCashServiceStatus = isCanUpdateCashServiceStatus,
            CDNUri = CDNUri,
            NickName = request.UserSession.NickName,
            IsCanEditCashServicePermission = isCanEditCashServicePermission
        };
    }
}