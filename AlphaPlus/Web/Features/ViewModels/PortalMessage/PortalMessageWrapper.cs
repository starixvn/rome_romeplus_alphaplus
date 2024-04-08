namespace AlphaPlus.Features.Site.Messages.ViewModels
{
    using System;
    using AlphaPlus.Features.Messages.API.Constants;
    using AlphaPlus.Features.Messages.API.Models.Message;
    using AlphaPlus.Features.Messages.Utils;

    public class PortalMessageWrapper
    {
        private const int DefaultMaxContentLength = 100;
        private const string Ellipsis = "...";

        private readonly string currentLang;

        public PortalMessageWrapper(PortalMessage noticeMessage, string currentLang)
        {
            this.currentLang = currentLang;
            CurrentMessage = noticeMessage;
            PostTime = CurrentMessage.PostTime;
        }

        public PortalMessage CurrentMessage { get; }

        public DateTime PostTime { get; }

        public string MessageType
        {
            get
            {
                var msgType = CurrentMessage.MessageType;
                var isSticky = CurrentMessage.IsSticky;

                if (isSticky)
                {
                    if (msgType == DataBaseMessageType.Personal || msgType == DataBaseMessageType.Others)
                    {
                        return "special";
                    }
                }
                else
                {
                    if (msgType == DataBaseMessageType.Personal)
                    {
                        return "private";
                    }

                    if (msgType == DataBaseMessageType.Others)
                    {
                        return "public";
                    }
                }

                return string.Empty;
            }
        }

        public string ShortContent
        {
            get
            {
                int length = Math.Min(DefaultMaxContentLength, Content.Length);

                return Content.Substring(0, length) + Ellipsis;
            }
        }

        private string content;
        public string Content
        {
            get
            {
                if (content == null)
                {
                    content = CurrentMessage.GetContentByKey(currentLang)
                        .Replace("@@", CurrentMessage.BetTransId)
                        .Replace(MessageConstants.ContentFontStartMarkup, string.Empty)
                        .Replace(MessageConstants.ContentFontEndMarkup, string.Empty);
                }

                return content;
            }
        }

        public string BoldContent => MessageFormatter.MarkBoldText(Content, currentLang);

        public string GetStyleMessage
            => CurrentMessage.GetContentByKey(currentLang).Contains(MessageConstants.ContentFontEndMarkup) ?
            "text-red" :
            string.Empty;
    }
}