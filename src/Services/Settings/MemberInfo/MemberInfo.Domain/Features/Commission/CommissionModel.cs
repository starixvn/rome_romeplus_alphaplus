namespace AlphaPlus.Settings.MemberInfo.Domain.Features.Commission;

public record CommissionModel(
    bool Closed,
    bool Enabled,
    bool Suspended,
    string Username,
    int CustId,
    string UGroup,
    string FirstName,
    string LastName,
    decimal Credit,
    byte Locked,
    byte ClosedReason,
    string Group,
    bool DoubleCommLosing,
    bool IsCashMember,
    IEnumerable<SportCommissionModel> Commissions);
