using AlphaPlus.Settings.Site.Features.MemberInfo.Features.CreateAccount.Requests;
using AlphaPlus.Settings.Site.Features.MemberInfo.Features.CreateAccount.ViewModels;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using Sunplus.Agent.Utilities;
using Fanex.Cryptography.Aes;
using AlphaPlus.Core.Helpers;
using AlphaPlus.Settings.MemberInfo.Domain.Models;
using AlphaPlus.Core;
using Microsoft.AspNetCore.Mvc;
using AlphaPlus.Shared.AppResources.Settings.MemberInfo;
using AlphaPlus.Settings.MemberInfo.Domain.Configurations;
using Fanex.Customer.Client.Services;
using Fanex.Customer.Client.Constants;
using Fanex.Customer.Client.Params;

namespace AlphaPlus.Settings.Site.Features.MemberInfo.Features.CreateAccount
{
    public class CreateAccountController : BaseController
    {
        private const string HashKeySessionKey = "AlphaPlus.MemberInfo.Site.Features.CreateAccount.CreateAccountController.PasswordHash";
        private const string IndexPath = "~/Features/MemberInfo/Features/CreateAccount/Views/Index.cshtml";

        private readonly IUser viewer;
        private readonly Encryptor encryptor;
        private readonly SessionManager sessionManager;
        private readonly IAesService aesService;
        private readonly ICustomerService customerService;

        public CreateAccountController(
            IUser viewer,
            Encryptor encryptor,
            SessionManager sessionManager,
            IAesService aesService,
            ICustomerService customerService)
        {
            this.viewer = viewer;
            this.encryptor = encryptor;
            this.sessionManager = sessionManager;
            this.aesService = aesService;
            this.customerService = customerService;
        }

        public async Task<ActionResult> Index()
        {
            var limitNewDownlineResult = await customerService.CheckAllLimitedRules(
                   viewer.Id,
                   viewer.LevelId,
                   viewer.Name,
                   viewer.Language);

            if (limitNewDownlineResult.IsFailed)
            {
                return View(IndexPath, new CreateAccountViewModel(
                    viewer.Language,
                    !string.IsNullOrWhiteSpace(viewer.DarkModeName),
                    limitNewDownlineResult.IsFailed,
                    limitNewDownlineResult.Message,
                    viewer.LevelId));
            }

            return View(IndexPath, new CreateAccountViewModel(
                viewer.Language,
                !string.IsNullOrWhiteSpace(viewer.DarkModeName),
                false,
                string.Empty,
                viewer.LevelId,
                GetHashKey(),
                await customerService.GetSuggestUsername(viewer.Name, viewer.LevelId),
                viewer.Id));
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

            return Content(await customerService.CreateCustomer(new CreateCustomerParam(
                generalInfo,
                request.ProductGroups,
                viewer.Id,
                viewer.SubAccId,
                viewer.IP,
                viewer.Site,
                AppSettings.SiteLabel,
                CreationModes.LiteMode,
                viewer.Language,
                request.UIBehavior)));
        }

        public async Task<ActionResult> GetCustomerInfo()
            => Content(await customerService.GetCustomerSetting(viewer.Id, viewer.LevelId, 4, viewer.LevelId, viewer.Language, false, 0));

        private ActionResult UnAuthorized()
           => Content(JsonConvert.SerializeObject(Result.CreateErrorResult(Lang.NotAuthorized), new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() }));
    }
}