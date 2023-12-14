using Fanex.Data.Repository;

namespace Alpha.Main.Criteria;

public abstract class BaseCommand : NonQueryCommand
{
    public int ErrCode { get; set; }

    public string ErrMsg { get; set; }
}
