using AlphaPlus.AccountInfo.Site.Features.OpenDownlines.Models;

namespace AlphaPlus.AccountInfo.Site.Features.OpenDownlines.ViewModels
{
    public class OpenMultipleViewModel
    {
        public OpenMultipleViewModel(
            byte searchAccountRoleId,
            IEnumerable<OpenMultipleModel> data)
        {
            SearchAccountRoleId = searchAccountRoleId;
            Data = data;
        }

        public byte SearchAccountRoleId { get; }

        public IEnumerable<OpenMultipleModel> Data { get; }

        public string Resources { get; }
    }
}