using Alpha.Announcements.Specs;
using AlphaPlus.Features.Messages.API.Configurations;
using AlphaPlus.Features.Messages.API.Constants;
using AlphaPlus.Features.Messages.API.Extensions;
using Fanex.Logging;
using MediatR;
using AlphaPlus.Features.Messages.API.Models.Announcement;
using AlphaPlus.Features.Messages.API.Models;
using Newtonsoft.Json;
using AlphaPlus.Features.Messages.API.Params.Announcement;
using AlphaPlus.Features.Messages.API.Requests;
using AlphaPlus.Features.Messages.API.Services;
using AlphaPlus.Features.Messages.API.ViewModels;

namespace AlphaPlus.Features.Messages.API.Handlers
{
    public class AnnouncementHandler : IRequestHandler<GetAnnouncementsQuery, IEnumerable<AnnouncementViewModel>>
    {
        private const byte UUSCurrencyId = 20;
        private const string UATEnvironment = "UAT";

        private readonly CustomerService customerService;
        private readonly ProductService productService;
        private readonly AnnouncementService announcementService;
        private readonly AnnouncementFormatService formatAnnouncementService;
        private readonly ILogger logger;

        public AnnouncementHandler(
            CustomerService customerService,
            ProductService productService,
            AnnouncementService announcementService,
            AnnouncementFormatService formatAnnouncementService,
            ILogger logger)
        {
            this.customerService = customerService;
            this.productService = productService;
            this.announcementService = announcementService;
            this.formatAnnouncementService = formatAnnouncementService;
            this.logger = logger;
        }

        public async Task<IEnumerable<AnnouncementViewModel>> Handle(GetAnnouncementsQuery message, CancellationToken cancellation)
        {
            if (AppSettings.EnableLogInfo)
            {
                logger.Info(JsonConvert.SerializeObject(message, Formatting.Indented));
            }

            if (message.Customers == null || !message.Customers.Any())
            {
                return Enumerable.Empty<AnnouncementViewModel>();
            }

            var fromTime = message.FromTime.ToLocalTime();
            var toTime = message.ToTime.ToLocalTime();

            var announcements = await announcementService.GetAnnouncements(fromTime, toTime);

            if (announcements == null || !announcements.Any())
            {
                return Enumerable.Empty<AnnouncementViewModel>();
            }

            var result = new List<AnnouncementViewModel>();

            foreach (var customer in message.Customers)
            {
                var custInfo = customerService.GetCustomerInfo(customer.CustId);

                if (custInfo != null && custInfo.Enabled)
                {
                    var offeredProductIds = productService.GetOfferedProductIdsByCustomer(customer.CustId);
                    var specificationParam = new SpecificationParam(custInfo, offeredProductIds);
                    var announcementParam = CreateAnnouncementParam(specificationParam, fromTime, toTime, customer.Language);

                    var filteredAnnouncements = announcementService.GetAnnouncementsByUser(
                        announcements.Where(a => a.Systems.Contains(custInfo.System, StringComparer.OrdinalIgnoreCase)),
                        announcementParam,
                        toTime);

                    if (AppSettings.Environment.Equals(UATEnvironment))
                    {
                        filteredAnnouncements = FilterCurrencyRule(filteredAnnouncements);
                    }

                    var formatedAnnouncements = formatAnnouncementService.Format(filteredAnnouncements, announcementParam);

                    var custAnnouncement = formatedAnnouncements.Select(a => new AnnouncementViewModel(customer.CustId, a != null ? a.Content : string.Empty));

                    result.AddRange(custAnnouncement);
                }
            }

            if (result.Any() && AppSettings.EnableLogInfo)
            {
                logger.Info(JsonConvert.SerializeObject(result, Formatting.Indented));
            }

            return result;
        }

        private static IEnumerable<AlphaAnnouncement> FilterCurrencyRule(IEnumerable<AlphaAnnouncement> announcements)
        {
            return announcements.Where(ann =>
            {
                foreach (var spec in ann.Specifications)
                {
                    if (spec is CurrencySpecification)
                    {
                        var currencyList = spec.Value as IEnumerable<byte>;

                        if (currencyList != null && currencyList.Any())
                        {
                            return currencyList.Contains(UUSCurrencyId);
                        }
                    }
                }

                return false;
            });
        }

        private static AnnouncementsHistoryParams CreateAnnouncementParam(
            SpecificationParam param,
            DateTime fromDate,
            DateTime toDate,
            string language)
            => new AnnouncementsHistoryParams
            {
                AdminId = param.AdminId,
                CustId = param.UserId,
                Language = language,
                RoleId = param.LevelId,
                Site = param.Site,
                ParentSites = MessageConstants.ParentSitesByLevel[param.LevelId],
                UserName = param.UserName,
                FromDate = fromDate,
                ToDate = toDate,
                SpecificationParamter = param
            };
    }
}