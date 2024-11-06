using AlphaPlus.Core.Models;
using MediatR;
using AlphaPlus.Features.Site.Messages.ViewModels;
using AlphaPlus.Features.Messages.API.Configurations;

namespace AlphaPlus.Features.Site.Messages.Requests.MarqueeMessage
{
    public class MarqueeMessageQuery : IRequest<MarqueeMessageViewModel>
    {
        private readonly bool? isTest;
        private readonly IUserSession userSession;

        public MarqueeMessageQuery(IUserSession userSession)
        {
            CustId = userSession.CustId;
            CurrentLang = userSession.Language;
            RoleId = userSession.RoleId;
            this.userSession = userSession;
        }

        public MarqueeMessageQuery(
            IUserSession userSession,
            bool? isTest)
        {
            CustId = userSession.CustId;
            CurrentLang = userSession.Language;
            RoleId = userSession.RoleId;
            this.isTest = isTest;
            this.userSession = userSession;
        }

        public int CustId { get; }

        public bool IsTest
            => this.isTest ?? AppSettings.LocalTest;

        public string CurrentLang { get; }

        public byte RoleId { get; }

        public string UserName => userSession.UserName;

        public bool IsAdminUser => userSession.AdminId != 0;

        public string CustomerSite => userSession.Site;
    }
}