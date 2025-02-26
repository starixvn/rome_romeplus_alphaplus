using Fanex.BetList.Core.Entities;

namespace AlphaPlus.BetLists.Shared.Models;

public class BetListData(
    IEnumerable<TicketDto> tickets,
    IEnumerable<ITicketData> ticketData,
    BetListOutputParams outputParams = null)
{
    public IEnumerable<TicketDto> Tickets { get; } = tickets ?? Enumerable.Empty<TicketDto>();

    public List<ITicketData> TicketData { get; } = ticketData == null ? new() : ticketData.ToList();

    public int Total => Tickets.Count();

    public BetListOutputParams OutputParams { get; set; } = outputParams ?? new BetListOutputParams();
}
