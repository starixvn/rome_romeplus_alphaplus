using Fanex.Data.Repository;

namespace AlphaPlus.BetLists.Shared.Criteria;

public class SmartPunterSMACheckerCommand(int? custId, string username) : NonQueryCommand
{
    public int? CustId { get; } = custId;

    public string Username { get; } = username;

    public bool IsSmartPunter { get; set; }

    public bool HasDownlineSmartPunter { get; set; }

    public override string GetSettingKey() => "Age_CustomerSelCheckSmartPunterSMA";

    public override bool IsValid() => CustId.HasValue || Username != null;
}