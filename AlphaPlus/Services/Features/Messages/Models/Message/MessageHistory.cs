namespace AlphaPlus.Features.Messages.API.Models.Message
{
    using System;
    using AlphaPlus.Features.Messages.Utils;
    using Messages.API.Constants;

    public class MessageHistory : MultiLanguague
    {
        public MessageHistory()
        {
        }

        public MessageHistory(
            int id,
            string contentEN,
            string contentCS,
            string contentCH,
            string contentJP,
            string contentTH,
            string contentKO,
            string contentVI,
            DateTime postTime,
            byte messageType,
            bool isSticky,
            string bettransId,
            long rowNum) : base(contentEN, contentCS, contentCH, contentTH, contentJP, contentKO, contentVI)
        {
            Id = id;
            PostTime = postTime;
            MessageType = messageType;
            IsSticky = isSticky;
            RowNum = rowNum;
            BetTransId = bettransId;
        }

        public int Id { get; set; }

        public DateTime PostTime { get; set; }

        public byte MessageType { get; set; }

        public bool IsSticky { get; set; }

        public string BetTransId { get; set; }

        public int TotalMessasge { get; set; }

        public long RowNum { get; set; }

        public bool HasRedColor => ContentEN.Contains(MessageConstants.ContentFontStartMarkup);

        public string Content(string language)
        {
            var rawContent = GetContentByKey(language);

            if (MessageType == API.Constants.MessageType.Personal)
            {
                rawContent = MessageFormatter.MarkBoldText(rawContent, language);
            }

            return rawContent
                .Replace(MessageConstants.ContentFontStartMarkup, string.Empty)
                .Replace(MessageConstants.ContentFontEndMarkup, string.Empty)
                .Replace("@@", BetTransId);
        }
    }
}