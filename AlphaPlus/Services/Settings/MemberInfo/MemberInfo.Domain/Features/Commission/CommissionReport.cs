namespace AlphaPlus.Settings.MemberInfo.Domain.Features.Commission;

public record CommissionReport(
    IEnumerable<string> SportTypes,
    IEnumerable<CommissionModel> Commissions)
{
    public bool HasData { get; } = Commissions.Any();
}