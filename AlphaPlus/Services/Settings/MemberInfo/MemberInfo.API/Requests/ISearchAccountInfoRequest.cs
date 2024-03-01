namespace AlphaPlus.Settings.MemberInfo.API.Requests
{
    using Alpha.Products.Core.Models;
    using AlphaPlus.Core.Models;

    public interface ISearchAccountInfoRequest : IRequest, IExtendedRequest
    {
        ICustomer SearchAccount { get; }

        bool ActorIsEdittingDirectDownline { get; }

        bool ActorIsNotEdittingDirectDownline { get; }

        bool IsSubAcc { get; }

        PageAccess SubPageAccess { get; }

        bool SupportAutoTransfer { get; }

        bool AutoTransferAffect { get; }
    }
}