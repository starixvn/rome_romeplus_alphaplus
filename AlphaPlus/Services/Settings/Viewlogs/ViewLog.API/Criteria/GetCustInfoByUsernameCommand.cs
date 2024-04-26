namespace AlphaPlus.Settings.ViewLogs.API.Criteria
{
    using Fanex.Data.Repository;

    public class GetCustInfoByUsernameCommand : NonQueryCommand
    {
        public string? Username { get; set; }

        public int UplineId { get; set; }

        public int CustId { get; set; }

        public bool IsSubAcc { get; set; }

        public int RoleId { get; set; }

        public override string GetSettingKey()
        {
            return "Age_CustomerSelCheckBelonging_ByUsername";
        }

        public override bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(Username);
        }
    }
}
