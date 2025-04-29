using Fanex.Data;
using Fanex.Data.Repository;

namespace AlphaPlus.CashService.API.Criteria.MemberSite
{
    public class UpdateMemberSiteInfoCommand : INonQueryCommand
    {
        
        public int CustId { get; set; }

        public int SettingInfo { get; set; }

        public string CountryCode { get; set; }

        public string Mobile { get; set; }

        public string LineId { get; set; }
        
        public string Announcement { get; set; }

        public string GetSettingKey() => "Age_CashService_GeneralSetting_Upd";
        
        public int ErrCode { get; set; }

        public string ErrMsg { get; set; } = "";
        
        public bool IsValid() => CustId > 0;
    }
}