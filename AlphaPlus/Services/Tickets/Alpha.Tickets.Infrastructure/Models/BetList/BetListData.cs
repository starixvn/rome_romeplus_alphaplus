namespace Alpha.Tickets.Infrastructure.Models.BetList;

public record BetListData(IEnumerable<Ticket> Tickets, IEnumerable<TicketData> TicketData);
