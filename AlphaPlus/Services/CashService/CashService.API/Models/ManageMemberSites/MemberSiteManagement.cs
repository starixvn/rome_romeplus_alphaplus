using System.ComponentModel.DataAnnotations;

namespace AlphaPlus.CashService.API.Models;

public class MemberSiteManagement
{
    // Domain settings
    public List<Domain> Domains { get; set; } = new();
    public List<GeneralSetting> GeneralSettings { get; set; } = new();

    // Message settings
    [RegularExpression(@"^\+66[0-9]{9}$", ErrorMessage = "Mobile number must be in format +66XXXXXXXXX")]
    public string Mobile { get; set; }

    [Required(ErrorMessage = "LINE ID is required")]
    public string LineId { get; set; }

    [MaxLength(1000, ErrorMessage = "Announcement cannot exceed 1000 characters")]
    public string Announcement { get; set; }

    public class Domain
    {
        public string Url { get; set; }
        public string QrCode { get; set; }
    }

    public class GeneralSetting
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Value { get; set; }
    }
}