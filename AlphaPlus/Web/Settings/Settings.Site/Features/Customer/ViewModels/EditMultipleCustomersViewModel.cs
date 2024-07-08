using System.Text.Json;
using AlphaPlus.Settings.Site.Features.Customer.Requests;

namespace AlphaPlus.Settings.Site.Features.Customer.ViewModels
{
    public class EditMultipleCustomersViewModel : CustomerBaseViewModel
    {
        public EditMultipleCustomersViewModel(EditMultipleCustomersQuery query, AppConfiguration appConfig, string cndUri)
            : base(appConfig, cndUri)
        {
            Params = JsonSerializer.Serialize(
                new
                {
                    uplineId = query.CustId,
                    uplineRoleId = query.CustLevelId,
                    custIds = query.CustIds,
                    usernames = query.Usernames,
                    roleId = query.RoleId,
                    productGroupId = query.ProductGroupId
                });
        }
    }
}