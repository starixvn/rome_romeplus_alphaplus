namespace Alpha.Main.Site.Features.Banner
{
    public class BannerItem
    {
        public BannerItem()
        {
        }

        public BannerItem(byte[] imageContent)
        {
            ImageContent = imageContent;
        }

        public byte[] ImageContent { get; }
    }
}
