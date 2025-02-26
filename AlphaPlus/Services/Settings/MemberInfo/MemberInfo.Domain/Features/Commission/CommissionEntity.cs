namespace AlphaPlus.Settings.MemberInfo.Domain.Features.Commission;

public class CommissionEntity
{
    public bool Closed { get; init; }

    public bool Enabled { get; init; }

    public bool Suspended { get; init; }

    public string Username { get; init; }

    public int CustId { get; init; }

    public string UGroup { get; init; }

    public string FirstName { get; init; }

    public string LastName { get; init; }

    public decimal Credit { get; init; }

    // Properties of Sportsbook
    public bool DoubleCommLosing { get; init; }

    public decimal ADiscount { get; init; }
    public decimal Discount { get; init; }
    //public decimal PlayerDiscount { get; init; }

    public decimal ADiscount1X2 { get; init; }
    public decimal Discount1X2 { get; init; }
    public decimal PlayerDiscount1X2 { get; init; }

    public decimal ADiscountCS { get; init; }
    public decimal DiscountCS { get; init; }
    public decimal PlayerDiscountCS { get; init; }

    // End of Properties of Sportsbook

    // Properties of DCS
    public int SportType { get; init; }

    public decimal MasterDiscount { get; init; }

    public decimal AgentDiscount { get; init; }

    public decimal PlayerDiscount { get; init; }
    // End of Properties of DCS

    public byte Locked { get; init; }

    public byte ClosedReason { get; init; }
}
