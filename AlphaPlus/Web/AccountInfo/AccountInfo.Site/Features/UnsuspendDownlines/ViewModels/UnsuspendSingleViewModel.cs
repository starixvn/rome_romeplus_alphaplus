using AlphaPlus.AccountInfo.Site.Features.UnsuspendDownlines.Models;

namespace AlphaPlus.AccountInfo.Site.Features.UnsuspendDownlines.ViewModels
{
    public class UnsuspendSingleViewModel
    {
        public UnsuspendSingleViewModel(
            UnsuspendSingleDataViewModel data)
        {
            Data = data;
        }

        public UnsuspendSingleDataViewModel Data { get; private set; }
    }

    public class UnsuspendSingleDataViewModel
    {
        public UnsuspendSingleDataViewModel(
            int searchAccountId,
            string searchAccountName,
            IEnumerable<UnsuspendSingleModel> levels)
        {
            SearchAccountId = searchAccountId;
            SearchAccountName = searchAccountName;
            Levels = levels.OrderByDescending(level => level.RoleId);
        }

        public int SearchAccountId { get; private set; }

        public string SearchAccountName { get; private set; }

        public IEnumerable<UnsuspendSingleModel> Levels { get; private set; }

        public int AffectedDownlinesTotal => Levels.Sum(item => item.AffectedDownlines);

        public int UnchangedDownlinesTotal => Levels.Sum(item => item.UnchangedDownlines);

    }
}