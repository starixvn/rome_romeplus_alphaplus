namespace Alpha.Main.Criteria.SecurityCode
{
    public class UpdateSubAccountSecurityCodeCounterCriteria : UpdateSecurityCodeCriteria
    {
        public int SubaccId { get; set; }

        public bool CheckedSecurityCode { get; set; }

        public override string GetSettingKey() => "Age_UpdateSubAccountSecurityCodeCounter";
    }
}
