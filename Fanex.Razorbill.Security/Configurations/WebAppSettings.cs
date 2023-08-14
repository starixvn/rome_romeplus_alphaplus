namespace Fanex.Razorbill.Security.Configurations
{
    using System.Configuration;

    public class WebAppSettings
    {
        public static string VirtualDirectory
        {
            get
            {
                return ConfigurationManager.AppSettings["VirtualDirectory"];
            }
        }
    }
}