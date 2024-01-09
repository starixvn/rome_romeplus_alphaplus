using Alpha.Main.AppResources;
using AlphaPlus.Core.Models;

namespace Alpha.Main.Models
{
    public class UnauthorizedResult : Result
    {
        private const int UnauthorizedCode = 401;

        public bool IsNotAuthorized => Code == UnauthorizedCode;

        public static UnauthorizedResult CreateUnauthorizedResult()
            => new UnauthorizedResult
            {
                Code = UnauthorizedCode,
                Message = Security.NotAuthorized
            };
    }
}