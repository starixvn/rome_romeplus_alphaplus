using AlphaPlus.Core.Models;
using AlphaPlus.Products.Models;

namespace AlphaPlus.Settings.Site.Features.CustomerList.Requests
{
    public interface ISearchAccountInfoRequest : IRequest, IExtendedRequest
    {
        ICustomer SearchAccount { get; }

        bool ActorIsEditingDirectDownline { get; }

        bool ActorIsNotEditingDirectDownline { get; }

        bool IsSubAcc { get; }

        PageAccess SubPageAccess { get; }

        bool SupportAutoTransfer { get; }

        bool AutoTransferAffect { get; }

        string Language { get; }

        bool IsDarkMode { get; }
    }
}