namespace AlphaPlus.Main.Site.Features.CustomerProfile
{
    public class ActorProfilePictureModel
    {
        public byte[] Image { get; init; }

        public int ImageId { get; init; }

        public DateTime? Modified { get; init; }

        public string ImageBase64String => Image == null ? string.Empty : $"data:image/jpeg;base64,{Convert.ToBase64String(Image)}";
    }
}
