using AlphaPlus.Core.Extensions;
using AlphaPlus.Core.Models;
using AlphaPlus.Reports.Features.Customer;
using AlphaPlus.Reports.Features.EnteredResult;
using AlphaPlus.Reports.Features.Shared;
using AlphaPlus.Reports.Features.Shared.Models;
using AlphaPlus.Reports.Features.WinLossDetail;
using AlphaPlus.Reports.Site.Features.Shared;
using MediatR;
using Microsoft.Extensions.Options;
using AlphaPlus.Shared.AppResources.Report;

namespace AlphaPlus.Reports.Site.Features.WinLossDetail;

public class WinLossDetailNewHandler(
    IProductService productService,
    IOptions<AppSettings> options,
    IEnteredResultService enteredResultService,
    ICustomerService customerService,
    IWinLossDetailService winlossDetailService,
    IWinLossDetailWorkbook workbook) :
        BaseHandler(productService, options),
        IRequestHandler<WinLossDetailNewQuery, WinLossDetailNewViewModel>,
        IRequestHandler<WinLossDetailGetDataQuery, WinLossDetailGetDataViewModel>,
        IRequestHandler<WinLossDetailGetWeeksQuery, IEnumerable<WinLossDetailWeekViewModel>>,
        IRequestHandler<WinLossDetailExcelNewQuery, ExcelFileViewModel>
{
    public async Task<WinLossDetailNewViewModel> Handle(WinLossDetailNewQuery message, CancellationToken cancellationToken)
    {
        var (productViewModel, _) = BuildProductViewModel(message, message.UserSelectedProductIds, message.UserSelectedResourceIds);

        var (isValidUsername, breadcrumbs) = await BuildWinLossDetailGetDataQuery(message);
        var weekDataTask = Task.FromResult<IEnumerable<WinLossDetailWeekModel>>([]);
        var winlossDetailTask = Task.FromResult(new ReportData<WinLossDetailModel>(null, null, 0));

        if (isValidUsername)
        {
            var (validUserSelectedProductIds, productDetailXml) = FilterProductAndProductDetailIds(
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
                message.Site);
            weekDataTask = winlossDetailService.GetWeekData(weekDataParam);
        }

        var enteredResultTask = enteredResultService.GetTotalEnteredResults(message.Site);
        await Task.WhenAll(enteredResultTask, winlossDetailTask);

        return new WinLossDetailNewViewModel(
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
            var (validUserSelectedProductIds, productDetailXml) = FilterProductAndProductDetailIds(
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
            message.CustLevel,
            (message.WeekFromDate ?? message.FromDate).GetGreaterDate(message.LimitedDateToShowInfo),
            message.WeekToDate ?? message.ToDate,
            validUserSelectedProductIds,
            productDetailXml,
            message.Username,
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
                var breadcrumb = customerService.BuildBreadcrumb(customerLevel);

                // var nickname = message.ViewerLevel.IsDirectUplineOf(customerLevel.RoleId)
                //     ? customerLevel.Username2
                //     : string.Empty;
                // customerService. (
                //     message.ViewerId,
                //     customerLevel.Username,
                //     customerLevel.FirstName,
                //     customerLevel.LastName,
                //     nickname);

                message.CustId = breadcrumb.CustId;
                message.CustName = breadcrumb.CustName;
                message.CustLevelId = breadcrumb.CustLevelId;
                breadcrumbs = customerService.BuildBreadcrumbs(customerLevel, message.ViewerLevel).ToList();
            }
            else
            {
                breadcrumbs = [new Breadcrumb(message.ViewerId, message.CustName, message.ViewerLevelId)];
            }
        }

        return (isValidUsername, breadcrumbs ?? BuildBreadcrumbViewModel(message).Breadcrumbs);
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
            var (validUserSelectedProductIds, productDetailXml) = FilterProductAndProductDetailIds(
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
                message.Site);
            var weekData = await winlossDetailService.GetWeekData(weekDataParam);

            return BuildWeekViewModels(
                weekData,
                weekDataParam.FromDate,
                weekDataParam.ToDate,
                message.WeekFromDate,
                message.WeekToDate);
        }

        return BuildWeekViewModels(
            [],
            message.FromDate.GetGreaterDate(message.LimitedDateToShowInfo),
            message.ToDate,
            message.WeekFromDate,
            message.WeekToDate);
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

    public async Task<ExcelFileViewModel> Handle(WinLossDetailExcelNewQuery message, CancellationToken cancellationToken)
    {
        var (validUserSelectedProductIds, productDetailXml) = FilterProductAndProductDetailIds(
            message,
            message.UserSelectedProductIds,
            message.UserSelectedResourceIds);
        var reportData = await GetWinLossDetail(message, validUserSelectedProductIds.JoinWithComma(), productDetailXml);
        var productNames = productService.ConcatProductNames(validUserSelectedProductIds);

        return workbook.BuildExcelFile(message, reportData, productNames);
    }
}