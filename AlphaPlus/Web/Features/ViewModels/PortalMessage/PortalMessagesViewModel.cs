namespace AlphaPlus.Features.Site.Messages.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AlphaPlus.Features.Messages.API.Constants;

    public class PortalMessagesViewModel
    {
        public PortalMessagesViewModel(List<PortalMessageWrapper> messages)
        {
            NormalMessages = messages
                .Where(m => m.CurrentMessage.TopLevel >= (byte)MessageRange.Normal)
                .OrderByDescending(m => m.CurrentMessage.PostTime);

            HighlightMessages = messages
                .Where(m => m.CurrentMessage.TopLevel <= (byte)MessageRange.Highlight)
                .OrderBy(m => m.CurrentMessage.TopLevel)
                .ThenByDescending(m => m.PostTime);

            LastMessagePostTime = messages.Any() ? messages.Last().PostTime : DateTime.MinValue;
        }

        public IEnumerable<PortalMessageWrapper> NormalMessages { get; }

        public IEnumerable<PortalMessageWrapper> HighlightMessages { get; }

        public DateTime LastMessagePostTime { get; }
    }
}