using AlphaPlus.Core;
using AlphaPlus.Core.Helpers;
using AlphaPlus.Core.Models;
using AlphaPlus.Core.Utils;
using AlphaPlus.Settings.MemberInfo.Domain.Configurations;
using AlphaPlus.Settings.MemberInfo.Domain.Models;
using AlphaPlus.Settings.Site.Features.CreateAccount.Requests;
using AlphaPlus.Settings.Site.Features.CreateAccount.ViewModels;
using AlphaPlus.Shared.AppResources.Settings.MemberInfo;
using Fanex.Cryptography.Aes;
using Fanex.Customer.Client.Constants;
using Fanex.Customer.Client.Params;
using Fanex.Customer.Client.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace AlphaPlus.Settings.Site.Features.CreateAccount
{
    public class CreateAccountController : BaseController
    {
        private const string HashKeySessionKey = "AlphaPlus.MemberInfo.Site.Features.CreateAccount.CreateAccountController.PasswordHash";

        private readonly IUser viewer;
        private readonly IEncryption encryption;
        private readonly ISessionManager sessionManager;
        private readonly IAesService aesService;
        private readonly ICustomerService customerService;

        public CreateAccountController(
            IUser viewer,
            IEncryption encryption,
            ISessionManager sessionManager,
            IAesService aesService,
            ICustomerService customerService)
        {
            this.viewer = viewer;
            this.encryption = encryption;
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
                   false,
                   viewer.Language);

            if (limitNewDownlineResult.IsFailed)
            {
                return View(new CreateAccountViewModel(
                    viewer.Language,
                    !string.IsNullOrWhiteSpace(viewer.DarkModeName),
                    limitNewDownlineResult.IsFailed,
                    limitNewDownlineResult.Message,
                    viewer.LevelId));
            }

            return View(new CreateAccountViewModel(
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
            var hashKey = encryption.RandomString(4);
            sessionManager.Add(HashKeySessionKey, hashKey);

            return hashKey;
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> Index([FromBody] CreateAccountRequest request)
        {
            if (viewer.IsInternal)
            {
                return UnAuthorized();
            }

            var generalInfo = request.GeneralInfo;
            var rawPassword = aesService.Decrypt(generalInfo.Password, sessionManager.Get<string>(HashKeySessionKey));
            generalInfo.Password = rawPassword;
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
                viewer.CustDBGroupId,
                request.Group,
                1,
                CreationModes.LiteMode,
                viewer.Language,
                request.UIBehavior)));
        }

        public async Task<ActionResult> GetCustomerInfo()
            => Content(await customerService.GetCustomerSetting(
                viewer.Id,
                viewer.LevelId,
                4,
                viewer.LevelId,
                viewer.Language,
                false,
                0,
                AppSettings.SiteLabel,
                agentId: 0));

        private ActionResult UnAuthorized()
           => Content(JsonConvert.SerializeObject(Result.CreateErrorResult(Lang.NotAuthorized), new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() }));
    }
}