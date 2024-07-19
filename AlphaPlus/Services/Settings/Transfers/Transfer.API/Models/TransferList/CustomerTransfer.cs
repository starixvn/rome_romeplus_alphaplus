namespace AlphaPlus.Settings.Transfer.API.Models.TransferList;

public class CustomerTransfer
{
    public int RowNumber { get; init; }

    public string Username { get; init; }

    public int CustId { get; init; }

    public bool IsAllowAutoTransfer { get; init; }

    public string ScheduleWeekDays { get; init; }

    public decimal YesterdayBalance { get; init; }

    public decimal Balance { get; init; }

    public decimal YesterdayCashBalance { get; init; }

    public decimal CashBalance { get; init; }

    public decimal Outstanding { get; init; }

    public decimal GivenCredit { get; init; }

    public decimal UsedCredit { get; init; }

    public decimal PendingTransfer3rd { get; init; }

    public string FirstName { get; init; }

    public string LastName { get; init; }

    public bool Enabled { get; init; }

    public bool Closed { get; init; }

    public byte ClosedReason { get; init; }

    public bool Suspended { get; init; }

    public byte Locked { get; init; }
}
