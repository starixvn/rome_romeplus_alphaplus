using AlphaPlus.Core.Extensions;
using AlphaPlus.Core.Models;
using AlphaPlus.Shared.AppResources.Report;
using AlphaPlus.Reports.Features.Shared.Models;
using AlphaPlus.Reports.Features.WhiteLabel;
using AlphaPlus.Reports.Site.Features.Shared;
using ONEbook.SpreadSheets;
using static ONEbook.SpreadSheets.Column;

namespace AlphaPlus.Reports.Site.Features.WhiteLabel;

public interface ISettlementWorkbook
{
    ExcelFileViewModel BuildExcelFile(SettlementExcelQuery message, ReportData<SettlementModel> reportData);
}

public class SettlementWorkbook(IWebHostEnvironment webHostEnvironment) :
    ReportNpoiWorkbook<SettlementModel>(webHostEnvironment),
    ISettlementWorkbook
{
    public ExcelFileViewModel BuildExcelFile(SettlementExcelQuery message, ReportData<SettlementModel> reportData)
    {
        var fromDate = message.FromDate.ToMMYYYYWithSlash();
        var toDate = message.ToDate.ToMMYYYYWithSlash();

        var fileContent = UseTemplate(
            new TemplateInfo(
                GetExcelTemplatePath($"Settlement{message.ViewerLevel.GetDirectDownLineLevel().DisplayName}"),
                noInfomationText: Resources.NoInformationIsAvailable))
            .Title($"{Resources.SettlementReport} {fromDate} --> {toDate}")
            .Header(Resources.ResourceManager.GetString)
            .Body(reportData.DataSource, GetRowMapper(message, false))
            .Footer(reportData.Total, GetRowMapper(message, true))
            .Sheet(Resources.DefaultSheetName)
            .ExportToBytes();

        return new ExcelFileViewModel($"{Resources.SettlementReport}-{message.CustName}-{fromDate}-{toDate}", fileContent);
    }

    internal static Mapper<SettlementModel> GetRowMapper(SettlementExcelQuery message, bool isTotal)
    {
        if (message.ViewerLevel.IsSuperLevel())
        {
            return GetSuperRowMapper(isTotal);
        }

        if (message.ViewerLevel.IsMasterLevel())
        {
            return GetMasterRowMapper(isTotal);
        }

        return GetAgentRowMapper(isTotal);
    }

    internal static Mapper<SettlementModel> GetSuperRowMapper(bool isTotal)
    {
        var mapper = new Mapper<SettlementModel>();
        if (isTotal)
        {
            mapper.Map(A, settlement => Resources.Total);
        }
        else
        {
            mapper
                .Map(A, settlement => settlement.Month)
                .Map(B, settlement => settlement.WhiteLabel)
                .Map(C, settlement => settlement.Agent)
                .Map(D, settlement => settlement.Master);
        }

        return mapper
            .Map(E, settlement => settlement.MemberWL)
            .Map(F, settlement => settlement.MemberPromotion)
            .Map(G, settlement => settlement.LossCarryforward)
            .Map(H, settlement => settlement.WhiteLabelWinLoss)
            .Map(I, settlement => settlement.WhiteLabelNetWinLoss)
            .Map(J, settlement => settlement.WhiteLabelFee)
            .Map(K, settlement => settlement.WhiteLabelPromotionSettlement)
            .Map(L, settlement => settlement.WhiteLabelPromotionShare)
            .Map(M, settlement => settlement.WhiteLabelTotal)
            .Map(N, settlement => settlement.MasterEarning)
            .Map(O, settlement => settlement.SuperEarning)
            .Map(P, settlement => settlement.CompanyIncome)
            .Map(Q, settlement => settlement.CompanyPromotionShare)
            .Map(R, settlement => settlement.CompanyTotal);
    }

    internal static Mapper<SettlementModel> GetMasterRowMapper(bool isTotal)
    {
        var mapper = new Mapper<SettlementModel>();
        if (isTotal)
        {
            mapper.Map(A, settlement => Resources.Total);
        }
        else
        {
            mapper
                .Map(A, settlement => settlement.Month)
                .Map(B, settlement => settlement.WhiteLabel)
                .Map(C, settlement => settlement.Agent);
        }

        return mapper
            .Map(D, settlement => settlement.MemberWL)
            .Map(E, settlement => settlement.MemberPromotion)
            .Map(F, settlement => settlement.LossCarryforward)
            .Map(G, settlement => settlement.WhiteLabelWinLoss)
            .Map(H, settlement => settlement.WhiteLabelNetWinLoss)
            .Map(I, settlement => settlement.WhiteLabelFee)
            .Map(J, settlement => settlement.WhiteLabelPromotionSettlement)
            .Map(K, settlement => settlement.WhiteLabelPromotionShare)
            .Map(L, settlement => settlement.WhiteLabelTotal)
            .Map(M, settlement => settlement.MasterEarning)
            .Map(N, settlement => settlement.CompanyIncome)
            .Map(O, settlement => settlement.CompanyPromotionShare)
            .Map(P, settlement => settlement.CompanyTotal);
    }

    internal static Mapper<SettlementModel> GetAgentRowMapper(bool isTotal)
    {
        var mapper = new Mapper<SettlementModel>();
        if (isTotal)
        {
            mapper.Map(A, settlement => Resources.Total);
        }
        else
        {
            mapper.Map(A, settlement => settlement.Month);
        }

        return mapper
            .Map(B, settlement => settlement.MemberWL)
            .Map(C, settlement => settlement.MemberPromotion)
            .Map(D, settlement => settlement.LossCarryforward)
            .Map(E, settlement => settlement.WhiteLabelWinLoss)
            .Map(F, settlement => settlement.WhiteLabelNetWinLoss)
            .Map(G, settlement => settlement.WhiteLabelFee)
            .Map(H, settlement => settlement.WhiteLabelPromotionSettlement)
            .Map(I, settlement => settlement.WhiteLabelPromotionShare)
            .Map(J, settlement => settlement.WhiteLabelTotal)
            .Map(K, settlement => settlement.CompanyIncome)
            .Map(L, settlement => settlement.CompanyPromotionShare)
            .Map(M, settlement => settlement.CompanyTotal);
    }
}