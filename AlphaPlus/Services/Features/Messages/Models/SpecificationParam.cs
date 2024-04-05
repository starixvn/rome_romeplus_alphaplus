namespace AlphaPlus.Features.Messages.Api.Models
{
    using System.Collections.Generic;
    using Alpha.Announcements.Models;
    using Messages.Models.Constants;
    using Messages.Models.Customer;

    public class SpecificationParam : IParam
    {
        private readonly Customer customer;

        public SpecificationParam(Customer customer, IEnumerable<byte> offeredProductIds)
        {
            this.customer = customer;
            OfferedProductIds = offeredProductIds;
        }

        public int AdminId => 0;

        public int ARecommendId => customer.ARecommendId;

        public byte CurrencyId => customer.CurrencyId;

        public Dictionary<string, object> DynamicParams { get; }

        public bool EnableYesterdayTransfer => customer.IsAllDayBalTransfer;

        public bool IsAgent => UserLevel.IsAgent(customer.RoleId);

        public bool IsIMBCCustomer => customer.IsIMBC;

        public bool IsInternal => false;

        public bool IsMaster => UserLevel.IsMaster(customer.RoleId);

        public bool IsSubAcc => false;

        public bool IsSuper => UserLevel.IsSuper(customer.RoleId);

        public string LangKey { get; }

        public string Language { get; }

        public byte LevelId => customer.RoleId;

        public int MRecommendId => customer.MRecommendId;

        public IEnumerable<byte> OfferedProductIds { get; }

        public string Site => customer.Site;

        public int SRecommendId => customer.SRecommendId;

        public int UserId => customer.CustId;

        public string UserName => customer.UserName;
    }
}