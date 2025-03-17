using System.Numerics;
using AlphaPlus.Utilities.IP;
using Fanex.Alpha.Before.Criteria;
using Fanex.Alpha.Before.Models;
using Fanex.Data.Repository;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace Fanex.Alpha.Before.Services
{
    public interface IProfilePictureService
    {
        Task<ProfilePicture> GetProfilePicture(int custId);

        public string MakeWaterMarkImage(byte[] imageBytes, string waterMarkText);
    }

    public class ProfilePictureService : IProfilePictureService
    {
        private readonly IPHelper ipHelper;
        private readonly Logging.ILogger logger;
        private readonly IDynamicRepository dynamicRepository;

        public ProfilePictureService(IDynamicRepository dynamicRepository, Logging.ILogger logger, IPHelper ipHelper)
        {
            this.dynamicRepository = dynamicRepository;
            this.logger = logger;
            this.ipHelper = ipHelper;
        }

        public async Task<ProfilePicture> GetProfilePicture(int custId)
        {
            try
            {
                if (custId > 0)
                {
                    var criteria = new GetProfilePictureCriteria(custId);
                    var profilePicture = await GetProfilePicture(criteria);
                    if (profilePicture == null || string.IsNullOrEmpty(profilePicture.ImageBase64String))
                    {
                        return await InsertDefaultProfilePicture(custId);
                    }

                    return profilePicture;
                }
                else
                {
                    return await GetRandomProfilePicture();
                }
            }
            catch (Exception ex)
            {
                await logger.ErrorAsync(ex.Message, ex);
                return new ProfilePicture();
            }
        }

        private async Task<ProfilePicture> GetProfilePicture(GetProfilePictureCriteria criteria)
        {
            var profilePicture = await dynamicRepository.GetAsync<ProfilePicture>(criteria);
            return profilePicture ?? new ProfilePicture();
        }

        private async Task<ProfilePicture> InsertDefaultProfilePicture(int custId)
        {
            var command = new UpdateProfilePictureCommand(custId, ipHelper.GetClientIP());
            await dynamicRepository.ExecuteAsync(command);
            return command.ErrCode == 0 ? new ProfilePicture { Image = command.Image } : new ProfilePicture();
        }

        public async Task<ProfilePicture> GetRandomProfilePicture()
        {
            try
            {
                var profilePicture = await dynamicRepository.GetAsync<ProfilePicture>(new GetRandomProfilePictureCriteria());
                return profilePicture ?? new ProfilePicture();
            }
            catch (Exception ex)
            {
                await logger.ErrorAsync(ex.Message, ex);
                return new ProfilePicture();
            }
        }

        public string MakeWaterMarkImage(byte[] imageBytes, string waterMarkText)
        {
            var image = Image.Load<Rgba32>(imageBytes);
            var font = SystemFonts.CreateFont("Arial", 24, FontStyle.Bold);

            image.Mutate(x => ApplyScalingWaterMark(x, waterMarkText, font, Color.Red, 5));

            using var ms = new MemoryStream();
            image.SaveAsJpeg(ms);

            return $"data:image/jpeg;base64,{Convert.ToBase64String(ms.ToArray())}";
        }

        private IImageProcessingContext ApplyScalingWaterMark(IImageProcessingContext processingContext,
            string text,
            Font font,
            Color color,
            float padding)
        {
            Size imageSize = processingContext.GetCurrentSize();
            float targetWidth = imageSize.Width - (padding * 2);
            float targetHeight = imageSize.Height - (padding * 2);
            float minFontSize = 1;
            float maxFontSize = font.Size;

            FontRectangle currentBounds = TextMeasurer.MeasureAdvance(text, new TextOptions(font));
            if (currentBounds.Width < targetWidth)
            {
                maxFontSize = MathF.Floor(maxFontSize * (targetWidth / currentBounds.Width));
            }

            while (minFontSize < maxFontSize)
            {
                float midFontSize = (minFontSize + maxFontSize) / 2;
                Font midFont = new(font, midFontSize);
                currentBounds = TextMeasurer.MeasureAdvance(text, new TextOptions(midFont)
                {
                    WrappingLength = targetWidth
                });

                if (currentBounds.Height > targetHeight)
                {
                    maxFontSize = midFontSize - 0.1f;
                }
                else
                {
                    minFontSize = midFontSize + 0.1f;
                }
            }

            Font scaledFont = new(font, minFontSize);

            RichTextOptions textOptions = new(scaledFont)
            {
                Origin = new Vector2(padding, imageSize.Height * .5f),
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Center,
                WrappingLength = targetWidth
            };

            processingContext.Fill(Color.LightBlue, new RectangleF(padding, imageSize.Height * .5f - 18, targetWidth, 35));

            return processingContext.DrawText(textOptions, text, color);
        }
    }
}