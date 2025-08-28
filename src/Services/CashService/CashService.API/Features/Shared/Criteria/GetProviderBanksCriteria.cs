using Fanex.Data.Repository;

namespace AlphaPlus.CashService.API.Features.Shared.Criteria
{
    public class GetProviderBanksCriteria(string site) : CriteriaBase
    {
        public string Site { get; } = site;

        public override string GetSettingKey() => "Age_CashService_ProviderBankList";

        public override bool IsValid() => true;
    }
} 