namespace AlphaPlus.Reports.Features.BetChoiceMatching;

public class BetChoiceMatchingUserModel
{
    public BetChoiceMatchingUserModel(IEnumerable<BetChoiceMatchingTicketModel> tickets)
    {
        Tickets = tickets;
        var firstTicket = tickets.First();

        CustId = firstTicket.CustId;
        Username = firstTicket.Username;
        IsRunning = firstTicket.IsRunning;

        foreach (var ticket in tickets)
        {
            Stake += ticket.Stake;
            if (!IsRunning)
            {
                MemberWinLoss += ticket.MemberWinLoss;
                SuperWinLoss += ticket.SuperWinLoss;
                Company += ticket.Company;
            }
        }

        MemberWinLoss = RoundDown(MemberWinLoss);
    }

    public int CustId { get; }

    public string Username { get; }

    public decimal Stake { get; }

    public decimal MemberWinLoss { get; }

    public decimal SuperWinLoss { get; }

    public decimal Company { get; }

    public bool IsRunning { get; }

    public IEnumerable<BetChoiceMatchingTicketModel> Tickets { get; }

    private static decimal RoundDown(decimal value)
    {
        var power = Convert.ToDecimal(Math.Pow(10, 2));

        return Math.Floor(value * power) / power;
    }
}