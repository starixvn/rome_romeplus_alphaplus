namespace AlphaPlus.Settings.Transfer.API.Criteria.TransferCondition;

public class ToastContentDto(string title, string body)
{
    public string Title { get; } = title;

    public string Body { get; } = body;
}