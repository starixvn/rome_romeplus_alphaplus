using AlphaPlus.Core.Extensions;
using AlphaPlus.Core.Models;
using AlphaPlus.Reports.Features.Customer;
using AlphaPlus.Reports.Features.EnteredResult;
using AlphaPlus.Reports.Features.Shared.Models;
using AlphaPlus.Reports.Features.WinLossDetail;
using AlphaPlus.Reports.Site.Features.Shared;
using MediatR;
using Microsoft.Extensions.Options;
using AlphaPlus.Shared.AppResources.Report;
using AlphaPlus.Reports.Site.Features.Customer;
using AlphaPlus.Reports.Features.Products;
using AlphaPlus.Reports.Site.Features.Shared.Helpers;

namespace AlphaPlus.Reports.Site.Features.WinLossDetail;

public class WinLossDetailHandler(
    IProductService productService,
    IOptions<AppSettings> options,
    IEnteredResultService enteredResultService,
    ICustomerService customerService,
    IWinLossDetailService winlossDetailService,
    IWinLossDetailWorkbook workbook,
    ICustomerClient customerClient) :
        BaseHandler(productService, options),
        IRequestHandler<WinLossDetailQuery, WinLossDetailViewModel>,
        IRequestHandler<WinLossDetailGetDataQuery, WinLossDetailGetDataViewModel>,
        IRequestHandler<WinLossDetailGetWeeksQuery, IEnumerable<WinLossDetailWeekViewModel>>,
        IRequestHandler<WinLossDetailExcelQuery, ExcelFileViewModel>
{
    private readonly IEnteredResultService enteredResultService = enteredResultService;
    private readonly ICustomerService customerService = customerService;
    private readonly IWinLossDetailService winlossDetailService = winlossDetailService;
    private readonly IWinLossDetailWorkbook workbook = workbook;
    private readonly ICustomerClient customerClient = customerClient;

    public async Task<WinLossDetailViewModel> Handle(WinLossDetailQuery message, CancellationToken cancellationToken)
    {
        var (productViewModel, _) = await BuildProductViewModel(message, message.UserSelectedProductIds, message.UserSelectedResourceIds);

        var (isValidUsername, breadcrumbs) = await BuildWinLossDetailGetDataQuery(message);
        var weekDataTask = Task.FromResult<IEnumerable<WinLossDetailWeekModel>>([]);
        var winlossDetailTask = Task.FromResult(new ReportData<WinLossDetailModel>(null, null, 0));

        if (isValidUsername)
        {
            var (validUserSelectedProductIds, productDetailXml) = await FilterProductAndProductDetailIds(
                message,
                message.UserSelectedProductIds,
                message.UserSelectedResourceIds);
            winlossDetailTask = GetWinLossDetail(message, validUserSelectedProductIds.JoinWithComma(), productDetailXml);
            var weekDataParam = new WinLossDetailWeekParam(
                message.ViewerId,
                message.FromDate.GetGreaterDate(message.LimitedDateToShowInfo),
                message.ToDate,
                validUserSelectedProductIds.JoinWithComma(),
                productDetailXml,
                string.IsNullOrWhiteSpace(message.Username) ? message.CustName : message.Username,
                IsReward: true,
                message.LevelType,
                message.Site);
            weekDataTask = winlossDetailService.GetWeekData(weekDataParam);
        }

        var enteredResultTask = enteredResultService.GetTotalEnteredResults(message.Site);
        await Task.WhenAll(enteredResultTask, winlossDetailTask);

        return new WinLossDetailViewModel(
            message,
            await enteredResultTask,
            await winlossDetailTask,
            breadcrumbs,
            productViewModel,
            BuildWeekViewModels(await weekDataTask, message.FromDate, message.ToDate, message.WeekFromDate, message.WeekToDate));
    }

    public async Task<WinLossDetailGetDataViewModel> Handle(WinLossDetailGetDataQuery message, CancellationToken cancellationToken)
    {
        var (isValidUsername, breadcrumbs) = await BuildWinLossDetailGetDataQuery(message);
        var winlossDetailTask = Task.FromResult(new ReportData<WinLossDetailModel>(null, null, 0));

        if (isValidUsername)
        {
            var (validUserSelectedProductIds, productDetailXml) = await FilterProductAndProductDetailIds(
                message,
                message.UserSelectedProductIds,
                message.UserSelectedResourceIds);
            winlossDetailTask = GetWinLossDetail(message, validUserSelectedProductIds.JoinWithComma(), productDetailXml);
        }

        var enteredResultTask = enteredResultService.GetTotalEnteredResults(message.Site);
        await Task.WhenAll(enteredResultTask, winlossDetailTask);

        return new WinLossDetailGetDataViewModel(
            message,
            await enteredResultTask,
            await winlossDetailTask,
            breadcrumbs);
    }

    private Task<ReportData<WinLossDetailModel>> GetWinLossDetail(
        WinLossDetailGetDataQuery message,
        string validUserSelectedProductIds,
        string productDetailXml)
    {
        var param = new WinLossDetailParam(
            message.CustId,
            message.CustLevel.Value,
            (message.WeekFromDate ?? message.FromDate).GetGreaterDate(message.LimitedDateToShowInfo),
            message.WeekToDate ?? message.ToDate,
            validUserSelectedProductIds,
            productDetailXml,
            message.Username,
            IsReward: true,
            message.LevelType,
            message.Site);

        return winlossDetailService.GetWinLossDetail(param);
    }

    private async Task<(bool, IEnumerable<Breadcrumb>)> BuildWinLossDetailGetDataQuery(WinLossDetailGetDataQuery message)
    {
        var isValidUsername = true;
        IEnumerable<Breadcrumb> breadcrumbs = null;

        if (!string.IsNullOrWhiteSpace(message.Username))
        {
            var customerLevel = await customerService.GetCustomerLevel(message.ViewerId, message.Username);
            isValidUsername = customerLevel != null;

            if (isValidUsername)
            {
                if (!message.IsInternal)
                {
                    _ = Task.Run(async () =>
                    {
                        var nickname = message.ViewerLevel.IsDirectUplineOf(customerLevel.RoleId)
                            ? customerLevel.Username2
                            : string.Empty;
                        await customerClient.UpdateUserSearch(
                            message.ViewerId,
                            customerLevel.Username,
                            customerLevel.FirstName,
                            customerLevel.LastName,
                            nickname);
                    });
                }

                var breadcrumb = customerService.BuildBreadcrumb(customerLevel);
                message.CustId = breadcrumb.CustId;
                message.CustName = breadcrumb.CustName;
                message.CustLevelId = breadcrumb.CustLevelId;
                breadcrumbs = customerService.BuildBreadcrumbs(customerLevel, message.ViewerLevel.Value).ToList();
            }
            else
            {
                breadcrumbs = [new Breadcrumb(message.ViewerId, message.CustName, message.ViewerLevelId)];
            }
        }

        return (isValidUsername, breadcrumbs ?? BreadcrumbHelper.BuildBreadcrumbs(message));
    }

    public async Task<IEnumerable<WinLossDetailWeekViewModel>> Handle(WinLossDetailGetWeeksQuery message, CancellationToken cancellationToken)
    {
        var isValidUsername = true;

        if (!string.IsNullOrWhiteSpace(message.Username))
        {
            var customerLevel = await customerService.GetCustomerLevel(message.ViewerId, message.Username);
            isValidUsername = customerLevel != null;
        }

        if (isValidUsername)
        {
            var (validUserSelectedProductIds, productDetailXml) = await FilterProductAndProductDetailIds(
                message,
                message.UserSelectedProductIds,
                message.UserSelectedResourceIds);

            var weekDataParam = new WinLossDetailWeekParam(
                message.ViewerId,
                message.FromDate.GetGreaterDate(message.LimitedDateToShowInfo),
                message.ToDate,
                validUserSelectedProductIds.JoinWithComma(),
                productDetailXml,
                string.IsNullOrWhiteSpace(message.Username) ? message.CustName : message.Username,
                IsReward: true,
                message.LevelType,
                message.Site);
            var weekData = await winlossDetailService.GetWeekData(weekDataParam);

            return BuildWeekViewModels(
                weekData,
                weekDataParam.FromDate,
                weekDataParam.ToDate,
                message.WeekFromDate,
                message.WeekToDate);
        }

        return [];
    }

    private static IEnumerable<WinLossDetailWeekViewModel> BuildWeekViewModels(
        IEnumerable<WinLossDetailWeekModel> weekData,
        DateTime fromDate,
        DateTime toDate,
        DateTime? selectedFromDate = null,
        DateTime? selectedToDate = null)
    {
        var weekDataHash = new HashSet<string>();
        foreach (var item in weekData)
        {
            weekDataHash.Add($"{item.StartOfWeek.ToShortDateWithSlash()}-{item.EndOfWeek.ToShortDateWithSlash()}");
        }

        var weekSelections = new List<WinLossDetailWeekViewModel>();
        var startDate = fromDate.Date;
        var endDate = toDate.Date;

        while (endDate.Subtract(startDate).TotalDays >= 0)
        {
            var endDateStartOfWeek = endDate.StartOfWeek();
            var startDateOrEndDateStartOfWeek = startDate < endDateStartOfWeek ? endDateStartOfWeek : startDate;

            var startDateOrEndDateStartOfWeekString = startDateOrEndDateStartOfWeek.ToShortDateWithSlash();
            var endDateString = endDate.ToShortDateWithSlash();
            var weekSelection = new WinLossDetailWeekViewModel(
                startDateOrEndDateStartOfWeekString,
                endDateString,
                GetDisplayWeek(startDateOrEndDateStartOfWeek, endDate),
                weekDataHash.Contains($"{startDateOrEndDateStartOfWeekString}-{endDateString}"),
                selectedFromDate.HasValue
                && selectedToDate.HasValue
                && selectedFromDate.Value == startDateOrEndDateStartOfWeek
                && selectedToDate.Value == endDate);
            weekSelections.Add(weekSelection);

            endDate = endDateStartOfWeek.AddDays(-1);
        }

        return weekSelections;
    }

    private static string GetDisplayWeek(DateTime fromDate, DateTime toDate)
    {
        var displayToDate = $"{toDate.ToString("dd")} {Resources.ResourceManager.GetString($"_{toDate.ToString("MMM")}")}";

        return fromDate == toDate ? displayToDate : $"{fromDate.ToString("dd")} - {displayToDate}";
    }

    public async Task<ExcelFileViewModel> Handle(WinLossDetailExcelQuery message, CancellationToken cancellationToken)
    {
        var isValidUsername = true;

        if (!string.IsNullOrWhiteSpace(message.Username))
        {
            var customerLevel = await customerService.GetCustomerLevel(message.ViewerId, message.Username);
            isValidUsername = customerLevel != null;

            if (isValidUsername)
            {
                var breadcrumb = customerService.BuildBreadcrumb(customerLevel);
                message.CustId = breadcrumb.CustId;
                message.CustName = breadcrumb.CustName;
                message.CustLevelId = breadcrumb.CustLevelId;
            }
        }

        var (validUserSelectedProductIds, productDetailXml, productNames) = await FilterProductAndProductDetailIdsForExcel(
            message,
            message.UserSelectedProductIds,
            message.UserSelectedResourceIds);
        var reportData = isValidUsername
            ? await GetWinLossDetail(message, validUserSelectedProductIds.JoinWithComma(), productDetailXml)
            : new ReportData<WinLossDetailModel>(null, null, 0);

        return workbook.BuildExcelFile(message, reportData, productNames);
    }
}