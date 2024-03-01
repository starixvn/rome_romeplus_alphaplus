using Fanex.Data.Repository;

namespace AlphaPlus.Reports.Features.Legends;

public class GetProductsByRefTableCriteria(string refTable) : CriteriaBase
{
    public string RefTable { get; } = refTable;

    public override string GetSettingKey() => "Age_Rpt_GetProductListByRefTable";

    public override bool IsValid() => true;
}