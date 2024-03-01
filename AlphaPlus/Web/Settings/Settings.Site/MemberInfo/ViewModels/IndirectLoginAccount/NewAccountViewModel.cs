namespace Alpha.MemberInfo.Site.ViewModels.IndirectLoginAccount
{
    public class NewAccountViewModel
    {
        public NewAccountViewModel()
        {
        }

        public NewAccountViewModel(string hashKey)
        {
            HashKey = hashKey;
        }

        public string CustName { get; set; }

        public string Password { get; set; }

        public string HashKey { get; }
    }
}