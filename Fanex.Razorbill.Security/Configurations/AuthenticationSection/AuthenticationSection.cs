namespace Fanex.Razorbill.Security.Configurations.AuthenticationSection
{
    using System.Configuration;

    public class AuthenticationSection : ConfigurationSection
    {
        public static AuthenticationSection Instance
        {
            get
            {
                var authenticationSection =
                 ConfigurationManager.GetSection("AuthenticationSection") as AuthenticationSection;
                return authenticationSection;
            }
        }

        [ConfigurationProperty("", IsDefaultCollection = true, IsRequired = true)]
        public GroupLevelCollection GroupLevel
        {
            get
            {
                return (GroupLevelCollection)base[string.Empty];
            }
        }

        [ConfigurationProperty("fullpermission", DefaultValue = "0", IsRequired = false)]
        public string FullPermission
        {
            get { return (string)this["fullpermission"]; }
            set { this["fullpermission"] = value.ToLower(); }
        }
    }
}