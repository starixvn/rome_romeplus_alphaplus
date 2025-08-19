using Fanex.Data.Repository;
using AlphaPlus.CashService.API.Features.MemberSites.Criteria;
using AlphaPlus.CashService.API.Features.MemberSites.Models;
using AlphaPlus.CashService.API.Features.Shared.Models;
using AlphaPlus.CashService.API.Features.Shared.Criteria;
using AlphaPlus.CashService.API.Features.Shared.Services;
using AlphaPlus.CashService.API.Features.Shared.Requests;

namespace AlphaPlus.CashService.API.Features.MemberSites.Services;

public interface IMemberSiteManagementService
{
    Task<MemberSiteModel> GetAsync(int custId, string site);

    Task<ResultModel> UpdateAsync(UpdateMemberSiteModel model);
}

public class MemberSiteManagementService(IDynamicRepository repository, ICommonService commonService) : IMemberSiteManagementService
{
    public async Task<MemberSiteModel> GetAsync(int custId, string site)
    {
        var (domains, settings) = await repository.FetchMultipleAsync<DomainModel, MemberSiteDataObject>(new GetMemberSiteInfoCriteria(custId));
        var countryCodes = await repository.FetchAsync<string>(new GetCountryCodesCriteria(site));
        var settingInfo = settings.FirstOrDefault() ?? new MemberSiteDataObject();

        var transformModel = new MemberSiteModel
        {
            Domains = [.. domains],
            GeneralSettings = BuildGeneralSettings(settingInfo),
            Announcement = settingInfo.Announcement,
            Mobile = settingInfo.Mobile,
            Contacts = settingInfo.Contacts,
            CountryCodes = [.. countryCodes]
        };

        return transformModel;
    }

    public async Task<ResultModel> UpdateAsync(UpdateMemberSiteModel model)
    {
        ArgumentNullException.ThrowIfNull(model);

        var command = new UpdateMemberSiteInfoCommand(model.CustId, model.SubAccId, model.IP, model.Language)
        {
            Mobile = model.Mobile,
            Contacts = model.Contacts,
            Announcement = model.Announcement,
            SettingInfo = ConvertToSettingInfo(model.GeneralSettings)
        };

        var request = new BaseActionRequest
        {
            ActorId = model.UserId,
            UserId = model.CustId,
            IP = model.IP,
            Language = model.Language,
            CustDBGroupId = model.SiteDB
        };

        await commonService.RunDBCommand(request, command);
        return new ResultModel(command.ErrCode, command.ErrMsg);
    }

    private List<MemberSitePermission> BuildGeneralSettings(MemberSiteDataObject settingInfo)
    {
        return
        [
            new()
            {
                Id = "allowRegistration", Name = "Allow Registration", Description = "Allow member to register",
                Value = settingInfo.IsAllowRegister
            },
            new()
            {
                Id = "allowDeposit", Name = "Allow Deposit", Description = "Allow member to deposit",
                Value = settingInfo.IsAllowDeposit
            }
        ];
    }

    private int ConvertToSettingInfo(IEnumerable<MemberSitePermission> generalSettings)
    {
        var memberSitePermissions = generalSettings.ToList();
        var allowRegistration = memberSitePermissions.First(x => x.Id == "allowRegistration").Value;
        var allowDeposit = memberSitePermissions.First(x => x.Id == "allowDeposit").Value;

        if (allowRegistration && allowDeposit)
        {
            return (int)MemberSiteDataObject.GeneralSettingType.AllowAll;
        }

        if (allowRegistration)
        {
            return (int)MemberSiteDataObject.GeneralSettingType.AllowRegister;
        }
        
        if (allowDeposit)
        {
            return (int)MemberSiteDataObject.GeneralSettingType.AllowDeposit;
        }

        return (int)MemberSiteDataObject.GeneralSettingType.None;
    }
}