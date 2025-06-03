namespace AlphaPlus.CashService.API.Features.Payments.Models
{
    public class DepositMatchingModeModel
    {
        public string SourceType { get; set; }

        public IEnumerable<int> MappingType { get; set; }
        public IEnumerable<int> MappingTypes  => MappingType;
        public string SourceTypeName { get; set; }
    }
} 