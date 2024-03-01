namespace AlphaPlus.Settings.MemberInfo.Domain.Features.MyWatchlist.MyWatchlistAccounts.Models
{
    using AlphaPlus.Settings.MemberInfo.Domain.Models;

    public class NamedResult : Result
    {
        public NamedResult()
        {
        }

        public NamedResult(string name, int code, string message, CancellationToken cancellation)
        {
            Name = name;
            Code = code;
            Message = message;
        }

        public string Name { get; set; }
    }
}
