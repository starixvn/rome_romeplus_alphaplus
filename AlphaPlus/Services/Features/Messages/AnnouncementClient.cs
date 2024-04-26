namespace AlphaPlus.Features.Messages.API.Client
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using Alpha.Announcements.Models;
    using AlphaPlus.Features.Messages.API.Client.Helpers;
    using AlphaPlus.Features.Messages.API.Client.Requests;
    using Configurations;
    using Newtonsoft.Json;
    using RestSharp;

    public interface AnnouncementClient
    {
        Task<IEnumerable<Announcement>> GetAnnouncementsHistory(AnnouncementHistoryRequest request);
    }

    public class AnnouncementClientImpl : AnnouncementClient
    {
        private readonly IRestClient restClient;

        public AnnouncementClientImpl(IRestClient restClient)
        {
            this.restClient = restClient ?? new RestClient(new Uri(AppSettings.AnnouncementApi))
            {
                Authenticator = new HmacAuthenticator()
            };
        }

        public async Task<IEnumerable<Announcement>> GetAnnouncementsHistory(AnnouncementHistoryRequest request)
        {
            var restRequest = new RestRequest("AnnouncementApi/GetAnnouncementsHistory", Method.POST);
            restRequest.AddJsonBody(request);

            IRestResponse response = await restClient.ExecuteTaskAsync(restRequest);

            if (response.StatusCode == HttpStatusCode.OK)
            {
#pragma warning disable CS8603 // Possible null reference return.
                return JsonConvert.DeserializeObject<IEnumerable<Announcement>>(
                    response.Content,
                    new SpecificationConverter());
#pragma warning restore CS8603 // Possible null reference return.
            }

            return Enumerable.Empty<Announcement>();
        }
    }
}