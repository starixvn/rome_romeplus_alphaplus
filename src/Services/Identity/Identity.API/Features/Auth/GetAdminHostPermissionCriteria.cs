using Fanex.Data.Repository;

namespace AlphaPlus.Identity.API.Features.Auth;

public class GetAdminHostPermissionCriteria(int adminId, int? funcId, string funcName) : NonQueryCommand
{
    public int CustId => adminId;

    public int? FuncId => funcId;

    public string FuncName => funcName;

    public int ErrCode { get; set; }

    public string ErrMsg { get; set; }

    public override string GetSettingKey()
        => "Acc_Con_CheckPermission";

    public override bool IsValid()
        => CustId > 0 && (FuncId.HasValue || !string.IsNullOrWhiteSpace(FuncName));
}