using Fanex.Customer.Client.Configurations;

namespace AlphaPlus.Settings.Site.Features.Helpers
{
    public static class CustomerClientConfigurationExtension
    {
        public static CustomerClientConfiguration Initialize(this CustomerClientConfiguration clientConfiguration, IConfiguration configuration)
        {
            clientConfiguration = new CustomerClientConfiguration(
                configuration.GetValue<string>("Fanex.Customer:ServiceUri"),
                configuration.GetValue("Fanex.Customer:SystemId", 2),
                configuration.GetValue("Fanex.Customer:AccessDeniedUrl", "~/User/AccessDenied"));

            return clientConfiguration;
        }
    }
}