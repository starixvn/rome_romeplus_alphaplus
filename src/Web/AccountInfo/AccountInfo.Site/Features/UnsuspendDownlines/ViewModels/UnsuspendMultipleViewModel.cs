using AlphaPlus.AccountInfo.Site.Features.UnsuspendDownlines.Models;

namespace AlphaPlus.AccountInfo.Site.Features.UnsuspendDownlines.ViewModels
{
    public class UnsuspendMultipleViewModel
    {
        public UnsuspendMultipleViewModel(
            byte searchAccountRoleId,
            IEnumerable<UnsuspendMultipleModel> data)
        {
            SearchAccountRoleId = searchAccountRoleId;
            Data = data;
        }

        public byte SearchAccountRoleId { get; private set; }

        public IEnumerable<UnsuspendMultipleModel> Data { get; private set; }
    }
}