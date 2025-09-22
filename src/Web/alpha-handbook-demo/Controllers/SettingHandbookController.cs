using Alpha.Handbook.Demo.Requests;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Alpha.Handbook.Demo.Controllers
{
    public class SettingHandbookController : BaseController
    {
        public SettingHandbookController(IMediator mediator) : base(mediator)
        {
        }

        public ActionResult SportBookInformation(SportBookInformationRequest request)
        {
            var url = "~/setting/Customer/SportBookInformation?tabName=" + request.TabName
                + "&custId=" + request.CustId
                + "&roleId=" + request.RoleId
                + "&site=" + request.SiteLabel
                + "&siteLabel=" + request.SiteLabel
                + "&currency=" + request.Currency
                + "&lng=" + request.Language;

            return Redirect(url);
        }
    }
}