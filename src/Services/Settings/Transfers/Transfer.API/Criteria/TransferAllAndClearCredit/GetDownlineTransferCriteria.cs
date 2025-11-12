namespace AlphaPlus.Settings.Transfer.API.Criteria.TransferAllAndClearCredit;

using Fanex.Data;
using Fanex.Data.Repository;

public class GetDownlineTransferCriteria(int custId, string downlineUserName) : NonQueryCommand
{
    public int CustId { get; set; } = custId;

    [SpParam(SqlParamName = "DownlineUsername")]
    public string Username { get; set; } = downlineUserName;

    public int DownlineCustId { get; set; }

    public byte DownlineRoleId { get; set; }

    public int ErrCode { get; set; }

    public string ErrMsg { get; set; }

    public override string GetSettingKey()
        => "SiteDB_Age_Transfer_Credit_All_SelDownlineInfo";

    public override bool IsValid()
        => !string.IsNullOrWhiteSpace(Username) && CustId > 0;
}