using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;

namespace AlphaPlus.AccountInfo.Site.Features.Shared.Helpers
{
    public static class ImageConverter
    {
        // Length of LOB data to be replicated exceeds configured maximum 65536
        const long MAX_SIZE = 1024 * 20;
        const int STEP_LEVEL = 5;
        const int MIN_LEVEL = 5;
        const int MAX_LEVEL = 100;

        const int IMAGE_WIDTH = 240;
        const int IMAGE_HEIGHT = 240;

        public static byte[] ToLimitedImageSize(string base64String)
        {
            for (int level = MAX_LEVEL; level > MIN_LEVEL; level -= STEP_LEVEL)
            {
                var content = SaveJpg(Base64ToImage(base64String), level);

                if (content.Length <= MAX_SIZE)
                {
                    return content;
                }
            }

            return null;
        }

        private static byte[] SaveJpg(Image image, int level)
        {
            var encoder = new JpegEncoder()
            {
                Quality = level
            };
            using MemoryStream myStream = new();
            image.Save(myStream, encoder);
            return myStream.ToArray();
        }

        private static Image Base64ToImage(string base64String)
        {
            byte[] imageBytes = Convert.FromBase64String(base64String);

            using var ms = new MemoryStream(imageBytes, 0, imageBytes.Length);
            using var image = Image.Load(ms);
            image.Mutate(c => c.Resize(IMAGE_WIDTH, IMAGE_HEIGHT));
            using MemoryStream outStream = new();
            image.Save(outStream, new JpegEncoder());
            return image;
        }
    }
}