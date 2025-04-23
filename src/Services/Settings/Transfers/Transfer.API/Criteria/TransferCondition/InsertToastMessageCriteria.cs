using Fanex.Data;
using Fanex.Data.Repository;

namespace AlphaPlus.Settings.Transfer.API.Criteria.TransferCondition;

public class InsertToastMessageCriteria(
    int typeId,
    int siteFunctionId,
    int targetId,
    int creator,
    string contentEN,
    string contentCH,
    string contentCS,
    string contentTH) : NonQueryCommand
{
    public int TypeId { get; } = typeId;

    public int SiteFunctionId { get; } = siteFunctionId;

    public int TargetId { get; } = targetId;

    public int Creator { get; } = creator;

    [SpParam(SqlParamName = "content_en")]
    public string ContentEN { get; } = contentEN;

    [SpParam(SqlParamName = "content_ch")]
    public string ContentCH { get; } = contentCH;

    [SpParam(SqlParamName = "content_cs")]
    public string ContentCS { get; } = contentCS;

    [SpParam(SqlParamName = "content_jp")]
    public string ContentJP { get; }

    [SpParam(SqlParamName = "content_it")]
    public string ContentIT => string.Empty;

    [SpParam(SqlParamName = "content_th")]
    public string ContentTH { get; } = contentTH;

    [SpParam(SqlParamName = "content_ko")]
    public string ContentKO { get; }

    [SpParam(SqlParamName = "content_es")]
    public string ContentES => string.Empty;

    [SpParam(SqlParamName = "content_vn")]
    public string ContentVN { get; }

    public int ErrCode { get; set; }

    public string ErrMsg { get; set; }

    public int ToastId { get; set; }

    public override string GetSettingKey() => "Age_ToastMessage_Ins";

    public override bool IsValid() => true;
}