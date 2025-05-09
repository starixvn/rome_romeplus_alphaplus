using AlphaPlus.CashService.API.Criteria.MemberSite;
using AlphaPlus.CashService.API.Criteria.Common;
using AlphaPlus.CashService.API.Models;
using Fanex.Data.Repository;
using AlphaPlus.Shared;

namespace AlphaPlus.CashService.API.Services;
public interface IMemberSiteManagementService
{
    Task<MemberSiteManagementModel> GetAsync(int custId, string site);

    Task<ResultModel> UpdateAsync(MemberSiteManagementModel model);
}

public class MemberSiteManagementService(
    IConfiguration configuration,
    ILogger<MemberSiteManagementService> logger,
    IDynamicRepository repository
    )
    : IMemberSiteManagementService
{
    private readonly IConfiguration _configuration = configuration;
    private readonly ILogger<MemberSiteManagementService> _logger = logger;
    private readonly string _encryptSecretKey = configuration.GetValue<string>("EncryptSecretKey") ?? AlphaPlusConstants.EncryptSecretKey;
    public async Task<MemberSiteManagementModel> GetAsync(int custId, string site)
    {
        var (domains, settings) = await repository.FetchMultipleAsync<DomainModel, CashServiceManagementDto>(new GetMemberSiteInfoCriteria(custId));
        var countryCodes = await repository.FetchAsync<string>(new GetCountryCodesCriteria(site));
        var settingInfo = settings.FirstOrDefault() ?? new CashServiceManagementDto();

        var trasnformModel = new MemberSiteManagementModel
        {
            Domains = domains.ToList(),
            GeneralSettings = BuildGeneralSettings(settingInfo),
            Announcement = settingInfo.Announcement,
            Mobile = settingInfo.Mobile,
            LineId = settingInfo.LineId,
            CountryCodes = countryCodes
        };

        return trasnformModel;
    }

    private List<MemberSitePermission> BuildGeneralSettings(CashServiceManagementDto settingInfo)
    {
        return new List<MemberSitePermission>
        {
            new()
            {
                Id = "allowRegistration", Name = "Allow Registration", Description = "Allow member to register",
                Value = settingInfo.IsAllowRegister
            },
            new()
            {
                Id = "allowDeposit", Name = "Allow Deposit", Description = "Allow member to deposit",
                Value = settingInfo.IsAllowLogin
            }
        };
    }

    private int ConvertToSettingInfo(IEnumerable<MemberSitePermission> generalSettings)
    {
        var memberSitePermissions = generalSettings.ToList();
        var allowRegistration = memberSitePermissions.FirstOrDefault(x => x.Id == "allowRegistration")?.Value ?? false;
        var allowDeposit = memberSitePermissions.FirstOrDefault(x => x.Id == "allowDeposit")?.Value ?? false;

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
        if (model == null) throw new ArgumentNullException(nameof(model));
        
        var command = new UpdateMemberSiteInfoCommand
        {
            CustId = model.CustId,
            Mobile = model.Mobile,
            LineId = model.LineId,
            Announcement = model.Announcement,
            SettingInfo = ConvertToSettingInfo(model.GeneralSettings)
        };
        await repository.ExecuteAsync(command);
        return new ResultModel(command.ErrCode, command.ErrMsg);
    }
}