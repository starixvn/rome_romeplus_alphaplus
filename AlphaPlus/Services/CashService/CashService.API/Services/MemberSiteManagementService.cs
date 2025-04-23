using AlphaPlus.CashService.API.Models;
using System.ComponentModel.DataAnnotations;
using AlphaPlus.CashService.API.Criteria.MemberSite;
using Fanex.Data.Repository;
using System.Numerics;

namespace AlphaPlus.CashService.API.Services;
public interface IMemberSiteManagementService
{
    Task<MemberSiteManagementModel> GetAsync(int custId);
    Task<bool> UpdateAsync(MemberSiteManagementModel model);
}
public class MemberSiteManagementService : IMemberSiteManagementService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<MemberSiteManagementService> _logger;
    private readonly IDynamicRepository _repository;

    public MemberSiteManagementService(
        IConfiguration configuration,
        ILogger<MemberSiteManagementService> logger,
        IDynamicRepository repository)
    {
        _configuration = configuration;
        _logger = logger;
        _repository = repository;
    }

    public async Task<MemberSiteManagementModel> GetAsync(int custId)
    {

        var (domains, settings) = await _repository.FetchMultipleAsync<DomainModel, CashServiceManagementDto>(new GetMemberSiteInfoCriteria(custId));

        var settingInfo = settings.FirstOrDefault() ?? new CashServiceManagementDto();

        var trasnformModel = new MemberSiteManagementModel
        {
            Domains = domains.ToList(),
            GeneralSettings = BuildGeneralSettings(settingInfo),
            Announcement = settingInfo.Announcement,
            Mobile = settingInfo.Mobile,
            LineId = settingInfo.LineId
        };

        return trasnformModel;
    }
    private List<MemberSitePermission> BuildGeneralSettings(CashServiceManagementDto settingInfo)
    {
        return new List<MemberSitePermission>
        {
            new() { Id = "allowRegistration", Name = "Allow Registration", Description = "Allow member to register", Value = settingInfo.IsAllowRegister },
            new() { Id = "allowDeposit", Name = "Allow Deposit", Description = "Allow member to deposit", Value = settingInfo.IsAllowLogin }
        };
    }


    public async Task<bool> UpdateAsync(MemberSiteManagementModel model)
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