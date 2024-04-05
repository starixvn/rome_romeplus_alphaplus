namespace AlphaPlus.Features.Messages.Api.ViewModels
{
    public class AnnouncementViewModel
    {
        public AnnouncementViewModel(int custId, string content)
        {
            CustId = custId;
            Content = content;
        }

        public int CustId { get; }

        public string Content { get; }
    }
}