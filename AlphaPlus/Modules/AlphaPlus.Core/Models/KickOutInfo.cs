namespace AlphaPlus.Core.Models;

[Serializable]
public class KickOutInfo(bool isKickedOut, byte kickOutReason = 0)
{

    public bool IsKickedOut { get; } = isKickedOut;

    public byte KickOutReason { get; } = kickOutReason;
}
