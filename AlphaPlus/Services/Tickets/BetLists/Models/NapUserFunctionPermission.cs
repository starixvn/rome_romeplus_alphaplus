namespace AlphaPlus.BetLists.Models;

public class NapUserFunctionPermission
{
    public int GroupId { get; init; }

    public string FileNames { get; init; }

    public string FuncName { get; init; }

    public byte ActualPolicy { get; init; }
}
