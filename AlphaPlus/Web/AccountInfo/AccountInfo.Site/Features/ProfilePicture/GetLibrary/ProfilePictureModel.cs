namespace AlphaPlus.AccountInfo.Site.Features.ProfilePicture.GetLibrary
{
    public class ProfilePictureModel(int imageId, byte[] image)
    {
        public int ImageId { get; private set; } = imageId;

        public string Image { get; private set; } = image == null ? string.Empty : $"data:image/jpeg;base64,{Convert.ToBase64String(image)}";
    }

    public class ProfilePictureDTO
    {
        public ProfilePictureDTO()
        {
        }

        public ProfilePictureDTO(int imageId, byte[] image)
        {
            ImageId = imageId;
            Image = image;
        }

        public int ImageId { get; set; }

        public byte[] Image { get; set; }
    }
}