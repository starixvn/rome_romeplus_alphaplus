using AlphaPlus.Shared.AppResources.Main;
using AlphaPlus.Core.Models;

namespace AlphaPlus.Main.Site.Shared.Models
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