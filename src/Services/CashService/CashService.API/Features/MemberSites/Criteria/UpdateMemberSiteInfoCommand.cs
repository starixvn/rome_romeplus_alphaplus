using Fanex.Data;
using AlphaPlus.CashService.API.Features.Shared.Criteria;

namespace AlphaPlus.CashService.API.Features.MemberSites.Criteria;

public class UpdateMemberSiteInfoCommand : BaseDBCommand
{

    public UpdateMemberSiteInfoCommand(int custId, int subAccId, string ip, string language) : base(custId, subAccId, ip, language)
    {
    }

    public int SettingInfo { get; set; }

    public string Mobile { get; set; }


    [SpParam(SqlParamName = "Contact")]
    public string Contacts { get; set; }

    public string Announcement { get; set; }

    public override string GetSettingKey() => "Age_CashService_GeneralSetting_Upd";
}