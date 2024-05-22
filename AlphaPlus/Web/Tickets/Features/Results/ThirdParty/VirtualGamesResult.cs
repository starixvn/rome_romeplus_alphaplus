using Fanex.Result.Core.ConfigurationHelper;
using Fanex.Result.Core.KenoLotteryTG;

namespace AlphaPlus.Tickets.Site.Features.Results.ThirdParty;

public interface IVirtualGamesResult : IKenoLotteryTGResult
{
}

public class VirtualGamesResult(string sectionName, IConfigurationHelper configurationHelper)
    : KenoLotteryTGResult(sectionName, configurationHelper), IVirtualGamesResult
{
}
