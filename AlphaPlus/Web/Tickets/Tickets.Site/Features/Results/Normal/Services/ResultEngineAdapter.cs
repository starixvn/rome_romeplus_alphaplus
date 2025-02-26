using Fanex.Result.Core;
using Fanex.Result.Core.Base;

namespace AlphaPlus.Tickets.Site.Features.Results.Normal;

public interface IResultEngineAdapter
{
    string HtmlParser<T>(IList<T> dataList, ResultSetting setting);

    string HtmlGameDetailParser(dynamic data, ResultSetting setting);

    MemoryStream ExcelParser<T>(IList<T> dataList, ResultSetting setting);
}

public class ResultEngineAdapter : IResultEngineAdapter
{
    public string HtmlParser<T>(IList<T> dataList, ResultSetting setting)
        => ResultEngine.HtmlParser(dataList, setting);

    public string HtmlGameDetailParser(dynamic data, ResultSetting setting)
        => ResultEngine.HtmlGameDetailParser(data, setting);

    public MemoryStream ExcelParser<T>(IList<T> dataList, ResultSetting setting)
        => ResultEngine.ExcelParser(dataList, setting);
}