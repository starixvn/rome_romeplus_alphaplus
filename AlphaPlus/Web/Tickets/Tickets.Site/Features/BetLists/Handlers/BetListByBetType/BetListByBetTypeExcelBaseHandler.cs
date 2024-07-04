using AlphaPlus.BetLists.Shared.Models;
using AlphaPlus.SmartPunter.Services;
using AlphaPlus.Tickets.Site.Features.BetLists.ViewModels;
using AlphaPlus.Tickets.Site.Features.Shared.Models;
using AlphaPlus.Utilities.IP;
using Fanex.BetList.Core.Builder;
using Fanex.BetList.Core.Entities;
using Microsoft.Extensions.Options;
using ONEbook.SpreadSheets;
using static ONEbook.SpreadSheets.Column;
using _UserLevel = AlphaPlus.Core.Models.UserLevel;

namespace AlphaPlus.Tickets.Site.Features.BetLists.Handlers.BetListByBetType;

public class BetListByBetTypeExcelBaseHandler(
    IBetListHTMLBuilder builder,
    IBetListExcelBuilder excelBuilder,
    ITicketHelper ticketHelper,
    ISmartPunterBetListMonitor monitorService,
    IPService ipService,
    IOptions<AppSettings> options,
    IWebHostEnvironment webHostEnvironment) : BetListExcelBaseHandler(builder, excelBuilder, ticketHelper, monitorService, ipService, options, webHostEnvironment)
{
    protected internal override Mapper<TicketDto> GetBodyRowMapper(_UserLevel viewerLevel, BetListBaseViewModel viewModel)
    {
        var ticketData = viewModel.BetListData.TicketData;

        var mapper = GetTicketDtoRowMapperIncludeUsername(viewModel, showWinLoss: false);

        mapper.Map(H, ticket => BuildPtCommmission(ticket, ticketData, viewerLevel));

        if (viewerLevel.IsMasterLevel())
        {
            mapper.Map(I, ticket => BuildPtCommmission(ticket, ticketData, _UserLevel.Agent));
        }

        AddPtCommissionForSuper(mapper, viewerLevel, ticketData);

        AddPlatform(mapper, viewerLevel);

        return mapper;
    }
}