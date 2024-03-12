namespace AlphaPlus.Settings.Site.Features.ParlayMaxPayout.Requests
{
    using AlphaPlus.Settings.Site.Requests;
    using AlphaPlus.Settings.MemberInfo.Domain.Models;
    using MediatR;
    using System.Collections.Generic;
    using AlphaPlus.Settings.Site.Models;
    using AlphaPlus.Core.Models;

    public class ParlayMaxPayoutUpdateBaseQuery : MemberInfoBaseQuery
    {
        public ParlayMaxPayoutUpdateBaseQuery() : this(null) { }

        public ParlayMaxPayoutUpdateBaseQuery(IUser viewer) : base(viewer)
        {
            viewer = viewer ?? new User(UserSession.Current);
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
        ParlayMaxPayoutUpdateBaseQuery, IRequest<Settings.MemberInfo.Domain.Models.Result>
    {
        public ParlayMaxPayoutUpdateSingleQuery() : this(null) { }

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
        ParlayMaxPayoutUpdateBaseQuery, IRequest<IEnumerable<Settings.MemberInfo.Domain.Models.Result>>
    {
        public ParlayMaxPayoutUpdateMultipleQuery() : this(null) { }

        public ParlayMaxPayoutUpdateMultipleQuery(IUser viewer) : base(viewer)
        {
        }
    }
}