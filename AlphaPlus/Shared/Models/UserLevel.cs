[Serializable]
public class UserLevel : Enumeration
{
    public static readonly UserLevel Super = new(4, "Super");
    public static readonly UserLevel Master = new(3, "Master");
    public static readonly UserLevel Agent = new(2, "Agent");
    public static readonly UserLevel Member = new(1, "Member");

    public UserLevel()
    {
    }

    private UserLevel(byte value, string displayName)
        : base(value, displayName)
    {
    }

    public bool IsSuperLevel() => this == Super;

    public bool IsMasterLevel() => this == Master;

    public bool IsMasterOrSuperLevel() => IsMasterLevel() || IsSuperLevel();

    public bool IsAgentLevel() => this == Agent;

    public bool IsNotAgentLevel() => !IsAgentLevel();

    public bool IsMemberLevel() => this == Member;

    public bool IsNotMemberLevel() => !IsMemberLevel();

    public bool IsMemberOrAgentLevel() => IsMemberLevel() || IsAgentLevel();

    public UserLevel GetDirectDownlineLevel() => FromValue<UserLevel>((byte)(Value - 1));

    public bool IsEqualOrUplineOf(Enumeration custLevel)
        => GapBetweenTwoLevels(Value, custLevel.Value) >= 0;

    private static int GapBetweenTwoLevels(byte uplineLevelValue, byte downlineLevelValue)
        => uplineLevelValue - downlineLevelValue;
}
