using System;

namespace AlphaPlus.Features.Handbook.API.Models
{
    public class ProfilePictureModel
    {
        public byte[] Image { get; set; }

        public int ImageId { get; set; }

        public DateTime? Modified { get; set; }

        public string ImageBase64String => Image == null ? string.Empty : $"data:image/jpeg;base64,{Convert.ToBase64String(Image)}";
    }
}