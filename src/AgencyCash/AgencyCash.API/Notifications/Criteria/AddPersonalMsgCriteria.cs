using Fanex.Data;
using Fanex.Data.Repository;

namespace AgencyCash.API.Notifications.Criteria;

public class AddPersonalMsgCriteria(
    string contentEN,
    string contentCH,
    string contentCS,
    string contentJP,
    string contentTH,
    string contentKO,
    string contentVN) : NonQueryCommand
{
    private const int PersonalMsgType = 1;

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

    [SpParam(SqlParamName = "msgtype")]
    public int MessageType { get; } = PersonalMsgType;

    public string CreatorName { get; } = "Agency-Cash-API";

    public bool IsBatch { get; } = true;

    public int MessageIdOutput { get; set; }

    public override string GetSettingKey() => "Age_Msg_SetMessage";

    public override bool IsValid() => true;
}