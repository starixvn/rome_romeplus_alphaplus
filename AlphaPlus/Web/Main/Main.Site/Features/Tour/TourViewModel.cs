namespace AlphaPlus.Main.Site.Features.Tour
{
    public class TourViewModel
    {
        public TourViewModel(string language, byte roleId)
        {
            Language = language;
            RoleId = roleId;
        }

        public string Language { get; set; }

        public byte RoleId { get; set; }
    }
}