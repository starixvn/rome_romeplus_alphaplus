using System.Net.Http.Json;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using AlphaPlus.CashService.Site.Features.MemberSites.Models;
using AlphaPlus.CashService.Site.Features.MemberSites.Queries;

namespace AlphaPlus.CashService.Site.Features.MemberSites.Handlers;

public class GetMemberSiteManagementHandler 
    : IRequestHandler<GetMemberSiteManagementQuery, MemberSiteManagementModel>
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;
    private readonly ILogger<GetMemberSiteManagementHandler> _logger;

    public GetMemberSiteManagementHandler(
        HttpClient httpClient,
        IConfiguration configuration,
        ILogger<GetMemberSiteManagementHandler> logger)
    {
        _httpClient = httpClient;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task<MemberSiteManagementModel> Handle(
        GetMemberSiteManagementQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            // var baseUrl = "http://localhost:5252/api";
            var baseUrl = _configuration["CashServiceAPIUrl"];
            var response = await _httpClient.GetFromJsonAsync<MemberSiteManagementModel>(
                $"{baseUrl}/member-site-info",
                cancellationToken);

            return response ?? new MemberSiteManagementModel();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching member site management data");
            throw;
        }
    }
} 