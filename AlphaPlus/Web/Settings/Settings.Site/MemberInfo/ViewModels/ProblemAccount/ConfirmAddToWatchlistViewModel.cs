namespace Alpha.MemberInfo.Site.ViewModels.ProblemAccount
{
    using AlphaPlus.Settings.MemberInfo.Domain.Features.MyWatchlist.MyWatchlistAccounts.Models;
    using System.Collections.Generic;
    using System.Linq;

    public class ConfirmAddToWatchlistViewModel
    {
        private const int limitMember = 50;
        private const int showLimit = 10;

        public ConfirmAddToWatchlistViewModel(MultipleWatchlistAccountModel multipleWatchlistAccountModel)
        {
            var alreadyAddedAccounts = multipleWatchlistAccountModel.AlreadyAddedAccounts;
            var newlyAddedAccounts = multipleWatchlistAccountModel.NewlyAddedAccounts;

            if (alreadyAddedAccounts.Any())
            {
                HasAlreadyAddedAccounts = true;
                NoOfAlreadyAddedAccounts = alreadyAddedAccounts.Count();
                IsShowAllAlreadyAddedAccounts = NoOfAlreadyAddedAccounts > showLimit;

                if (IsShowAllAlreadyAddedAccounts)
                {
                    MoreAlreadyAddedUsernames = alreadyAddedAccounts.Skip(showLimit).Select(c => c.Username);
                    ShortenAlreadyAddedUsernames = alreadyAddedAccounts.Take(showLimit).Select(c => c.Username);
                }

                AlreadyAddedAccounts = string.Join(", ", alreadyAddedAccounts.Select(c => c.Username));

            }
            if (newlyAddedAccounts.Any())
            {
                HasNewlyAddedAccounts = true;
                NoOfNewlyAddedAccounts = newlyAddedAccounts.Count();
                if (NoOfNewlyAddedAccounts > limitMember)
                {
                    IsExceedLimit = true;
                    newlyAddedAccounts = newlyAddedAccounts.Take(limitMember);
                    NoOfNewlyAddedAccounts = limitMember;
                }

                IsShowAllNewlyAddedAccounts = NoOfNewlyAddedAccounts > showLimit;
                if (IsShowAllNewlyAddedAccounts)
                {
                    MoreNewlyAddedUsernames = newlyAddedAccounts.Skip(showLimit).Select(c => c.Username);
                    ShortenNewlyAddedUsernames = newlyAddedAccounts.Take(showLimit).Select(c => c.Username);
                }

                NewlyAddedAccounts = string.Join(", ", newlyAddedAccounts.Select(c => c.Username));
            }
        }

        public string AlreadyAddedAccounts { get; }

        public int NoOfAlreadyAddedAccounts { get; }

        public string NewlyAddedAccounts { get; }

        public int NoOfNewlyAddedAccounts { get; }

        public bool HasAlreadyAddedAccounts { get; }

        public bool HasNewlyAddedAccounts { get; }

        public bool IsExceedLimit { get; }

        public bool IsShowAllAlreadyAddedAccounts { get; }

        public bool IsShowAllNewlyAddedAccounts { get; }

        public IEnumerable<string> ShortenNewlyAddedUsernames { get; }

        public IEnumerable<string> ShortenAlreadyAddedUsernames { get; }

        public IEnumerable<string> MoreNewlyAddedUsernames { get; }

        public IEnumerable<string> MoreAlreadyAddedUsernames { get; }

    }
}
