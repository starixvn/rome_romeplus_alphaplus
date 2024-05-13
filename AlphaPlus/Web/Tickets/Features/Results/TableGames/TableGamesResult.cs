using Fanex.Result.Core.ConfigurationHelper;
using Fanex.Result.Core.KenoLotteryTG;

namespace AlphaPlus.Tickets.Site.Features.Results.TableGames;

public interface ITableGamesResult : IKenoLotteryTGResult
{
}

public class TableGamesResult : KenoLotteryTGResult, ITableGamesResult
{
    public TableGamesResult(string sectionName, IConfigurationHelper configurationHelper)
        : base(sectionName, configurationHelper)
    {
    }
}