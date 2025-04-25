using System.ComponentModel.DataAnnotations;

namespace AlphaPlus.CashService.API.Models.Payments;

public class MobilePrefixModel
{
    public int Id { get; set; }
    public string Prefix { get; set; } = string.Empty;
    public string CountryCode { get; set; } = string.Empty;
    public string CountryName { get; set; } = string.Empty;
    public bool IsActive { get; set; }
} 