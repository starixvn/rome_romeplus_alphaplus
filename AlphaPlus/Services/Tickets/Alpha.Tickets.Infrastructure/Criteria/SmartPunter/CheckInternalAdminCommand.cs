using Fanex.Data.Repository;

namespace Alpha.Tickets.Infrastructure.Criteria.SmartPunter;

internal class CheckInternalAdminCommand(int custId) : NonQueryCommand
{
    public int CustId { get; } = custId;

    public bool IsInternalAdmin { get; set; }

    public override string GetSettingKey()
        => "Acc_CustomerIsInternalAdmin";

    public override bool IsValid()
        => CustId > 0;
}
