namespace AlphaPlus.Identity.API.Features.Auth
{
    public class ReCheckInResponse
    {
        public bool IsKickedOut { get; init; }

        public byte KickOutReason { get; init; }
    }
}