using AlphaPlus.Settings.Site.Features.CreateAccount.ViewModels;
using AlphaPlus.Core;
using AlphaPlus.Settings.MemberInfo.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace AlphaPlus.Settings.Site.Features.CreateAccount
{
    public class IntroductionController : BaseController
    {
        private readonly IUser viewer;
        private const string QuickIntroductionPath = "~/MemberInfo/Features/CreateAccount/Views/QuickIntroduction.cshtml";

        public IntroductionController(IUser viewer)
        {
            this.viewer = viewer;
        }
     
        public ActionResult QuickIntroduction()
            => View(QuickIntroductionPath, new QuickIntroductionViewModel(viewer.Name, viewer.Language));
    }
}