namespace AlphaPlus.Core.Authorization.Models;

[Serializable]
public class KickOutInfo
{
    public KickOutInfo() { }

    public KickOutInfo(bool isKickedOut, byte kickOutReason = 0)
    {
        IsKickedOut = isKickedOut;
        KickOutReason = kickOutReason;
    }

    public bool IsKickedOut { get; init; }

    public byte KickOutReason { get; init; }
}