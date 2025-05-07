using AlphaPlus.CashService.API.Criteria.MemberSite;
using AlphaPlus.CashService.API.Models;
using Fanex.Data.Repository;

namespace AlphaPlus.CashService.API.Services;

public interface IMemberSiteManagementService
{
    Task<MemberSiteManagementModel> GetAsync(int custId);

    Task<ResultModel> UpdateAsync(MemberSiteManagementModel model);
}

public class MemberSiteManagementService(
    IConfiguration configuration,
    ILogger<MemberSiteManagementService> logger,
    IDynamicRepository repository)
    : IMemberSiteManagementService
{
    private readonly IConfiguration _configuration = configuration;
    private readonly ILogger<MemberSiteManagementService> _logger = logger;

    public async Task<MemberSiteManagementModel> GetAsync(int custId)
    {
        var (domains, settings) = await repository.FetchMultipleAsync<DomainModel, CashServiceManagementDto>(new GetMemberSiteInfoCriteria(custId));

        var settingInfo = settings.FirstOrDefault() ?? new CashServiceManagementDto();

        var trasnformModel = new MemberSiteManagementModel
        {
            Domains = domains.ToList(),
            GeneralSettings = BuildGeneralSettings(settingInfo),
            Announcement = settingInfo.Announcement,
            Mobile = settingInfo.Mobile,
            LineId = settingInfo.LineId,
            CountryCode = "66"
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

    private int ConvertToSettingInfo(List<MemberSitePermission> generalSettings)
    {
        var allowRegistration = generalSettings.FirstOrDefault(x => x.Id == "allowRegistration")?.Value ?? false;
        var allowDeposit = generalSettings.FirstOrDefault(x => x.Id == "allowDeposit")?.Value ?? false;

        if (allowRegistration && allowDeposit)
            return (int)CashServiceManagementDto.GeneralSettingType.AllowAll;
        if (allowRegistration)
            return (int)CashServiceManagementDto.GeneralSettingType.AllowRegister;
        if (allowDeposit)
            return (int)CashServiceManagementDto.GeneralSettingType.AllowLogin;

        return (int)CashServiceManagementDto.GeneralSettingType.None;
    }

    public async Task<ResultModel> UpdateAsync(MemberSiteManagementModel model)
    {
        try
        {
            if (model == null) throw new ArgumentNullException(nameof(model));

            // Validate model
            var command = new UpdateMemberSiteInfoCommand
            {
                CustId = model.CustId,
                Mobile = model.Mobile,
                LineId = model.LineId,
                Announcement = model.Announcement,
                CountryCode = model.CountryCode,
                SettingInfo = ConvertToSettingInfo(model.GeneralSettings)
            };
            await repository.ExecuteAsync(command);
            return new ResultModel(command.ErrCode, command.ErrMsg);
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