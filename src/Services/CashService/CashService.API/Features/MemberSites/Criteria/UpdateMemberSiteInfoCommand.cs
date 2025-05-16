using Fanex.Data;
using Fanex.Data.Repository;

namespace AlphaPlus.CashService.API.Features.MemberSites.Criteria;

public class UpdateMemberSiteInfoCommand : INonQueryCommand
{

    public int CustId { get; set; }

    public int SubAccId { get; set; }

    public string IP { get; set; }

    public string Language { get; set; }

    public int SettingInfo { get; set; }

    public string Mobile { get; set; }


    [SpParam(SqlParamName = "Contact")]
    public string Contacts { get; set; }

    public string Announcement { get; set; }

    public string GetSettingKey() => "Age_CashService_GeneralSetting_Upd";

    public int ErrCode { get; set; }

    public string ErrMsg { get; set; } = "";

    public bool IsValid() => CustId > 0;
}