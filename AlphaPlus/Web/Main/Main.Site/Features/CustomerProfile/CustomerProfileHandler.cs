using System.Web;
using AlphaPlus.Core.Models;
using AlphaPlus.Shared.AppResources.Main;
using MediatR;
using AlphaPlus.Core.Utils;

namespace AlphaPlus.Main.Site.Features.CustomerProfile
{
    public class CustomerProfileHandler(
        IProfilePictureService customerProfileService,
        IUserSession userSession,
        IEncryption encryption) : IRequestHandler<BriefProfileQuery, BriefProfileViewModel>
    {
        private readonly IProfilePictureService customerProfileService = customerProfileService;
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
                NickName = BuildNickName(request).ToUpperInvariant(),
                IntroduceNickNameTooltipCookieName = encryption.CfsEnCode($"ToolTip_{request.CustId}"),
                IsEnableOtp = request.IsEnabledOtp,
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