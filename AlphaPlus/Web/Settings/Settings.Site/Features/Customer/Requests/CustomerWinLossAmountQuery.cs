using AlphaPlus.Settings.Site.Features.Shared;

namespace AlphaPlus.Settings.Site.Features.Customer.Requests
{
    public class CustomerWinLossAmountQuery : BaseQuery
    {
        public int SportType { get; set; }

        public int BetType { get; set; }
    }
}