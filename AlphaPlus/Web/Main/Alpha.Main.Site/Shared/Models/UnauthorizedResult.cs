using Alpha.Main.Site.AppResources;
using AlphaPlus.Core.Models;

namespace Alpha.Main.Site.Shared.Models
{
    public class UnauthorizedResult : Result
    {
        private const int UnauthorizedCode = 401;

        public bool IsNotAuthorized => Code == UnauthorizedCode;

        public static UnauthorizedResult CreateUnauthorizedResult()
            => new()
            {
                Code = UnauthorizedCode,
                Message = Security.NotAuthorized
            };
    }
}