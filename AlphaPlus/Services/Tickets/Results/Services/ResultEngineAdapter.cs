using Fanex.Result.Core;
using Fanex.Result.Core.Base;

namespace AlphaPlus.Results.Services;

public interface IResultEngineAdapter
{
    MemoryStream ExcelParser<T>(IList<T> dataList, ResultSetting setting);

    string HtmlGameDetailParser(dynamic data, ResultSetting setting);

    string HtmlParser<T>(IList<T> dataList, ResultSetting setting);
}

public class ResultEngineAdapter : IResultEngineAdapter
{
    public MemoryStream ExcelParser<T>(IList<T> dataList, ResultSetting setting)
        => ResultEngine.ExcelParser(dataList, setting);

    public string HtmlGameDetailParser(dynamic data, ResultSetting setting)
        => ResultEngine.HtmlGameDetailParser(data, setting);

    public string HtmlParser<T>(IList<T> dataList, ResultSetting setting)
        => ResultEngine.HtmlParser<T>(dataList, setting);
}
