using System.Web;
using Alpha.Main.ProfilePicture.Services;
using Alpha.Main.Site.Models.Requests.CustomerProfile;
using AlphaPlus.Core.Models;
using Alpha.Main.AppResources;
using MediatR;
using Alpha.Main.Site.ViewModels.CustomerProfile;
using AlphaPlus.Core.Helpers;
using AlphaPlus.Core.Utils;

namespace Alpha.Main.Site.Handlers
{
    public class CustomerProfileHandler(
        ProfilePictureServices customerProfileService,
        IUserSession userSession,
        INicknameChecker nicknameChecker,
        IEncryption encryption) : IRequestHandler<BriefProfileQuery, BriefProfileViewModel>
    {
        private readonly ProfilePictureServices customerProfileService = customerProfileService;
        private readonly INicknameChecker nicknameChecker = nicknameChecker;
        private readonly IUserSession userSession = userSession;
        private readonly IEncryption encryption = encryption;

        public async Task<BriefProfileViewModel> Handle(BriefProfileQuery request, CancellationToken cancellationToken)
        {
            var profile = await customerProfileService.GetActorProfilePicture(userSession.CustId);
            var viewModel = new BriefProfileViewModel
            {
                IsDefaultAvatar = string.IsNullOrEmpty(profile.ImageBase64String),
                EmbedImage = profile.ImageBase64String,
                ProfileImageId = profile.ImageId,
                CustId = request.CustId,
                Username = HttpUtility.HtmlEncode(request.UserName),
                IsInternal = request.IsInternal,
                NickName = BuildNickName(request),
                IntroduceNickNameTooltipCookieName = encryption.CfsEnCode($"ToolTip_{request.CustId}"),
                IsEnableOtp = request.IsEnabledOtp,
                IsHideNicknameLink = nicknameChecker.IsOfferForceLoginAndOverDueDate || request.IsProtectedMode,
                SubAccountTooltip = request.IsSubAcc ? $"{Resources.subAcc} {HttpUtility.HtmlEncode(request.SubUserName)}" : string.Empty,
                IsShowProfilePictureTooltip = !profile.Modified.HasValue,
                IsProtectedMode = request.IsProtectedMode
            };

            return viewModel;
        }

        private static string BuildNickName(BriefProfileQuery message)
            => message.IsInternal
                ? $"{Resources.admin} {HttpUtility.HtmlEncode(message.AdminName)}"
                : HttpUtility.HtmlEncode(message.NickName);
    }
}