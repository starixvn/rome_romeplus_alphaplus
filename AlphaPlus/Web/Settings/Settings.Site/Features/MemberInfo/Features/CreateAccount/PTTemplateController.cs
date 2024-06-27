using AlphaPlus.Settings.Site.Features.MemberInfo.Features.CreateAccount.Requests;
using AlphaPlus.Settings.Site.Features.MemberInfo.Features.CreateAccount.ViewModels;
using AlphaPlus.Core;
using AlphaPlus.Shared.AppResources.Settings.MemberInfo;
using AlphaPlus.Settings.MemberInfo.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace AlphaPlus.Settings.Site.Features.MemberInfo.Features.CreateAccount
{
    public class PTTemplateController : BaseController
    {
        private const string PTTemplatePath = "~/Features/MemberInfo/Features/CreateAccount/Views/PTTemplate.cshtml";

        private readonly IUser viewer;
        private readonly ICreateAccountService createAccountService;

        public PTTemplateController(IUser viewer, ICreateAccountService createAccountService)
        {
            this.viewer = viewer;
            this.createAccountService = createAccountService;
        }

        public ActionResult Index()
            => View(PTTemplatePath, new PTTemplateViewModel(viewer.LevelId));

        [HttpGet]
        public async Task<ActionResult> GetPTSettings()
            => Content(await createAccountService.GetPTSettings(viewer.Id, viewer.LevelId, viewer.Site, viewer.CurrencyId, viewer.Language));

        [HttpGet]
        public async Task<ActionResult> GetPTTemplates()
            => Content(await createAccountService.GetPTTemplates(viewer.Id));

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([FromBody]CreatePTTemplateRequest request)
        {
            if (viewer.IsInternal)
            {
                return UnAuthorized();
            }

            return Content(await createAccountService.CreatePTTemplate(
                viewer.Id,
                request.Name,
                request.IsDefault,
                request.Settings,
                viewer.SubAccId,
                viewer.Language,
                viewer.IP));
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([FromBody]EditPTTemplateRequest request)
        {
            if (viewer.IsInternal)
            {
                return UnAuthorized();
            }

            return Content(await createAccountService.EditPTTemplate(
                viewer.Id,
                request.Id,
                request.IsDefault,
                request.Settings,
                viewer.SubAccId,
                viewer.Language,
                viewer.IP));
        }

        [HttpPost, ValidateAntiForgeryToken]
        [Route("PTTemplate/Delete/{templateId}")]
        public async Task<ActionResult> Delete(int templateId)
        {
            if (viewer.IsInternal)
            {
                return UnAuthorized();
            }

            return Content(await createAccountService.DeletePTTemplate(viewer.Id, templateId, viewer.SubAccId, viewer.Language, viewer.IP));
        }

        private ActionResult UnAuthorized()
            => Content(JsonConvert.SerializeObject(Result.CreateErrorResult(Lang.NotAuthorized), new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() }));
    }
}