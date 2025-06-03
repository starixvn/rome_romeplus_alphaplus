using AlphaPlus.AccountInfo.Site.Features.Shared.Helpers;
using AlphaPlus.Core;
using AlphaPlus.Core.Models;
using MediatR;

namespace AlphaPlus.AccountInfo.Site.Features.ProfilePicture.Update
{
    public class UpdateActorProfilePictureRequest : QueryBase, IRequest<Result>
    {
        public int ImageId { get; set; }

        public string Image { get; set; }

        public byte[] ImageBinary
            => string.IsNullOrEmpty(Image) ? null : ImageConverter.ToLimitedImageSize(Image);

        public bool AllowUpdate => CanUpdateAccount && !userSession.IsSubAcc;

        public override bool IsValid()
        {
            var IsValidActorPermission = !IsInternal && AllowUpdate && ViewerId > 0;
            var IsValidImage = ImageId > 0 || (ImageId < 0 && ImageBinary != null);
            return IsValidActorPermission && IsValidImage;
        }
    }
}