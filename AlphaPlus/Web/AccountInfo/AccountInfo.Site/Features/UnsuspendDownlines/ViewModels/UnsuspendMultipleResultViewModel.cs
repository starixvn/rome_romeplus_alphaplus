using AlphaPlus.AccountInfo.Site.Features.UnsuspendDownlines.AppResources;
using AlphaPlus.Core.Models;

namespace AlphaPlus.AccountInfo.Site.Features.UnsuspendDownlines.ViewModels
{
    public class UnsuspendResult
    {
        public UnsuspendResult(string username, bool suspended, Result result)
        {
            Username = username;
            Suspended = suspended;
            Result = Result.CreateErrorResult($"{(!string.IsNullOrEmpty(username) ? username + ": " : "")}{result.Message}", result.Code);
        }

        public string Username { get; private set; }

        public bool Suspended { get; private set; }

        public Result Result { get; private set; }
    }

    public class UnsuspendMultipleResultViewModel
    {
        private readonly IEnumerable<UnsuspendResult> unsuspendResults;

        public UnsuspendMultipleResultViewModel(int roleId, IEnumerable<UnsuspendResult> unsuspendResults)
        {
            this.unsuspendResults = unsuspendResults;

            RoleId = roleId;
        }

        public int RoleId { get; }

        public IEnumerable<Result> Results
        {
            get
            {
                var failedResults = new List<Result>();
                var successfullySusependUsernames = new List<string>();
                var successfullyUnSusependUsernames = new List<string>();

                unsuspendResults.ToList().ForEach(unsuspendResult =>
                {
                    if (unsuspendResult.Result.IsSuccessful)
                    {
                        if (unsuspendResult.Suspended)
                        {
                            successfullySusependUsernames.Add(unsuspendResult.Username);
                        }
                        else
                        {
                            successfullyUnSusependUsernames.Add(unsuspendResult.Username);
                        }
                    }
                    else
                    {
                        failedResults.Add(unsuspendResult.Result);
                    }
                });

                var successfullyResults = new List<Result>();
                if (successfullyUnSusependUsernames.Count > 0)
                {
                    successfullyResults.Add(BuildSussesfullyMessage(
                        RoleId > UserLevel.Member.Value
                        ? UnsuspendDownlinesResources.UnsuspendedSuccessfully
                        : UnsuspendDownlinesResources.UnsuspendedMemberSuccessfully,
                        successfullyUnSusependUsernames));
                }

                if (successfullySusependUsernames.Count > 0)
                {
                    successfullyResults.Add(BuildSussesfullyMessage(
                        RoleId > UserLevel.Member.Value
                        ? UnsuspendDownlinesResources.SuspendedSuccessfully
                        : UnsuspendDownlinesResources.SuspendedMemberSuccessfully,
                        successfullySusependUsernames));
                }

                return successfullyResults.Concat(failedResults);
            }
        }

        private static Result BuildSussesfullyMessage(string formatedMessages, IEnumerable<string> usernames)
            => Result.CreateSuccessfulResult(string.Format(formatedMessages, string.Join(", ", usernames)));
    }
}