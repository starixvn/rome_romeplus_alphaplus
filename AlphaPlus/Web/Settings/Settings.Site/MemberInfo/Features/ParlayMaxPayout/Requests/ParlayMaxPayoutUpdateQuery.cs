namespace AlphaPlus.Settings.Site.Features.ParlayMaxPayout.Requests
{
    using AlphaPlus.Settings.Site.Requests;
    using AlphaPlus.Settings.MemberInfo.Domain.Models;
    using MediatR;
    using System.Collections.Generic;

    public class ParlayMaxPayoutUpdateBaseQuery : MemberInfoBaseQuery
    {
        public ParlayMaxPayoutUpdateBaseQuery(IUser viewer) : base(viewer)
        {
            UserId = viewer.Id;
        }

        public string CustIdList { get; set; }

        public string MaxPayoutLimit { get; set; }

        public int UserId { get; }

        public override bool IsValid()
            => base.IsValid()
            && UserId > 0
            && !string.IsNullOrWhiteSpace(CustIdList)
            && !string.IsNullOrWhiteSpace(MaxPayoutLimit)
            && CanUpdateAccount
            && !IsInternal;
    }

    public class ParlayMaxPayoutUpdateSingleQuery :
        ParlayMaxPayoutUpdateBaseQuery, IRequest<Result>
    {
        public ParlayMaxPayoutUpdateSingleQuery(IUser viewer) : base(viewer)
        {
        }

        public int SelectedCustId
        {
            get
            {
                int custId;
                int.TryParse(CustIdList, out custId);
                return custId;
            }
        }
    }

    public class ParlayMaxPayoutUpdateMultipleQuery :
        ParlayMaxPayoutUpdateBaseQuery, IRequest<IEnumerable<Result>>
    {
        public ParlayMaxPayoutUpdateMultipleQuery(IUser viewer) : base(viewer)
        {
        }
    }
}