namespace AlphaPlus.Core.Models;

public class ExcelFileViewModel(string name, byte[] content)
{
    public string Name => GetValidFileName(name);

    public byte[] Content { get; } = content;

    public string ContentType => "application/vnd.ms-excel";

    private static string GetValidFileName(string fileName)
        => string.IsNullOrWhiteSpace(fileName)
        ? "no-name.xls"
        : GetInvalidFileNameChars().Aggregate(fileName, (current, c) => current.Replace(c.ToString(), string.Empty)) + ".xls";

    private static IEnumerable<char> GetInvalidFileNameChars()
    {
        var invalidFileNameChars = Path.GetInvalidFileNameChars();

        if (!invalidFileNameChars.Contains('>'))
        {
            return invalidFileNameChars.Concat(['>']);
        }

        return invalidFileNameChars;
    }
}