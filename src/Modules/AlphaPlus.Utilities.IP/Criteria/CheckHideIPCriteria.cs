using Fanex.Data;
using Fanex.Data.Repository;

namespace AlphaPlus.Utilities.IP.Criteria;

public class CheckHideIPCriteria(int superId) : INonQueryCommand
{
    [SpParam(SqlParamName = "custId")]
    public int SuperId { get; } = superId;

    public bool IsHideIP { get; set; }

    public string GetSettingKey() => "Age_CustomerCheckHideIP";

    public bool IsValid() => true;
}
