namespace AlphaPlus.BetLists.CasinoDetails;

internal class GetRNGCasinoDetailsCriteria(long refNo,
    int custId,
    byte transType,
    int productId,
    int pageIndex,
    int pageSize,
    DateTime moveBetsDate,
    DateTime balanceUpDate)
        : GetCasinoDetailsCriteria(refNo, custId, transType, productId, pageIndex, pageSize, moveBetsDate, balanceUpDate)
{
    public override string GetSettingKey() => "Acc_BetList_RNG_Detail";
}