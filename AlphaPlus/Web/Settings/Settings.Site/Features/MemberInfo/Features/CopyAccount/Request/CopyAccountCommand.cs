using AlphaPlus.Settings.Site.Features.MemberInfo.Requests;
using AlphaPlus.Settings.Site.Features.MemberInfo.Features.CopyAccount.ViewModels;
using MediatR;
using AlphaPlus.Settings.MemberInfo.Domain.Models;
using AlphaPlus.Settings.MemberInfo.Domain.Configurations;
using AlphaPlus.Settings.Site.Features.MemberInfo.Models;
using AlphaPlus.Core.Models;

namespace AlphaPlus.Settings.Site.Features.MemberInfo.Features.CopyAccount.Request
{
    public class CopyAccountCommand : MemberInfoBaseQuery, IRequest<CopyAccountResponse>
    {
        public CopyAccountCommand() : this(null){}
        
        public CopyAccountCommand(IUser viewer) : base(viewer)
        {
            viewer = viewer ?? new User(UserSession.Current);
            SiteDBGroupIp = viewer.SiteDBGroupIp;
        }

        public string SourceUsername { get; set; }

        public int NumberOfAccounts { get; set; }

        public string Username => $"{CustName}{FirstSuffix}{SecondSuffix}{ThirdSuffix}";

        public string FirstSuffix { get; set; }

        public string SecondSuffix { get; set; }

        public string ThirdSuffix { get; set; }

        public decimal Credit { get; set; }

        public decimal CreditMin { get; set; }

        public decimal CreditMax { get; set; }

        public decimal MemberMaxCredit { get; set; }

        public string Password { get; set; }

        public string SiteDBGroupIp { get; }

        public override bool IsValid() =>
            base.IsValid() &&
            !IsInternal &&
            !string.IsNullOrEmpty(SourceUsername) &&
            !string.IsNullOrEmpty(Password) &&
            Credit <= CreditMax &&
            Credit >= CreditMin &&
            NumberOfAccounts > 0 &&
            NumberOfAccounts <= AppSettings.MaxAccountsCreatePerRequest;
    }
}