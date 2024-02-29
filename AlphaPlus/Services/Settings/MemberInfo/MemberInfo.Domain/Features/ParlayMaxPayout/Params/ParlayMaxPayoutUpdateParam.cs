namespace AlphaPlus.Settings.MemberInfo.Domain.Features.ParlayMaxPayout.Params
{
    public class ParlayMaxPayoutUpdateParam
    {
        public ParlayMaxPayoutUpdateParam(
            int custId,
            string maxPayoutLimit,
            int userId,
            int subAccId,
            string language,
            string ip)
        {
            CustId = custId;
            MaxPayoutLimit = maxPayoutLimit;
            UserId = userId;
            SubAccId = subAccId;
            Language = language;
            Ip = ip;
        }

        public int CustId { get; }

        public string MaxPayoutLimit { get; }

        public int UserId { get; }

        public int SubAccId { get; }

        public string Language { get; }

        public string Ip { get; }
    }
}