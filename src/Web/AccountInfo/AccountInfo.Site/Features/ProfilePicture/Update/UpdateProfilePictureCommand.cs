using Fanex.Data;
using Fanex.Data.Repository;

namespace AlphaPlus.AccountInfo.Site.Features.ProfilePicture.Update
{
    public class UpdateProfilePictureCommand : NonQueryCommand
    {
        public UpdateProfilePictureCommand(
            int custId,
            int imageId,
            byte[] image,
            int mode,
            string ip,
            bool isMainServer)
        {
            if (imageId < 0)
            {
                ImageId = null;
                Image = image;
            }
            else
            {
                ImageId = imageId;
                Image = null;
            }

            CustId = custId;
            Mode = mode;
            IP = ip;
            IsDefault = false;
            IsMainServer = isMainServer;
        }

        public int CustId { get; }

        public int? ImageId { get; }

        [SpParam(IsSpecialType = true)]
        public byte[] Image { get; }

        public bool IsDefault { get; }

        public bool IsMainServer { get; }

        public int Mode { get; }

        public string IP { get; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public override string GetSettingKey() => "Age_CustomerUpdProfilePicture";

        public override bool IsValid() => CustId > 0;
    }
}