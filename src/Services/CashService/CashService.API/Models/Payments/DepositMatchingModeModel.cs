namespace AlphaPlus.CashService.API.Models.Payments
{
    public class DepositMatchingModeModel
    {
        public string SourceType { get; set; }
        public IEnumerable<int> MappingType { get; set; }
        public IEnumerable<int> MappingTypes  => MappingType;
    }
} 