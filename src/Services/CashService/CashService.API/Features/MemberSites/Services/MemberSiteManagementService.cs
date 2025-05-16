using Fanex.Data.Repository;
using AlphaPlus.CashService.API.Features.MemberSites.Criteria;
using AlphaPlus.CashService.API.Features.MemberSites.Models;
using AlphaPlus.CashService.API.Features.Shared.Models;
using AlphaPlus.CashService.API.Features.Shared.Criteria;


namespace AlphaPlus.CashService.API.Features.MemberSites.Services;

public interface IMemberSiteManagementService
{
    Task<MemberSiteModel> GetAsync(int custId, string site);

    Task<ResultModel> UpdateAsync(UpdateMemberSiteModel model);
}

public class MemberSiteManagementService(IDynamicRepository repository) : IMemberSiteManagementService
{
    public async Task<MemberSiteModel> GetAsync(int custId, string site)
    {
        var (domains, settings) = await repository.FetchMultipleAsync<DomainModel, MemberSiteDataObject>(new GetMemberSiteInfoCriteria(custId));
        var countryCodes = await repository.FetchAsync<string>(new GetCountryCodesCriteria(site));
        var settingInfo = settings.FirstOrDefault() ?? new MemberSiteDataObject();

        var trasnformModel = new MemberSiteModel
        {
            Domains = [.. domains],
            GeneralSettings = BuildGeneralSettings(settingInfo),
            Announcement = settingInfo.Announcement,
            Mobile = settingInfo.Mobile,
            Contacts = settingInfo.Contacts,
            CountryCodes = [.. countryCodes]
        };

        return trasnformModel;
    }

    public async Task<ResultModel> UpdateAsync(UpdateMemberSiteModel model)
    {
        if (model == null) throw new ArgumentNullException(nameof(model));

        var command = new UpdateMemberSiteInfoCommand
        {
            CustId = model.CustId,
            SubAccId = model.SubAccId,
            IP = model.IP,
            Language = model.Language,
            Mobile = model.Mobile,
            Contacts = model.Contacts,
            Announcement = model.Announcement,
            SettingInfo = ConvertToSettingInfo(model.GeneralSettings)
        };
        await repository.ExecuteAsync(command);
        return new ResultModel(command.ErrCode, command.ErrMsg);
    }

    private List<MemberSitePermission> BuildGeneralSettings(MemberSiteDataObject settingInfo)
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
            return (int)MemberSiteDataObject.GeneralSettingType.AllowAll;
        if (allowRegistration)
            return (int)MemberSiteDataObject.GeneralSettingType.AllowRegister;
        if (allowDeposit)
            return (int)MemberSiteDataObject.GeneralSettingType.AllowLogin;

        return (int)MemberSiteDataObject.GeneralSettingType.None;
    }
}