namespace AlphaPlus.Main.Site.Features.UMMessage
{
    public class UMMessageDTO(long id, string message, CancellationToken cancellation)
    {
        public long Id { get; } = id;

        public string Message { get; } = message;
    }
}
