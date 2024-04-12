namespace AlphaPlus.Settings.Site.Transfer.Requests.AutoTransfer
{
    using AlphaPlus.Settings.Site.Transfer.ViewModels.AutoTransfer;
    using MediatR;

    public class GetTransferConditionQuery : IRequest<GetTransferConditionViewModel>
    {
        public int UplineId { get; set; }

        public byte ViewerLevel { get; set; }

        public string SearchAccountIds { get; set; }

        public string Usernames { get; set; }

        public bool IsValid() => !string.IsNullOrWhiteSpace(SearchAccountIds);
    }
}