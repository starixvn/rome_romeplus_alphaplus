using AlphaPlus.CashService.Site.Features.MemberSites.Commands;
using AlphaPlus.Core.Models;
using MediatR;

namespace AlphaPlus.CashService.Site.Features.MemberSites.Handlers;

public class UpdateMemberSiteManagementHandler
    : IRequestHandler<UpdateMemberSiteManagementCommand, Result>
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;
    private readonly ILogger<UpdateMemberSiteManagementHandler> _logger;

    public UpdateMemberSiteManagementHandler(
        HttpClient httpClient,
        IConfiguration configuration,
        ILogger<UpdateMemberSiteManagementHandler> logger)
    {
        _httpClient = httpClient;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task<Result> Handle(
        UpdateMemberSiteManagementCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            //var baseUrl = "http://localhost:5252";
            var baseUrl = _configuration["CashServiceAPIUrl"];
            var response = await _httpClient.PutAsJsonAsync(
                $"{baseUrl}/api/member-site-info",
                request.Model,
                cancellationToken);

            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<Result>(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating member site management data");
            throw;
        }
    }
}