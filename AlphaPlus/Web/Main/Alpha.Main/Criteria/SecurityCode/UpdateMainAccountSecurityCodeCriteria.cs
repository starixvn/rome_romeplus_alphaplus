namespace Alpha.Main.Criteria.SecurityCode
{
    using Fanex.Data;

    public class UpdateMainAccountSecurityCodeCriteria : UpdateSecurityCodeCriteria
    {
        public int SubaccId { get; set; }

        public int UserId { get; set; }

        [SpParam(SqlParamName = "securitycode")]
        public string HashedSecurityCode { get; set; }

        public override string GetSettingKey() => "Age_UpdateMainAccountSecurityCode";
    }
}
