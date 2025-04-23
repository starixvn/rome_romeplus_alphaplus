using AlphaPlus.CashService.API.Models;
using System.ComponentModel.DataAnnotations;

namespace AlphaPlus.CashService.API.Services;
public interface IMemberSiteManagementService
{
    Task<MemberSiteManagement> GetAsync(int custId);
    Task<bool> UpdateAsync(MemberSiteManagement model);
} 
public class MemberSiteManagementService : IMemberSiteManagementService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<MemberSiteManagementService> _logger;

    public MemberSiteManagementService(
        IConfiguration configuration,
        ILogger<MemberSiteManagementService> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    public async Task<MemberSiteManagement> GetAsync(int custId)
    {
        try
        {
            // TODO: Replace with database call
            return new MemberSiteManagement
            {
                Domains = new List<MemberSiteManagement.Domain>
                {
                    new() { 
                        Url = "https://quickm-6358906.hdupceria78.com",
                        QrCode = await GenerateQrCodeAsync("https://quickm-6358906.hdupceria78.com")
                    },
                    new() { 
                        Url = "https://quickm-6358906.nammamwang.com",
                        QrCode = await GenerateQrCodeAsync("https://quickm-6358906.nammamwang.com")
                    }
                },
                GeneralSettings = new List<MemberSiteManagement.GeneralSetting>
                {
                    new() { Id = "allowRegistration", Name = "Allow Registration", Description = "Allow member to register", Value = true },
                    new() { Id = "allowDeposit", Name = "Allow Deposit", Description = "Allow member to deposit", Value = false }
                },
                Mobile = "+66812345678",
                LineId = "lineId123",
                Announcement = "Welcome to our cash service!"
            };
        }
        catch (Exception ex)
        {
            //_logger.LogError(ex, "Error getting member site management data");
            throw ex;
        }
    }

    public async Task<bool> UpdateAsync(MemberSiteManagement model)
    {
        try
        {
            if (model == null) throw new ArgumentNullException(nameof(model));

            // Validate model
            var validationContext = new ValidationContext(model);
            var validationResults = new List<ValidationResult>();
            if (!Validator.TryValidateObject(model, validationContext, validationResults, true))
            {
                throw new ValidationException(string.Join(", ", validationResults.Select(r => r.ErrorMessage)));
            }

            // TODO: Save to database
            _logger.LogInformation("Member site management updated successfully");
            return true;
        }
        catch (Exception ex)
        {
            //_logger.LogError(ex, "Error updating member site management data");
            throw ex;
        }
    }

    private async Task<string> GenerateQrCodeAsync(string url)
    {
        // TODO: Implement actual QR code generation
        return "data:image/png;base64,QR_CODE_BASE64_STRING";
    }
} 