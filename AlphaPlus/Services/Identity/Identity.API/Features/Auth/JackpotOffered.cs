namespace AlphaPlus.Identity.API.Features.Auth
{
    public class JackpotOffered(IEnumerable<JackpotOfferedBySite> bySite, IEnumerable<JackpotOfferedByCurrency> byCurrency)
    {
        public IEnumerable<JackpotOfferedBySite> BySite { get; init; } = bySite;

        public IEnumerable<JackpotOfferedByCurrency> ByCurrency { get; init; } = byCurrency;

        public List<int> GetProductBySiteAndCurrency(string site, int currency)
        {
            var productIdBySite = BySite.Where(i => i.Site.Equals(site, StringComparison.CurrentCultureIgnoreCase));
            var productIdByCurrency = ByCurrency
                .Where(i => i.Currency == currency
                        && productIdBySite.Any(s => s.ProductId == i.ProductId))
                .Select(i => i.ProductId);

            return productIdByCurrency.Distinct().ToList();
        }
    }

    public class JackpotOfferedBySite
    {


        public string Site { get; init; }

        public int ProductId { get; init; }
    }

    public class JackpotOfferedByCurrency
    {
        public int Currency { get; init; }

        public int ProductId { get; init; }
    }
}