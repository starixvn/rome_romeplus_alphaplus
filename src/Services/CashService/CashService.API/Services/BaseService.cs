using AlphaPlus.CashService.API.Models.Payments;

namespace AlphaPlus.CashService.API.Services;

public interface IBaseService
{
    Task<IEnumerable<MobilePrefixModel>> GetMobilePrefixes();
}

public class BaseService : IBaseService
{
    public async Task<IEnumerable<MobilePrefixModel>> GetMobilePrefixes()
    {
        // Return dummy data with common mobile prefixes
        return new List<MobilePrefixModel>
        {
            new MobilePrefixModel { Id = 1, Prefix = "+1", CountryCode = "US", CountryName = "United States", IsActive = true },
            new MobilePrefixModel { Id = 2, Prefix = "+44", CountryCode = "GB", CountryName = "United Kingdom", IsActive = true },
            new MobilePrefixModel { Id = 3, Prefix = "+81", CountryCode = "JP", CountryName = "Japan", IsActive = true },
            new MobilePrefixModel { Id = 4, Prefix = "+86", CountryCode = "CN", CountryName = "China", IsActive = true },
            new MobilePrefixModel { Id = 5, Prefix = "+82", CountryCode = "KR", CountryName = "South Korea", IsActive = true },
            new MobilePrefixModel { Id = 6, Prefix = "+65", CountryCode = "SG", CountryName = "Singapore", IsActive = true },
            new MobilePrefixModel { Id = 7, Prefix = "+66", CountryCode = "TH", CountryName = "Thailand", IsActive = true },
            new MobilePrefixModel { Id = 8, Prefix = "+60", CountryCode = "MY", CountryName = "Malaysia", IsActive = true },
            new MobilePrefixModel { Id = 9, Prefix = "+62", CountryCode = "ID", CountryName = "Indonesia", IsActive = true },
            new MobilePrefixModel { Id = 10, Prefix = "+63", CountryCode = "PH", CountryName = "Philippines", IsActive = true },
            new MobilePrefixModel { Id = 11, Prefix = "+84", CountryCode = "VN", CountryName = "Vietnam", IsActive = true },
            new MobilePrefixModel { Id = 12, Prefix = "+91", CountryCode = "IN", CountryName = "India", IsActive = true },
            new MobilePrefixModel { Id = 13, Prefix = "+61", CountryCode = "AU", CountryName = "Australia", IsActive = true },
            new MobilePrefixModel { Id = 14, Prefix = "+64", CountryCode = "NZ", CountryName = "New Zealand", IsActive = true },
            new MobilePrefixModel { Id = 15, Prefix = "+971", CountryCode = "AE", CountryName = "United Arab Emirates", IsActive = true }
        };
    }
} 