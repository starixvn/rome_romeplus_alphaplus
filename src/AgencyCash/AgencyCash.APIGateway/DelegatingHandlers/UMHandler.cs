using System.Net;
using System.Text;
using ONEbook.UM.Services;

namespace AgencyCash.APIGateway.DelegatingHandlers;

public sealed class UMHandler(IMaintenanceService maintenanceService) : DelegatingHandler
{
    private readonly IMaintenanceService maintenanceService = maintenanceService;

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var umInfo = await maintenanceService.GetUnderMaintenanceTimeAsync();

        if (umInfo.IsUnderMaintenanceTime)
        {
            var error = $"The system is currently under maintenance. Maintenance time: {umInfo.From} - {umInfo.To} (GMT+8). Please try again later.";

            return new HttpResponseMessage(HttpStatusCode.ServiceUnavailable)
            {
                Content = new StringContent(error, Encoding.UTF8, "application/json")
            };
        }

        return await base.SendAsync(request, cancellationToken);
    }
}
