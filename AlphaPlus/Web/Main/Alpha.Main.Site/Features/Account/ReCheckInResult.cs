namespace Alpha.Main.Site.Features.Account
{
    public class ReCheckInResult
    {
        public bool IsKickedOut { get; init; }

        public byte KickOutReason { get; init; } = 0;
    }
}