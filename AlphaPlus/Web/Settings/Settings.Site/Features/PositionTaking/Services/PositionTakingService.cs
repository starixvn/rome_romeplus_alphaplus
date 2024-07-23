using AlphaPlus.Core.Models;

namespace AlphaPlus.Settings.Site.Features.PositionTaking.Services;

public interface IPositionTakingService
{
    //Task<dynamic> GetPT()
}

public class PositionTakingService (HttpClient httpClient) : IPositionTakingService
{
    private readonly HttpClient httpClient = httpClient;


}