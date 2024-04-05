namespace AlphaPlus.Features.Messages.Site.Requests.PortalMessage
{
    using MediatR;
    using Messages.Constants;
    using AlphaPlus.Features.Site.Messages.ViewModels;
    using AlphaPlus.Core.Models;

    public class PortalMessageQuery : IRequest<PortalMessagesViewModel>
    {
        public PortalMessageQuery(
            IUserSession userSession,
            int totalMessage,
            bool isTest)
        {
            Level = userSession.RoleId;
            CustId = userSession.CustId;
            CurrentLang = userSession.Language;
            TotalMessage = totalMessage;
            IsTest = isTest;
        }

        public byte Level { get; }

        public int CustId { get; }
        
        public int TotalMessage { get; }

        public bool IsTest { get; }

        public string CurrentLang { get; }

        public string ParentSites => MessageConstants.ParentSitesByLevel[Level];        
    }
}