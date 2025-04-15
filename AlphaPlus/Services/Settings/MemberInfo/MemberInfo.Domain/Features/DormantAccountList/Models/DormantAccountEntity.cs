namespace AlphaPlus.Settings.MemberInfo.Domain.Features.DormantAccountList.Models;

public class DormantAccountEntity
{
    public int CustId { get; init; }

    public string Username { get; init; }

    public byte RoleId { get; init; }

    public byte StatusId { get; init; }

    public byte StageId { get; init; }

    public decimal? ToBeClearedGivenCredit { get; init; }

    public decimal? ClearedGivenCredit { get; init; }

    public DateTime? LastLogged { get; init; }

    public DateTime? ClosedDate { get; init; }

    public DateTime? DisabledDate { get; init; }

    public DateTime? SettingDeletedDate { get; init; }

    public DateTime? AccountDeletedDate { get; init; }

    public bool IsWarningTicket { get; init; }

    public int RowNumber { get; init; }

    public int Recommend { get; init; }
}
