using Fanex.Data;
using Fanex.Data.Repository;

namespace AlphaPlus.AccountInfo.Site.Features.SearchAccount.Update.Balance;

public class InsertPersonalMessageCriteria : INonQueryCommand
{
    private const int PersonalMessageType = 1;

    public InsertPersonalMessageCriteria(
        string contentEN,
        string contentCH,
        string contentCS,
        string contentTH,
        string contentVN)
    {
        ContentEN = contentEN;
        ContentCH = contentCH;
        ContentCS = contentCS;
        ContentTH = contentTH;
        ContentVN = contentVN;

        MessageType = PersonalMessageType;
        CreatorName = string.Empty;
        IsBatch = true;
    }

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

    [SpParam(SqlParamName = "msgtype")]
    public int MessageType { get; set; }

    public string CreatorName { get; }

    public bool IsBatch { get; }

    public int MessageIDOutput { get; set; }

    public string GetSettingKey() => "Age_Msg_SetMessage";

    public bool IsValid() => true;
}