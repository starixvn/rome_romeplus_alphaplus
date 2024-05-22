using Fanex.Result.Core.ConfigurationHelper;
using Fanex.Result.Core.KenoLotteryTG;

namespace AlphaPlus.Tickets.Site.Features.Results.ThirdParty;

public interface ILotteryResult : IKenoLotteryTGResult
{
}

public class LotteryResult(string sectionName, IConfigurationHelper configurationHelper)
    : KenoLotteryTGResult(sectionName, configurationHelper), ILotteryResult
{

}