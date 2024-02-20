namespace AlphaPlus.Tickets.Site.Features.BetLists.ViewModels;

public class ExcelFileViewModel(string name, byte[] content)
{
    public string Name { get; } = GetValidFileName(name);

    public byte[] Content { get; } = content;

    public string ContentType => "application/vnd.ms-excel";

    private static string GetValidFileName(string fileName)
        => string.IsNullOrWhiteSpace(fileName)
        ? "no-name.xls"
        : Path.GetInvalidFileNameChars().Aggregate(fileName, (current, c) => current.Replace(c.ToString(), string.Empty)) + ".xls";
}
