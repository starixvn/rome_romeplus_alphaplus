using AlphaPlus.BetLists.Shared.Models;
using AlphaPlus.Core.Models;
using AlphaPlus.SmartPunter.Services;
using AlphaPlus.Tickets.Site.Features.BetLists.ViewModels;
using AlphaPlus.Tickets.Site.Features.Shared.Models;
using AlphaPlus.Utilities.IP;
using Fanex.BetList.Core.Builder;
using Fanex.BetList.Core.Entities;
using Microsoft.Extensions.Options;
using ONEbook.SpreadSheets;
using static ONEbook.SpreadSheets.Column;

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
    protected internal override Mapper<TicketDto> GetBodyRowMapper(UserLevel viewerLevel, BetListBaseViewModel viewModel)
    {
        var ticketData = viewModel.BetListData.TicketData;

        var mapper = GetTicketDtoRowMapperIncludeUsername(viewModel, showWinLoss: false);
        mapper.Map(H, ticket => BuildPtCommmission(ticket, ticketData, viewerLevel));

        if (viewerLevel.IsSuperLevel())
        {
            AddPtCommissionForSuper(mapper, viewerLevel, ticketData);
            mapper.Map(K, ticket => ticket.MemberWinText);
            mapper.Map(L, ticket => viewModel.GetSMARiskAmount(ticket));
            mapper.Map(M, ticket => BuildPlatform(ticket.Platform));
        }
        else if (viewerLevel.IsMasterLevel())
        {
            mapper.Map(I, ticket => BuildPtCommmission(ticket, ticketData, UserLevel.Agent));
            mapper.Map(J, ticket => ticket.MemberWinText);
            mapper.Map(K, ticket => viewModel.GetSMARiskAmount(ticket));
            mapper.Map(L, ticket => BuildPlatform(ticket.Platform));
        }
        else if (viewerLevel.IsAgentLevel())
        {
            mapper.Map(I, ticket => ticket.MemberWinText);
            mapper.Map(J, ticket => viewModel.GetSMARiskAmount(ticket));
            mapper.Map(K, ticket => BuildPlatform(ticket.Platform));
        }

        return mapper;
    }
}