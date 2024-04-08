using AlphaPlus.Core.Extensions;
using AlphaPlus.Core.Helpers;
using AlphaPlus.Core.Models;
using AlphaPlus.RequestInfo;
using Fanex.Caching;
using Fanex.Data;
using Fanex.Data.Repository;
using Fanex.RemotingRef;
using Microsoft.AspNetCore.Authentication.Cookies;
using MediatR;
using AlphaPlus.Features.Messages.API.Services;
using AlphaPlus.Features.Messages.API.Client;
using RestSharp;
using Alpha.Announcements.Services;
using AlphaPlus.Features.Messages.API.Models.Announcement;
using Alpha.Announcements.Models;
using AlphaPlus.Features.Site.Messages.Requests.Announcement;
using AlphaPlus.Features.Messages.API.Configurations;
using AlphaPlus.Features.Messages.API.Client.Helpers;

namespace AlphaPlus.Features.Site.Helpers
{
    public static class ApplicationServicesHelper
    {
        public static IServiceCollection RegisterApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddHealthChecks();
                
            services
                .AddControllersWithViews()
                .AddNewtonsoftJson(option => 
                {
                    option.UseMemberCasing();
                })
                .AddRazorOptions(options =>
                {
                    options.ViewLocationFormats.Clear();
                    options.ViewLocationFormats.Add("/Views/{1}/{0}.cshtml");
                    options.ViewLocationFormats.Add("/Views/Shared/{0}.cshtml");
                });
            
            services
                .AddMediatR(typeof(Program).Assembly)
                .AddFanexData(configuration)
                .AddFanexLogging(configuration)
                .AddRemotingRef(configuration)
                .AddSingleton<IDynamicRepository, DynamicRepository>();

            services
                .AddSingleton<IRemotingRefService, RemotingRefService>()
                .AddSingleton<ISessionManager, SessionManager>();

            services
                .AddSingleton<Utilities.IP.IPService, Utilities.IP.IPServiceImpl>()
                .AddSingleton<IPService, IPServiceImpl>()
                .AddSingleton<RequestInfo.IPHelper, IPHelperImpl>()
                .AddSingleton<RequestInfo.IPHelper, IPHelperImpl>()
                .AddSingleton<IBrowserHelper, BrowserHelper>()
                .AddSingleton<IDeviceHelper, DeviceHelper>()
                .AddScoped<IUserSession, UserSessionExt>();

            services
                .AddScoped<IParam, SpecificationParameter>()
                .AddScoped<Func<int, bool>>((provider) => UserPermission.HasPermissionOnProduct)
                .AddScoped<ResourceManager, SiteResourceManager>()
                .AddScoped<SpecificationService, SpecificationServiceImpl>()
                .AddScoped<AnnouncementFormatService, AnnouncementFormatServiceImpl>()
                .AddScoped<AnnouncementService, AnnouncementServiceImpl>()
                .AddScoped<AnnouncementFormatService, AnnouncementFormatServiceImpl>()
                .AddScoped<BackupDomainsService, BackupDomainsServiceImpl>()
                .AddScoped<CustomerService, CustomerServiceImpl>()
                .AddScoped<FeedbackService, FeedbackServiceImpl>()
                .AddScoped<MarqueeMessageService, MarqueeMessageServiceImpl>()
                .AddScoped<MessageHistoryService, MessageHistoryServiceImpl>()
                .AddScoped<NewMessageService, NewMessageServiceImpl>()
                .AddScoped<PortalMessageService, PortalMessageServiceImpl>()
                .AddScoped<ProductService, ProductServiceImpl>()
                .AddScoped<ToastMessageService, ToastMessageServiceImpl>()
                .AddScoped<AnnouncementClient, AnnouncementClientImpl>()
                .AddSingleton<IRestClient>(client => new RestClient(new Uri(AppSettings.AnnouncementApi)) { Authenticator = new HmacAuthenticator() })
                .AddSingleton<ICacheService, CacheService>();

            services
                .AddSharedSession(configuration)
                .AddAuthorization()
                .AddHttpContextAccessor()
                .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddSharedCookie();

            return services;
        }
    }
}