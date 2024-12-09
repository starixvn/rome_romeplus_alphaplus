using AlphaPlus.AccountInfo.Site.Features.UnsuspendDownlines.AppResources;
using AlphaPlus.AccountInfo.Site.Features.UnsuspendDownlines.Requests;
using AlphaPlus.Core.Models;

namespace AlphaPlus.AccountInfo.Site.Features.UnsuspendDownlines.ViewModels
{
    public class SuspendMultipleConfirmViewModel
    {
        public SuspendMultipleConfirmViewModel(SuspendMultipleConfirmRequest message)
        {
            Data = new SuspendMultipleConfirmDataViewModel(message);
        }

        public SuspendMultipleConfirmDataViewModel Data { get; private set; }

    }

    public class SuspendMultipleConfirmDataViewModel
    {
        private const int numberOfDisplayDownlines = 10;

        public SuspendMultipleConfirmDataViewModel(SuspendMultipleConfirmRequest message)
        {
            Ids = message.Ids;
            RoleId = message.RoleId;
            UserNames = message.UserNames;
            selectedUsernames = message.UserNames.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            ShortenUsernames = selectedUsernames.Take(numberOfDisplayDownlines);
            DisplayShowAllAccountOption = selectedUsernames.Count() > numberOfDisplayDownlines;
            MoreDownlineUsernames = selectedUsernames.Skip(numberOfDisplayDownlines);
            UnsuspendList = getUnsuspends();
        }

        private IEnumerable<UnsuspendCustomerInfo> getUnsuspends()
        {
            var idArray = Ids.Split(',');
            var userArray = UserNames.Split(',');
            var result = new List<UnsuspendCustomerInfo>();
            for (var i = 0; i < userArray.Length; i++)
            {
                result.Add(new UnsuspendCustomerInfo()
                {
                    SearchAccountId = int.Parse(idArray[i]),
                    Username = userArray[i],
                    Suspended = true
                });
            }
            return result;
        }

        private IEnumerable<string> selectedUsernames { get; }

        public IEnumerable<string> ShortenUsernames { get; }

        public IEnumerable<UnsuspendCustomerInfo> UnsuspendList { get; set; }

        public bool DisplayShowAllAccountOption { get; }

        public IEnumerable<string> MoreDownlineUsernames { get; }

        public string Ids { get; }

        public string UserNames { get; }

        public byte RoleId { get; }

        public UserLevel Role => Enumeration.FromValue<UserLevel>(RoleId);

        public string TotalSelectedText => $"{selectedUsernames.Count()} {UnsuspendDownlinesResources.SelectedAccounts}";

    }
}