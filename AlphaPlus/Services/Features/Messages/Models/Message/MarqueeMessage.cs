namespace AlphaPlus.Features.Messages.API.Models.Message
{
    public class MarqueeMessage : MultiLanguague
    {
        public MarqueeMessage()
        {
        }

        public MarqueeMessage(
            int messageId,
            string contentEN,
            string contentCS,
            string contentCH,
            string contentJP,
            string contentTH,
            string contentKO,
            string contentVi,
            bool isBatch,
            string betTransId,
            int userId,
            bool isSticky,
            int msgType) : base(contentEN, contentCS, contentCH, contentJP, contentTH, contentKO, contentVi)
        {
            Id = messageId;

            BetTransId = betTransId;
            IsBatch = isBatch;
            IsSticky = isSticky;
            MsgType = msgType;
        }

        public int Id { get; set; }

        public string BetTransId { get; set; }

        public bool IsBatch { get; set; }

        public int MsgType { get; set; }

        public bool IsSticky { get; set; }
    }
}