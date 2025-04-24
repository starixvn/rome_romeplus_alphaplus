using Fanex.Customer.Client.Models;

namespace AlphaPlus.Settings.Site.Features.CopyAccount.ViewModels
{
    public class CopyAccountResultViewModel
    {
        public CopyMultipleCustomersResult Result { get; set; }

        public decimal Credit { get; set; }

        public int NumberOfAccounts { get; set; }

        public int NoOfFailedAccount { get; set; }
    }
}