namespace Alpha.MemberInfo.Site.Features.ParlayMaxPayout.Requests
{
    using Alpha.MemberInfo.Site.Requests;
    using AlphaPlus.Settings.MemberInfo.Domain.Models;
    using MediatR;

    public class ParlayMaxPayoutCheckDownlineQuery : MemberInfoBaseQuery, IRequest<Result>
    {
        public ParlayMaxPayoutCheckDownlineQuery(IUser viewer) : base(viewer)
        {
        }

        public string CustIdList { get; set; }

        public string MaxPayoutLimit { get; set; }

        public override bool IsValid()
            => base.IsValid()
            && !string.IsNullOrWhiteSpace(CustIdList)
            && !string.IsNullOrWhiteSpace(MaxPayoutLimit);
    }
}