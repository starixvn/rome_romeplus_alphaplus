namespace AlphaPlus.Settings.Site.ViewModels.ProblemAccount
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AlphaPlus.Settings.Site.Requests.ProblemAccount;
    using AlphaPlus.Settings.MemberInfo.Domain.AppResources;
    using AlphaPlus.Settings.MemberInfo.Domain.Enumerations;
    using AlphaPlus.Settings.MemberInfo.Domain.Models;

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