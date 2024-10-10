using AlphaPlus.Core.Models;
using MediatR;

namespace AlphaPlus.Settings.Site.Features.ProductStatus.Requests
{
    public class ProductStatusCommand : ProductStatusQuery, IRequest<IEnumerable<Result>>
    {
        public BetTypeStatus[] BetTypeStatus { get; set; }

        public int ActorId => ViewerId;

        public string ActorName => ViewerUsername;

        public override bool IsValid()
            => base.IsValid() && BetTypeStatus?.Length > 0;
    }
}