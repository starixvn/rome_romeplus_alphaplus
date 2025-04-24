namespace AlphaPlus.Settings.Site.Features.Customer.ViewModels
{
    public class AppConfiguration(string appPath, int systemId = 1)
    {
        public string ApplicationPath { get; } = appPath;

        public int SystemId { get; } = systemId;
    }
}