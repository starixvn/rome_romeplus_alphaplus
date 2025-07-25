using Fanex.Data;
using Fanex.Data.Repository;

namespace AgencyCash.API.Notifications.Criteria;

public class AddToastMsgCriteria(
    int targetId,
    byte typeId,
    byte siteFunctionId,
    int creator,
    string contentEN,
    string contentCH,
    string contentCS,
    string contentTH,
    string contentVN,
    string contentJP,
    string contentKO) : NonQueryCommand
{
    public int TargetId { get; } = targetId;

    public byte TypeId { get; } = typeId;

    public byte SiteFunctionId { get; } = siteFunctionId;

    public int Creator { get; } = creator;

    [SpParam(SqlParamName = "content_en")]
    public string ContentEN { get; } = contentEN;

    [SpParam(SqlParamName = "content_ch")]
    public string ContentCH { get; } = contentCH;

    [SpParam(SqlParamName = "content_cs")]
    public string ContentCS { get; } = contentCS;

    [SpParam(SqlParamName = "content_jp")]
    public string ContentJP { get; } = contentJP;

    [SpParam(SqlParamName = "content_th")]
    public string ContentTH { get; } = contentTH;

    [SpParam(SqlParamName = "content_ko")]
    public string ContentKO { get; } = contentKO;

    [SpParam(SqlParamName = "content_vn")]
    public string ContentVN { get; } = contentVN;

    public int ToastId { get; set; }

    public int ErrCode { get; set; }

    public string ErrMsg { get; set; }

    public override string GetSettingKey() => "Age_ToastMessage_Ins";

    public override bool IsValid() => true;
}
