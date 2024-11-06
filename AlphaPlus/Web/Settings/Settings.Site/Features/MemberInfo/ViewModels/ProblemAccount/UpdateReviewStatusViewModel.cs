using AlphaPlus.Core.Models;
using AlphaPlus.Settings.Site.Features.MemberInfo.Requests.ProblemAccount;
using AlphaPlus.Shared.AppResources.Settings.MemberInfo;

namespace AlphaPlus.Settings.Site.Features.MemberInfo.ViewModels.ProblemAccount
{
    public class UpdateReviewStatusViewModel
    {
        private const int numberOfDisplayDownlines = 10;

        public UpdateReviewStatusViewModel(UpdateProblemAccountRequest message)
        {
            RoleId = message.RoleId;
            Ids = message.Ids;
            SelectedDownlineUsernames = message.UserNames.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            DisplayShowAllAccountOption = SelectedDownlineUsernames.Count() > numberOfDisplayDownlines;
            TopDownlineUsernames = SelectedDownlineUsernames.Take(numberOfDisplayDownlines);
            MoreDownlineUsernames = SelectedDownlineUsernames.Skip(numberOfDisplayDownlines);
        }

        public bool DisplayShowAllAccountOption { get; }

        public IEnumerable<string> TopDownlineUsernames { get; }

        public IEnumerable<string> SelectedDownlineUsernames { get; }

        public IEnumerable<string> MoreDownlineUsernames { get; }

        public string Ids { get; }

        public byte RoleId { get; }

        public UserLevel Role => Enumeration.FromValue<UserLevel>(RoleId);

        public string ConfirmMessage => $"{Resources.SelectedAccounts} {string.Join(", ", TopDownlineUsernames)}";

        public int TotalAllAccount => SelectedDownlineUsernames.Count();
    }
}