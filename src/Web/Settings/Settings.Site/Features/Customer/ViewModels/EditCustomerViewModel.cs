using System.Text.Json;
using AlphaPlus.Settings.Site.Features.Customer.Requests;

namespace AlphaPlus.Settings.Site.Features.Customer.ViewModels
{
    public class EditCustomerViewModel : CustomerBaseViewModel
    {
        public EditCustomerViewModel(
            EditCustomerQuery query,
            AppConfiguration appConfig,
            string cndUri,
            int serviceType,
            string assetsVersion)
            : base(appConfig, cndUri, assetsVersion)
        {
            Params = JsonSerializer.Serialize(new
            {
                custId = query.CustId,
                roleId = query.RoleId,
                uplineRoleId = query.ViewerLevelId,
                productGroupId = query.ProductGroupId,
                agentId = query.AgentId,
                serviceType
            });
        }
    }
}