namespace AlphaPlus.Features.Messages.API.Models.Message
{
    public class GeneralMarqueeMessage
    {
        private const int NormalMessageDB = 3;

        public GeneralMarqueeMessage()
        {
            Id = 0;
            Content = string.Empty;
        }

        public GeneralMarqueeMessage(
            int id,
            string content,
            string betTransId,
            bool isBatch,
            bool isSticky,
            int msgTypeFromDB)
        {
            Id = id;
            Content = content.Replace("\n", string.Empty)
                .Replace("\r", string.Empty)
                .Replace("'", "\'");

            if (isBatch)
            {
                Content = Content.Replace("@@", betTransId);
            }

            MessageType = GetMessageType(isSticky, msgTypeFromDB);
        }

        public int Id { get; }

        public string Content { get; }

        public int MessageType { get; }

        private static int GetMessageType(bool isSticky, int messageType)
        {
            if (isSticky)
            {
                return API.Constants.MessageType.Special;
            }

            return messageType == NormalMessageDB ? API.Constants.MessageType.Normal : API.Constants.MessageType.Personal;
        }
    }
}