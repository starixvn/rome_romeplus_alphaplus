using AlphaPlus.Core.Helpers;
using AlphaPlus.Products;
using AlphaPlus.Settings.Site.Features.CustomerList.Requests;

namespace AlphaPlus.Settings.Site.Features.CustomerList.ViewModels.Status.Base
{
    public class ProductGroupStatusItemViewModel : StatusItemViewModel
    {
        private const int DisplayOrderStep = 10;

        public ProductGroupStatusItemViewModel(ISearchAccountInfoRequest request, int productGroupId)
            : base(request)
        {
            ProductGroupId = productGroupId;
        }

        public int ProductGroupId { get; }

        public override int DisplayOrder => ProductGroupId * DisplayOrderStep;

        public override string Text => Request.GetProductGroupName(ProductGroupId);

        public override string Url => MicroSites.SettingUrl("~/ProductStatus?" + string.Join("&",
                                           $"isGroup=true",
                                           $"productid={ProductGroupId}",
                                           $"custId={SearchAccount.Id}",
                                           $"custName={SearchAccount.Username}",
                                           $"custRoleId={SearchAccount.CustomerLevelValue}",
                                           $"agentId={SearchAccount.AgentId}",
                                           $"uid={Guid.NewGuid()}"));

        public override bool Visible => ProductGroupId != ProductConstants.Sportsbook && Actor.IsDirectUplineOf(SearchAccount);

        public override bool Checked => true;
    }
}