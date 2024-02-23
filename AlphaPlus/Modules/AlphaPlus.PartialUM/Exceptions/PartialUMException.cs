namespace AlphaPlus.PartialUM.Exceptions;

public class PartialUMException(string umFrom, string umTo) : Exception
{
    public string UMFrom { get; } = umFrom;

    public string UMTo { get; } = umTo;
}
