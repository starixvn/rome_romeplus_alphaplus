using Fanex.Data;
using Fanex.Data.Repository;

namespace AlphaPlus.CashService.API.Criteria.MemberSite
{
    public class UpdateMemberSiteInfoCommand : INonQueryCommand
    {
        [SpParam(SqlParamName = "CustID")]
        public int CustId { get; set; }

        [SpParam(SqlParamName = "SettingInfo")]
        public int SettingInfo { get; set; }

        [SpParam(SqlParamName = "CountryCode")]
        public string CountryCode { get; set; }

        [SpParam(SqlParamName = "Mobile")]
        public string Mobile { get; set; }

        [SpParam(SqlParamName = "LineID")]
        public string LineId { get; set; }
        
        [SpParam(SqlParamName = "Announcement")]
        public string Announcement { get; set; }
        public string GetSettingKey() => "Age_CashService_GeneralSetting_Upd";
        public int ErrCode { get; set; }
        public string ErrMsg { get; set; } = "";
        public bool IsValid() => CustId > 0;
    }
}