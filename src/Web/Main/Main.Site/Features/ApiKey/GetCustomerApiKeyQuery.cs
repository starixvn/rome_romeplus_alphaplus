using AlphaPlus.Core.Models;
using MediatR;

namespace AlphaPlus.Main.Site.Features.ApiKey
{
    public class GetCustomerApiKeyQuery : IRequest<ApiKeyManagementViewModel>
    {
        public GetCustomerApiKeyQuery() : this(null)
        {
        }

        public GetCustomerApiKeyQuery(IUserSession userSession)
        {
            userSession ??= new UserSessionExt();
            CustId = userSession.CustId;
            CustLevel = Enumeration.FromValue<UserLevel>(userSession.RoleId);
            Language = userSession.Language;
        }

        public int CustId { get; }

        public UserLevel CustLevel { get; }

        public string Language { get; }
    }
}