namespace AlphaPlus.Settings.MemberInfo.Domain.Features.Shared.Models
{
    public class UsernameSuggestion
    {
        public UsernameSuggestion()
        {
        }

        public UsernameSuggestion(string userName, string firstName, string lastName, string username2)
        {
            UserName = userName;
            FirstName = firstName;
            LastName = lastName;
            UserName2 = username2;
        }

        public string UserName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string UserName2 { get; set; }
    }
}