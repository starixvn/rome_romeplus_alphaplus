namespace AlphaPlus.Features.Messages.Criteria.Customer
{
    using Fanex.Data.Repository;

    public class GetCustomerInfoCriteria : CriteriaBase
    {
        public GetCustomerInfoCriteria(int custId)
        {
            CustId = custId;
        }

        public int CustId { get; }

        public override string GetSettingKey() => "Age_GetCustomerInfo";

        public override bool IsValid() => CustId > 0;
    }
}