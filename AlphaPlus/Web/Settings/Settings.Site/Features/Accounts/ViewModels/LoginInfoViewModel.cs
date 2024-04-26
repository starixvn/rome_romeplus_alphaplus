namespace AlphaPlus.Settings.Site.Features.Accounts.ViewModels
{
    public class LoginInfoViewModel(
        DateTime createdDate,
        DateTime lastLogin,
        string loginIP)
    {
        public string CreatedDate { get; } = createdDate.ToString();

        public string LastLogin { get; } = DateTime.MinValue == lastLogin ? string.Empty : lastLogin.ToString();

        public string LoginIP { get; } = loginIP;
    }
}