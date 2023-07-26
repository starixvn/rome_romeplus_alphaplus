using AlphaPlus.Auth.Enumerations;

namespace AlphaPlus.Auth.Models;

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

    public bool IsAgentLevel() => this == Agent;

    public bool IsNotAgentLevel() => !IsAgentLevel();

    public bool IsMasterLevel() => this == Master;

    public bool IsNotMasterLevel() => !IsMasterLevel();

    public bool IsSuperLevel() => this == Super;

    public bool IsMasterOrSuperLevel() => IsMasterLevel() || IsSuperLevel();

    public bool IsNotSuperLevel() => !IsSuperLevel();

    public bool IsMemberLevel() => this == Member;

    public bool IsDirectUplineOf(byte downlineLevelValue)
        => GapBetweenTwoLevels(Value, downlineLevelValue) == 1;

    public bool IsDirectUplineOf(Enumeration downlineLevel)
        => (downlineLevel != null) && IsDirectUplineOf(downlineLevel.Value);

    public bool IsUplineOf(byte downlineLevelValue)
        => GapBetweenTwoLevels(Value, downlineLevelValue) >= 1;

    public bool IsUplineOf(Enumeration downlineLevel)
        => (downlineLevel != null) && IsUplineOf(downlineLevel.Value);

    public bool IsEqualOrUplineOf(Enumeration custLevel)
       => this.Value == custLevel.Value || IsUplineOf(custLevel.Value);

    public UserLevel GetDirectUplineLevel() => FromValue<UserLevel>((byte)(Value + 1));

    public UserLevel GetDirectDownLineLevel() => FromValue<UserLevel>((byte)(Value - 1));

    public bool IsDirectDownlineOf(Enumeration uplineLevel)
        => (uplineLevel != null) && GapBetweenTwoLevels(uplineLevel.Value, Value) == 1;

    public bool Is(Enumeration level)
        => (level != null) && this == level;

    public bool IsNot(Enumeration level) => !Is(level);

    private static int GapBetweenTwoLevels(byte uplineLevelValue, byte downlineLevelValue)
        => uplineLevelValue - downlineLevelValue;
}