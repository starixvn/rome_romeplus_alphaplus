namespace Alpha.Main.Models
{
    public class ReCheckInResult
    {
        public bool IsKickedOut { get; init; }

        public byte KickOutReason { get; init; } = 0;
    }
}