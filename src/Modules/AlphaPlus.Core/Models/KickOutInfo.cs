namespace AlphaPlus.Core.Models;

[Serializable]
public class KickOutInfo
{
    public KickOutInfo() { }

    public KickOutInfo(bool isKickedOut, byte kickOutReason = 0)
    {
        IsKickedOut = isKickedOut;
        KickOutReason = kickOutReason;
    }

    public bool IsKickedOut { get; set; }

    public byte KickOutReason { get; set; }
}
