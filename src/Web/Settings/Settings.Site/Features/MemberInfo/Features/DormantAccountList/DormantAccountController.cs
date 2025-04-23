using AlphaPlus.Settings.Site.Features.MemberInfo.Controllers;
using AlphaPlus.Settings.Site.Features.MemberInfo.Features.DormantAccountList.Requests;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AlphaPlus.Settings.Site.Features.MemberInfo.Features.DormantAccountList
{
    public class DormantAccountController : SiteBaseController
    {
        public DormantAccountController(IMediator mediator)
            : base(mediator)
        {
        }

        public async Task<ActionResult> Index(DormantAccountQuery message)
            => await ViewAsync(message, "~/Features/MemberInfo/Features/DormantAccountList/Views/Index.cshtml");

        [HttpGet]
        public async Task<ActionResult> Excel(DormantAccountExcelQuery message)
            => await ExcelViewAsync(message);
    }
}