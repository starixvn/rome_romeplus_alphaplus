namespace AlphaPlus.Settings.Site.ViewModels.IndirectLoginAccount
{
    using AlphaPlus.Core.Helpers;

    public class UpdatePasswordViewModel
    {
        public UpdatePasswordViewModel(int custId, int subAccId, string custName, string hashKey)
        {
            CustId = custId;
            SubAccId = subAccId;
            CustName = custName;
            HashKey = hashKey;
        }

        public int CustId { get; }

        public int SubAccId { get; }

        public string CustName { get; }

        public string HashKey { get; }

        public string RootURL => MicroSites.SettingUrl("~/IndirectLoginAccount/");
    }
}