using Fanex.Data.Repository;

namespace Alpha.Main.Criteria.SignIn;

public class CheckIsShowConsolidatedGroupMenuCommand : INonQueryCommand
{
    public int CustId { get; init; }

    public bool IsShownMenu { get; set; } = false;

    public string GetSettingKey() => "Age_Rpt_ConsolidatedGroup_isShownMenu";

    public bool IsValid() => CustId > 0;

}
