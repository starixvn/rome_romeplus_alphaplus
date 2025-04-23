using MediatR;
using AlphaPlus.Settings.MemberInfo.Domain.Configurations;
using AlphaPlus.Settings.Site.Features.Shared;
using Fanex.Customer.Client.Models;

namespace AlphaPlus.Settings.Site.Features.CopyAccount.Request
{
    public class CopyAccountCommand : BaseQuery, IRequest<CopyMultipleCustomersResult>
    {
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

        public string SiteDBGroupIp => viewer.SiteDBGroupIp;

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