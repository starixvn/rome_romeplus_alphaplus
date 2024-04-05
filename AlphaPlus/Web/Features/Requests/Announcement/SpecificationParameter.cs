namespace AlphaPlus.Features.Site.Messages.Requests.Announcement
{
    using System.Collections.Generic;
    using System.Linq;
    using Alpha.Announcements.Models;
    using AlphaPlus.Features.Messages.Services;
    using AlphaPlus.Core.Models;

    public class SpecificationParameter : IParam
    {
        private readonly IUserSession userSession;
        private readonly ProductService productService;
        private readonly CustomerService customerService;

        public SpecificationParameter(IUserSession userSession, ProductService productService, CustomerService customerService)
        {
            this.userSession = userSession;
            this.productService = productService;
            this.customerService = customerService;
        }

        public int AdminId => userSession.AdminId;

        public int ARecommendId => userSession.ARecommendId;

        public byte CurrencyId => (byte)userSession.CurrencyId;

        public int UserId => userSession.CustId;

        public bool IsInternal => userSession.IsInternal;

        public bool IsSubAcc => userSession.IsSubAcc;

        public string Language => userSession.Language;

        public string LangKey => userSession.LangKey;

        public byte LevelId => userSession.RoleId;

        public string UserName => userSession.UserName;

        public string Site => userSession.Site;

        public int MRecommendId => userSession.MRecommendId;

        public int SRecommendId => userSession.SRecommendId;

        public bool IsSuper => userSession.IsSuper;

        public bool IsMaster => userSession.IsMaster;

        public bool IsAgent => userSession.IsAgent;

        public Dictionary<string, object> DynamicParams { get; }

        public IEnumerable<byte> OfferedProductIds
            => productService.GetOfferedProductsByCustomer(UserId).Select(i => (byte)i.Id);

        public bool EnableYesterdayTransfer
            => customerService.IsAllDayBalanceTransfer(userSession.CustId);
    }
}