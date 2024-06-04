using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fanex.Customer.Client.Configurations;

namespace AlphaPlus.Settings.Site.Helpers
{
    public static class CustomerClientConfigurationExtension
    {
        public static CustomerClientConfiguration Initialize(this CustomerClientConfiguration clientConfiguration, IConfiguration configuration)
        {
            clientConfiguration = new CustomerClientConfiguration(
                configuration.GetValue<string>("Fanex.Customer:ServiceUri"),
                configuration.GetValue("Fanex.Customer:SystemId", 1),
                configuration.GetValue("Fanex.Customer:AccessDeniedUrl", "~/User/AccessDenied"));

            return clientConfiguration;
        }
    }
}