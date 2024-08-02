using AlphaPlus.AccountInfo.Site.Features.ProfilePicture.Get;
using AlphaPlus.AccountInfo.Site.Features.ProfilePicture.GetLibrary;
using AlphaPlus.Core.Configurations;
using AlphaPlus.SecurityCode.Services;
using Fanex.Caching;
using MediatR;
using JW;
using AlphaPlus.AccountInfo.Site.Features.ProfilePicture.Update;
using AlphaPlus.Core.Models;

namespace AlphaPlus.AccountInfo.Site.Features.ProfilePicture
{
    public class ProfilePictureHandler :
        IRequestHandler<ProfilePictureQuery, ProfilePictureViewModel>,
        IRequestHandler<ProfilePictureLibraryQuery, ProfilePictureLibraryViewModel>,
        IRequestHandler<UpdateActorProfilePictureRequest, Result>
    {
        private const string ProfilePictureLibraryKey = "AlphaPlus.AccountInfo.Site.Features.ProfilePicture.Library";
        private const int ProfilePictureLibraryCacheInMinutes = 5;
        private const int NumerousOfDefaultPictures = 7;
        private static readonly IDictionary<string, string> Languages = new Dictionary<string, string>
        {
            {"en-US", "English"},
            {"zh-TW", "繁體中文"},
            {"zh-CN", "简体中文"},
            {"ja-JP", "日本語"},
            {"th-TH", "ภาษาไทย"},
            {"ko-KR", "한국어"},
            {"vi-VN", "Tiếng Việt"}
        };
        private readonly ISecurityCodeChecker securityCodeChecker;
        private readonly IProfilePictureService profilePictureService;
        private readonly ICacheService cacheService;
        public ProfilePictureHandler(ISecurityCodeChecker securityCodeChecker, IProfilePictureService profilePictureService, ICacheService cacheService)
        {
            this.securityCodeChecker = securityCodeChecker;
            this.profilePictureService = profilePictureService;
            this.cacheService = cacheService;
        }

        public async Task<ProfilePictureViewModel> Handle(ProfilePictureQuery request, CancellationToken cancellationToken)
        {
            var isEnableSecurityCode = securityCodeChecker.IsEnableSecurityCode();
            var imageLibrary = await GetProfilePictureLibrary();

            return new ProfilePictureViewModel(
               imageLibrary.OrderBy(n => Guid.NewGuid()).Take(NumerousOfDefaultPictures),
               request.ViewerLevelId,
               request.AllowUpdate,
               isEnableSecurityCode,
               request.IsOfferProtectedMode,
               request.DisplayLanguage,
               AppSettings.DisabledVietnamese
                   ? Languages.Where(l => l.Key != "vi-VN").ToDictionary(l => l.Key, l => l.Value)
                   : Languages,
               request.AllowUpdateDisplayLanguage);
        }

        public async Task<ProfilePictureLibraryViewModel> Handle(ProfilePictureLibraryQuery request, CancellationToken cancellationToken)
        {
            var images = await GetProfilePictureLibrary();
            var pager = new Pager(totalItems: images.Count(), currentPage: request.CurrentPage);

            var currentPageItems = images.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize);
            return new ProfilePictureLibraryViewModel(currentPageItems, pager);
        }

        public Task<Result> Handle(UpdateActorProfilePictureRequest request, CancellationToken cancellationToken)
            => profilePictureService.UpdateProfilePicture(request.ViewerId, request.ImageId, request.ImageBinary, request.IP);

        internal Task<IEnumerable<ProfilePictureModel>> GetProfilePictureLibrary()
            => cacheService.GetOrSetAsync(
                    ProfilePictureLibraryKey,
                    profilePictureService.FetchProfilePictureLibrary,
                    new CacheItemOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(ProfilePictureLibraryCacheInMinutes))
                );
    }
}