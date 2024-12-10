using AlphaPlus.AccountInfo.Site.Features.OpenDownlines.AppResources;
using AlphaPlus.Core.Models;

namespace AlphaPlus.AccountInfo.Site.Features.OpenDownlines.ViewModels
{
    public class OpenResult
    {
        public OpenResult(string username, bool isClosed, Result result)
        {
            Username = username;
            IsClosed = isClosed;
            Result = Result.CreateErrorResult($"{username}: {result.Message}", result.Code);
        }

        public string Username { get; }

        public bool IsClosed { get; }

        public Result Result { get; }
    }

    public class OpenMultipleResultViewModel
    {
        private readonly IEnumerable<OpenResult> openResults;

        public OpenMultipleResultViewModel(int roleId, IEnumerable<OpenResult> openResults)
        {
            this.openResults = openResults;

            RoleId = roleId;
        }

        public int RoleId { get; }

        public IEnumerable<Result> Results
        {
            get
            {
                var failedResults = new List<Result>();
                var successfullyCloseUsernames = new List<string>();
                var successfullyOpenUsernames = new List<string>();

                openResults.ToList().ForEach(openResult =>
                {
                    if (openResult.Result.IsSuccessful)
                    {
                        if (openResult.IsClosed)
                        {
                            successfullyCloseUsernames.Add(openResult.Username);
                        }
                        else
                        {
                            successfullyOpenUsernames.Add(openResult.Username);
                        }
                    }
                    else
                    {
                        failedResults.Add(openResult.Result);
                    }
                });

                var successfullyResults = new List<Result>();
                if (successfullyOpenUsernames.Count > 0)
                {
                    successfullyResults.Add(BuildSussesfullyMessage(
                        RoleId > UserLevel.Member.Value
                        ? OpenDownlinesResources.OpenedSuccessfully
                        : OpenDownlinesResources.OpenedMemberSuccessfully,
                        successfullyOpenUsernames));
                }

                if (successfullyCloseUsernames.Count > 0)
                {
                    successfullyResults.Add(BuildSussesfullyMessage(
                        RoleId > UserLevel.Member.Value
                        ? OpenDownlinesResources.ClosedSuccessfully
                        : OpenDownlinesResources.ClosedMemberSuccessfully,
                        successfullyCloseUsernames));
                }

                return successfullyResults.Concat(failedResults);
            }
        }

        private static Result BuildSussesfullyMessage(string formatedMessages, IEnumerable<string> usernames)
            => Result.CreateSuccessfulResult(string.Format(formatedMessages, string.Join(", ", usernames)));
    }
}