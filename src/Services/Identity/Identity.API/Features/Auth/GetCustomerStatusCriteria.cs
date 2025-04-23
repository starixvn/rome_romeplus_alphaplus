using Fanex.Data.Repository;

namespace AlphaPlus.Identity.API.Features.Auth
{
    public class GetCustomerStatusCriteria : CriteriaBase
    {
        public int CustId { get; init; }

        public int AdminId { get; init; }

        public string System { get; init; } = "NAP";

        public byte SettingStatus { get; init; } = 0;

        public override string GetSettingKey() => "Acc_CustomerSelStatus";

        public override bool IsValid() => CustId > 0;
    }
}