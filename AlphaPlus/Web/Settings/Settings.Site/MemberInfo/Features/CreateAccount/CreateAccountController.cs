using AlphaPlus.Settings.Site.Features.CreateAccount.Requests;
using AlphaPlus.Settings.Site.Features.CreateAccount.ViewModels;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using Sunplus.Agent.Utilities;
using Fanex.Cryptography.Aes;
using AlphaPlus.Core.Helpers;
using AlphaPlus.Settings.MemberInfo.Domain.Models;
using AlphaPlus.Core;
using Microsoft.AspNetCore.Mvc;
using AlphaPlus.Settings.MemberInfo.Domain.AppResources;
using AlphaPlus.Settings.MemberInfo.Domain.Configurations;

namespace AlphaPlus.Settings.Site.Features.CreateAccount
{
    public class CreateAccountController : BaseController
    {
        private const string HashKeySessionKey = "lpha.MemberInfo.Site.Features.CreateAccount.CreateAccountController.PasswordHash";
        private const string IndexPath = "~/Features/CreateAccount/Views/Index.cshtml";

        private readonly IUser viewer;
        private readonly Encryptor encryptor;
        private readonly ICreateAccountService createAccountService;
        private readonly SessionManager sessionManager;
        private readonly IAesService aesService;

        public CreateAccountController(
            IUser viewer,
            Encryptor encryptor,
            ICreateAccountService createAccountService,
            SessionManager sessionManager,
            IAesService aesService)
        {
            this.viewer = viewer;
            this.encryptor = encryptor;
            this.createAccountService = createAccountService;
            this.sessionManager = sessionManager;
            this.aesService = aesService;
        }

        public async Task<ActionResult> Index()
        {
            var limitNewDownlineResult = await createAccountService.CheckAllLimitedRules(
                   viewer.Id,
                   viewer.LevelId,
                   viewer.Name,
                   viewer.Language);

            return View(IndexPath, new CreateAccountViewModel(limitNewDownlineResult.IsFailed, limitNewDownlineResult.Message, viewer.LevelId, GetHashKey()));
        }

        private string GetHashKey()
        {
            var hashKey = encryptor.RandomString(4);
            sessionManager.Add(HashKeySessionKey, hashKey);

            return hashKey;
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(CreateAccountRequest request)
        {
            if (viewer.IsInternal)
            {
                return UnAuthorized();
            }

            var generalInfo = request.GeneralInfo;
            var rawPassword = aesService.Decrypt(generalInfo.Password, sessionManager.Get<string>(HashKeySessionKey));
            generalInfo.Password = encryptor.CfsEnCode(rawPassword);
            generalInfo.UpLineRoleId = viewer.LevelId;
            generalInfo.Site = viewer.Site;
            generalInfo.SiteLabel = AppSettings.SiteLabel;

            return Content(await createAccountService.CreateAccount(
                viewer.Id,
                viewer.SubAccId,
                viewer.IP,
                viewer.Language,
                request.UIBehavior,
                generalInfo,
                request.ProductGroups));
        }

        public async Task<ActionResult> GetCustomerInfo()
            => Content(await createAccountService.GetCustomerInfo(viewer.Id, viewer.LevelId, viewer.Language));

        private ActionResult UnAuthorized()
           => Content(JsonConvert.SerializeObject(Result.CreateErrorResult(Lang.NotAuthorized), new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() }));
    }
}