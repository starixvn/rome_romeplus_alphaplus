using Fanex.Data.Repository;

namespace AlphaPlus.Utilities.IP.Criteria;

public class CheckHideIPCriteria(int superId) : INonQueryCommand
{
    public int SuperId { get; } = superId;

    public bool IsHideIP { get; set; }

    public string GetSettingKey() => "Age_CustomerCheckHideIP";

    public bool IsValid() => SuperId > 0;
}
