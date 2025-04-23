using AlphaPlus.Settings.Site.Features.Transfer.ViewModels.AutoTransfer;
using MediatR;

namespace AlphaPlus.Settings.Site.Features.Transfer.Requests.AutoTransfer
{
    public class GetTransferConditionQuery : IRequest<GetTransferConditionViewModel>
    {
        public int UplineId { get; set; }

        public byte ViewerLevel { get; set; }

        public string SearchAccountIds { get; set; }

        public string Usernames { get; set; }

        public bool IsValid() => !string.IsNullOrWhiteSpace(SearchAccountIds);
    }
}