namespace AlphaPlus.AccountInfo.Site.Features.UnsuspendDownlines.Models
{
    public class UnsuspendSingleModel
    {
        public byte RoleId { get; set; }

        public int AffectedDownlines { get; set; }

        public int UnchangedDownlines { get; set; }
    }
}