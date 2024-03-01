using System.Collections.Generic;
using System.Linq;

namespace Alpha.MemberInfo.Site.Features.CopyAccount.ViewModels
{
    public class CopyAccountResponse
    {
        public IEnumerable<string> Usernames { get; set; } = Enumerable.Empty<string>();

        public decimal Credit { get; set; }

        public int NoOfAccount { get; set; }

        public int NoOfFailedAccount => (NoOfAccount - Usernames.Count());

        public bool Success { get; set; }

        public int Code { get; set; }

        public string Message { get; set; }

        public bool IsSuccessful => Code == 0;
    }
}