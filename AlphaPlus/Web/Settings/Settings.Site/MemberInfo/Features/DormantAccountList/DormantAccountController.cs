namespace AlphaPlus.Settings.Site.Features.DormantAccountList
{
    using System.Threading.Tasks;
    using AlphaPlus.Settings.Site.Controllers;
    using AlphaPlus.Settings.Site.Features.DormantAccountList.Requests;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;

    public class DormantAccountController : SiteBaseController
    {
        public DormantAccountController(IMediator mediator)
            : base(mediator)
        {
        }

        public async Task<ActionResult> Index(DormantAccountQuery message)
            => await ViewAsync(message, "~/Features/DormantAccountList/Views/Index.cshtml");

        [HttpGet]
        public async Task<ActionResult> Excel(DormantAccountExcelQuery message)
            => await ExcelViewAsync(message);
    }
}