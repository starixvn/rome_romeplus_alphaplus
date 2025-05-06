namespace AlphaPlus.CashService.Site.Features.Payments.Models
{
    public class DepositMatchingModeModel
    {
        public string SourceType { get; set; }
        public IEnumerable<int> MappingTypes { get; set; }
    }
}