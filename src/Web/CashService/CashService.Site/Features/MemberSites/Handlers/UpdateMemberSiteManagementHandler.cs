using System.Net.Http.Json;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using AlphaPlus.CashService.Site.Features.MemberSites.Models;
using AlphaPlus.CashService.Site.Features.MemberSites.Commands;

namespace AlphaPlus.CashService.Site.Features.MemberSites.Handlers;

public class UpdateMemberSiteManagementHandler 
    : IRequestHandler<UpdateMemberSiteManagementCommand, MemberSiteManagementModel>
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

    public async Task<MemberSiteManagementModel> Handle(
        UpdateMemberSiteManagementCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var baseUrl = _configuration["CashServiceAPIUrl"];
            var response = await _httpClient.PutAsJsonAsync(
                $"{baseUrl}/api/member-site-info",
                request.Model,
                cancellationToken);

            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<MemberSiteManagementModel>(cancellationToken) 
                ?? new MemberSiteManagementModel();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating member site management data");
            throw;
        }
    }
} 