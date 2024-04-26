namespace AlphaPlus.Features.Messages.API.Models
{
    using System.Collections.Generic;
    using Alpha.Announcements.Models;
    using Messages.API.Models.Constants;

    public class SpecificationParam : IParam
    {
        private readonly Customer.Customer customer;

        public SpecificationParam(Customer.Customer customer, IEnumerable<byte> offeredProductIds)
        {
            this.customer = customer;
            OfferedProductIds = offeredProductIds;
        }

        public int AdminId => 0;

        public int ARecommendId => customer.ARecommendId;

        public byte CurrencyId => customer.CurrencyId;

        public Dictionary<string, object> DynamicParams { get; } = new Dictionary<string, object>();

        public bool EnableYesterdayTransfer => customer.IsAllDayBalTransfer;

        public bool IsAgent => UserLevel.IsAgent(customer.RoleId);

        public bool IsIMBCCustomer => customer.IsIMBC;

        public bool IsInternal => false;

        public bool IsMaster => UserLevel.IsMaster(customer.RoleId);

        public bool IsSubAcc => false;

        public bool IsSuper => UserLevel.IsSuper(customer.RoleId);

        public string LangKey { get; } = string.Empty;

        public string Language { get; } = string.Empty;

        public byte LevelId => customer.RoleId;

        public int MRecommendId => customer.MRecommendId;

        public IEnumerable<byte> OfferedProductIds { get; } = Enumerable.Empty<byte>();

        public string Site => customer.Site;

        public int SRecommendId => customer.SRecommendId;

        public int UserId => customer.CustId;

        public string UserName => customer.UserName;
    }
}