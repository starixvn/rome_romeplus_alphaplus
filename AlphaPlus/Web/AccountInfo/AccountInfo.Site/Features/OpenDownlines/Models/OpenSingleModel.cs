namespace AlphaPlus.AccountInfo.Site.Features.OpenDownlines.Models
{
    public class OpenSingleModel
    {
        public OpenSingleModel()
        {
        }

        public OpenSingleModel(byte roleId, int affectedDownlines, int unchangedDownlines)
        {
            RoleId = roleId;
            AffectedDownlines = affectedDownlines;
            UnchangedDownlines = unchangedDownlines;
        }

        public byte RoleId { get; set; }

        public int AffectedDownlines { get; set; }

        public int UnchangedDownlines { get; set; }
    }
}