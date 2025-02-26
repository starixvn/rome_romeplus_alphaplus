namespace AlphaPlus.Features.Messages.API.Models.Message
{
    public class PortalMessage : MultiLanguague
    {
        public PortalMessage()
        {
        }

        public PortalMessage(
            int id,
            string contentEN,
            string contentCS,
            string contentCH,
            string contentJP,
            string contentTH,
            string contentKO,
            string contentVI,
            DateTime postTime,
            byte type,
            bool isSticky,
            string betTransId,
            bool isRead,
            byte topLevel) : base(contentEN, contentCS, contentCH, contentTH, contentJP, contentKO, contentVI)
        {
            Id = id;
            PostTime = postTime;
            MessageType = type;
            IsSticky = isSticky;
            BetTransId = betTransId;
            IsRead = isRead;
            TopLevel = topLevel;
        }

        public int Id { get; set; }

        public DateTime PostTime { get; set; }

        public byte MessageType { get; set; }

        public bool IsSticky { get; set; }

        public string BetTransId { get; } = string.Empty;

        public bool IsRead { get; set; }

        public byte TopLevel { get; set; }
    }
}