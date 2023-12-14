namespace Alpha.Main.Criteria.SecurityCode
{
    public abstract class UpdateSecurityCodeCriteria : BaseCommand
    {
        public int CustId { get; set; }

        public string Language { get; set; }

        public string IP { get; set; }

        public override bool IsValid() => CustId > 0;
    }
}
