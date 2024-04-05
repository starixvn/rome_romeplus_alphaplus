namespace AlphaPlus.Features.Messages.Client
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using Alpha.Announcements.Models;
    using AlphaPlus.Features.Messages.Client.Helpers;
    using AlphaPlus.Features.Messages.Api.Client.Requests;
    using Configurations;
    using Newtonsoft.Json;
    using RestSharp;

    public interface AnnouncementClient
    {
        IEnumerable<Announcement> GetAnnouncementsHistory(AnnouncementHistoryRequest request);
    }

    public class AnnouncementClientImpl : AnnouncementClient
    {
        private readonly IRestClient restClient;

        public AnnouncementClientImpl() : this(null)
        {
        }

        public AnnouncementClientImpl(IRestClient restClient)
        {
            this.restClient = restClient ?? new RestClient(new Uri(AppSettings.AnnouncementApi))
            {
                Authenticator = new HmacAuthenticator()
            };
        }

        public IEnumerable<Announcement> GetAnnouncementsHistory(AnnouncementHistoryRequest request)
        {
            var restRequest = new RestRequest("AnnouncementApi/GetAnnouncementsHistory", Method.POST);
            restRequest.AddJsonBody(request);

            IRestResponse response = restClient.Execute(restRequest);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<IEnumerable<Announcement>>(
                    response.Content,
                    new SpecificationConverter());
            }

            return Enumerable.Empty<Announcement>();
        }
    }
}