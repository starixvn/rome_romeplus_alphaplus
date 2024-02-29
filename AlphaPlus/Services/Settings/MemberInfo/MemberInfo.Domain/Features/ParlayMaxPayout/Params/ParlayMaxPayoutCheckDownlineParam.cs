namespace AlphaPlus.Settings.MemberInfo.Domain.Features.ParlayMaxPayout.Params
{
    public class ParlayMaxPayoutCheckDownlineParam
    {
        public ParlayMaxPayoutCheckDownlineParam(
            int custId,
            string maxPayoutLimit,
            string language)
        {
            CustId = custId;
            MaxPayoutLimit = maxPayoutLimit;
            Language = language;
        }

        public int CustId { get; }

        public string MaxPayoutLimit { get; }

        public string Language { get; }
    }
}