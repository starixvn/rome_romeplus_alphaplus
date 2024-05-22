using Fanex.Result.Core.ConfigurationHelper;
using Fanex.Result.Core.KenoLotteryTG;

namespace AlphaPlus.Tickets.Site.Features.Results.ThirdParty;

public interface IKenoResult : IKenoLotteryTGResult
{
}

public class KenoResult(string sectionName, IConfigurationHelper configurationHelper)
    : KenoLotteryTGResult(sectionName, configurationHelper), IKenoResult
{
}