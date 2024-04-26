using AlphaPlus.Settings.Transfer.API.Params.AutoTransfer;
using Fanex.Data.Repository;

namespace AlphaPlus.Settings.Transfer.API.Criteria.AutoTransfer
{
    public class CancelAllJobCriteria : NonQueryCommand
    {
        public CancelAllJobCriteria(CancelAllJobParam param)
        {
            FromCustId = param.FromCustId;
            Language = param.Language;
            IP = param.IP;

            ErrCode = 0;
            ErrMsg = string.Empty;
        }

        public int FromCustId { get; }

        public string? Language { get; set; }

        public string? IP { get; set; }

        public int Creator => FromCustId;

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public override string GetSettingKey() => "SiteDB_Age_ClearBalanceCust_ProcessCancel_All";

        public override bool IsValid() => FromCustId > 0;
    }
}
