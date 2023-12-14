namespace Alpha.Main.Criteria.SecurityCode
{
    public class UpdateMainAccountSecurityCodeCounterCriteria : UpdateSecurityCodeCriteria
    {
        public bool CheckedSecurityCode { get; set; }

        public override string GetSettingKey() => "Age_UpdateMainAccountSecurityCodeCounter";
    }
}
