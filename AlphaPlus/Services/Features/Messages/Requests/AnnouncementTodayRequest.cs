namespace AlphaPlus.Features.Messages.Api.Client.Requests
{
    public class AnnouncementTodayRequest
    {
        public AnnouncementTodayRequest(string enviroment, string system)
        {
            Environment = enviroment;
            System = system;
        }

        public string Environment { get; set; }

        public string System { get; set; }
    }
}