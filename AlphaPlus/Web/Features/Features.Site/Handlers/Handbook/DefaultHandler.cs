using AlphaPlus.Features.Handbook.API.Constants;
using AlphaPlus.Features.Handbook.API.Helpers;
using AlphaPlus.Features.Handbook.API.Models;
using AlphaPlus.Features.Handbook.API.Services;
using AlphaPlus.Features.Site.Handbook.Handlers;
using AlphaPlus.Features.Site.Handbook.Requests;
using AlphaPlus.Features.Site.Handbook.ViewModels;
using MediatR;

namespace AlphaPlus.Features.Site.Handbook.Handlers
{
    public class DefaultHandler : 
        APIHandler,
        IRequestHandler<HandbookGetBySlugRequest, GetBySlugViewModel>
    {
        public readonly IProfileService profileService;

         public DefaultHandler(IProfileService profileService, Fanex.Logging.ILogger logger) : base(logger)
        {
            this.profileService = profileService;
        }

        public async Task<GetBySlugViewModel> Handle(HandbookGetBySlugRequest request, CancellationToken cancellation)
        {
            var profilePicture = profileService.GetProfilePicture(request.CustId);
            var briefProfile = new BriefProfileViewModel
            {
                IsDefaultAvatar = string.IsNullOrEmpty(profilePicture.ImageBase64String),
                EmbedImage = profilePicture.ImageBase64String
            };

            var langKey = request.LanguageKey.ToLowerInvariant();
            var locale = LanguageConsts.Locales[langKey];

             var url = contentAPIUrl + "/api/categories/latest/all" +
                "/" + LanguageConsts.Locales[langKey] +
                "/" + AppSettingsHelper.SiteLabel +
                "/" + request.RoleId +
                "/" + AppSettingsHelper.ContentAPIEnvironment;
            var categories = Get<IEnumerable<CategoryModel>>(url) ?? new List<CategoryModel>();
            foreach (var c in categories)
            {
                if (c.Articles != null)
                {
                    c.Articles = c.Articles.OrderBy(a => a.Order);
                }
            }

            var viewModel = new GetBySlugViewModel
            {
                LanguageKey = langKey,
                OnUserId = request.OnUserId,
                BriefProfile = briefProfile,
                Categories = categories.OrderBy(c => c.Order),
                Slug = request.Slug,

                UserId = request.UserId,
                CustId = request.CustId,
                SubAccId = request.SubAccId
            };

            return viewModel;
        }
    }
}