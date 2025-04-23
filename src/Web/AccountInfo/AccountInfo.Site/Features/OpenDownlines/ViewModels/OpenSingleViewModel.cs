using AlphaPlus.AccountInfo.Site.Features.OpenDownlines.Models;

namespace AlphaPlus.AccountInfo.Site.Features.OpenDownlines.ViewModels
{
    public class OpenSingleViewModel
    {
        public OpenSingleViewModel(OpenSingleDataViewModel data)
        {
            Data = data;

        }

        public OpenSingleDataViewModel Data { get; private set; }

        public string Resources { get; private set; }
    }

    public class OpenSingleDataViewModel
    {
        public OpenSingleDataViewModel(
            int searchAccountId,
            string searchAccountName,
            IEnumerable<OpenSingleModel> levels)
        {
            SearchAccountId = searchAccountId;
            SearchAccountName = searchAccountName;
            Levels = levels.OrderByDescending(level => level.RoleId);
        }

        public int SearchAccountId { get; }

        public string SearchAccountName { get; }

        public IEnumerable<OpenSingleModel> Levels { get; }

        public int AffectedDownlinesTotal => Levels.Sum(item => item.AffectedDownlines);

        public int UnchangedDownlinesTotal => Levels.Sum(item => item.UnchangedDownlines);
    }
}