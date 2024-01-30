namespace AlphaPlus.Tickets.Site.Features.Shared;

public class ServiceBase
{
    protected readonly HttpClient client;

    protected ServiceBase(HttpClient client)
        => this.client = client;

    public async Task<T> GetFromJson<T>(string url)
        => await client.GetFromJsonAsync<T>(url).ConfigureAwait(false);
}