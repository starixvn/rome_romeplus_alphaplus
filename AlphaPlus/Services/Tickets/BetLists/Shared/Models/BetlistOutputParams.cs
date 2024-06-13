namespace AlphaPlus.BetLists.Shared.Models;

public class BetListOutputParams(int errCode = 0, string errMsg = "", int maxTotalRow = 0)
{
    public int ErrCode { get; } = errCode;

    public string ErrMsg { get; } = errMsg;

    public int MaxTotalRow { get; } = maxTotalRow;
}