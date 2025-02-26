using Fanex.Data;
using Fanex.Data.Repository;

namespace AlphaPlus.AccountInfo.Site.Features.SearchAccount.Update.Balance;

public sealed class InsertToastMessageCriteria : NonQueryCommand
{
    private const int WarningTypeId = 3;
    private const int SiteFunctionId_ADBTChange = 2;

    public InsertToastMessageCriteria(
        int targetId,
        int creator,
        string contentEN,
        string contentCS,
        string contentCH,
        string contentTH,
        string contentVN)
    {
        TypeId = WarningTypeId;
        SiteFunctionId = SiteFunctionId_ADBTChange;
        TargetId = targetId;
        Creator = creator;

        ContentEN = contentEN;
        ContentCS = contentCS;
        ContentCH = contentCH;
        ContentTH = contentTH;
        ContentVN = contentVN;
    }

    public int TypeId { get; set; }

    public int SiteFunctionId { get; set; }

    public int TargetId { get; set; }

    public int Creator { get; set; }

    [SpParam(SqlParamName = "content_en")]
    public string ContentEN { get; set; }

    [SpParam(SqlParamName = "content_ch")]
    public string ContentCH { get; set; }

    [SpParam(SqlParamName = "content_cs")]
    public string ContentCS { get; set; }

    [SpParam(SqlParamName = "content_jp")]
    public string ContentJP { get; set; }

    [SpParam(SqlParamName = "content_it")]
    public string ContentIT => string.Empty;

    [SpParam(SqlParamName = "content_th")]
    public string ContentTH { get; set; }

    [SpParam(SqlParamName = "content_ko")]
    public string ContentKO { get; set; }

    [SpParam(SqlParamName = "content_es")]
    public string ContentES => string.Empty;

    [SpParam(SqlParamName = "content_vn")]
    public string ContentVN { get; set; }

    public int ErrCode { get; set; }

    public string ErrMsg { get; set; }

    public int ToastId { get; set; }

    public override string GetSettingKey() => "Age_ToastMessage_Ins";

    public override bool IsValid() => true;
}