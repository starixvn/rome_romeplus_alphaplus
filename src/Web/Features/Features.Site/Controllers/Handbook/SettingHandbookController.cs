
using AlphaPlus.Core.Helpers;
using AlphaPlus.Features.Site.Handbook.Requests;
using AlphaPlus.Features.Site.Messages.Controllers;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AlphaPlus.Features.Site.Handbook.Controllers
{
    public class SettingHandbookController : SiteBaseController
    {
        public SettingHandbookController(IMediator mediator) : base(mediator)
        {
        }

        public ActionResult SportBookInformation(SportBookInformationRequest request)
        {
            var url = MicroSites.SettingUrl() + "Customer/SportBookInformation?tabName=" + request.TabName
                + "&custId=" + request.CustId
                + "&roleId=" + request.RoleId
                + "&site=" + request.Site
                + "&siteLabel=" + request.SiteLabel
                + "&currency=" + request.Currency
                + "&lng=" + request.Language;

            return Redirect(url);
        }
    }
}