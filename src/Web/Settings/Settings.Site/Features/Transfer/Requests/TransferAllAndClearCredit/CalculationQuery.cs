using AlphaPlus.Core.Models;
using AlphaPlus.Settings.Site.Features.Transfer.Models;
using MediatR;
using AlphaPlus.Settings.Transfer.API.Models;
using AlphaPlus.Settings.Site.Features.Transfer.ViewModels.TransferAllAndClearCredit;

namespace AlphaPlus.Settings.Site.Features.Transfer.Requests.TransferAllAndClearCredit
{
    public class CalculationQuery : TransferBaseQuery, IRequest<CalculationViewModel>
    {
        public CalculationQuery() : this(null) { }

        public CalculationQuery(IUser viewer) : base(viewer)
        {
            viewer = viewer ?? new User(UserSession.Current);
            IsInternal = viewer.IsInternal;
            LanguageKey = viewer.LanguageKey;
        }

        public string Username { get; set; }

        public bool IsInternal { get; }

        public string LanguageKey { get; }
    }
}
