using Fanex.BetList.Core.Entities;

namespace AlphaPlus.BetLists.Models;

public class BetListData(
    IEnumerable<TicketDto> tickets,
    IEnumerable<ITicketData> ticketData,
    BetlistOutputParams betlistOutputParams = null)
{
    public IEnumerable<TicketDto> Tickets { get; } = tickets ?? Enumerable.Empty<TicketDto>();

    public List<ITicketData> TicketData { get; } = ticketData == null ? new() : ticketData.ToList();

    public int Total => Tickets.Count();

    public BetlistOutputParams BetlistOutputParams { get; } = betlistOutputParams ?? new BetlistOutputParams();
}
