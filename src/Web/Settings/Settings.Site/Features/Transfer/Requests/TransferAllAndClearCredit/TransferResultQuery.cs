using AlphaPlus.Core.Models;
using AlphaPlus.Settings.Site.Features.Transfer.Models;
using MediatR;
using AlphaPlus.Settings.Transfer.API.Models;
using AlphaPlus.Settings.Site.Features.Transfer.ViewModels.TransferAllAndClearCredit;

namespace AlphaPlus.Settings.Site.Features.Transfer.Requests.TransferAllAndClearCredit
{
    public class TransferResultQuery : TransferBaseQuery, IRequest<TransferResultViewModel>
    {
        public TransferResultQuery() : this(null) { }

        public TransferResultQuery(IUser viewer) : base(viewer)
        {
            viewer = viewer ?? new User(UserSession.Current);
            UserId = viewer.UserId;
            SubAccId = viewer.SubAccId;
            IP = viewer.IP;
            LanguageKey = viewer.LanguageKey;
            IsInternal = viewer.IsInternal;
        }

        public int UserId { get; }

        public int SubAccId { get; }

        public bool IsClearCredit { get; set; }

        public string Username { get; set; }

        public string IP { get; set; }

        public string TransferSteps { get; set; }

        public string ClearCreditSteps { get; set; }

        public string LanguageKey { get; set; }

        public bool IsInternal { get; }
    }
}