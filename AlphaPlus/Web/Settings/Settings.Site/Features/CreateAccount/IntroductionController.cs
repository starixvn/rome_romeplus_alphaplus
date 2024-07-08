using AlphaPlus.Settings.Site.Features.CreateAccount.ViewModels;
using AlphaPlus.Core;
using AlphaPlus.Settings.MemberInfo.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace AlphaPlus.Settings.Site.Features.CreateAccount
{
    public class IntroductionController(IUser viewer) : BaseController
    {
        private readonly IUser viewer = viewer;

        public ActionResult QuickIntroduction()
            => View(new QuickIntroductionViewModel(viewer.Name, viewer.Language));
    }
}