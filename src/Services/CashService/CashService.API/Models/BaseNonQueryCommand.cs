namespace AlphaPlus.CashService.API.Models;

using Fanex.Data.Repository;

public abstract class BaseNonQueryCommand : NonQueryCommand
{
    public int ErrCode { get; set; }

    public string ErrMsg { get; set; }
}