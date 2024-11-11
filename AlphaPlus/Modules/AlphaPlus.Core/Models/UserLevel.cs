namespace AlphaPlus.Core.Models;

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

    private UserLevel(byte value, string displayName) : base(value, displayName)
    {
    }

    public bool IsAgentLevel() => this == Agent;

    public bool IsNotAgentLevel() => !IsAgentLevel();

    public bool IsMasterLevel() => this == Master;

    public bool IsNotMasterLevel() => !IsMasterLevel();

    public bool IsMasterOrSuperLevel() => IsMasterLevel() || IsSuperLevel();

    public bool IsDownlineOfSuperLevel() => IsMasterLevel() || IsAgentLevel() || IsMemberLevel();

    public bool IsDownlineOfMasterLevel() => IsAgentLevel() || IsMemberLevel();

    public bool IsSuperLevel() => this == Super;

    public bool IsNotSuperLevel() => !IsSuperLevel();

    public bool IsMemberLevel() => this == Member;

    public bool IsNotMemberLevel() => !IsMemberLevel();

    public bool IsDirectUplineOf(byte downlineLevelValue)
        => GapBetweenTwoLevels(this.Value, downlineLevelValue) == 1;

    public bool IsNotDirectUplineOf(byte downlineLevelValue)
        => !IsDirectUplineOf(downlineLevelValue);

    public bool IsDirectUplineOf(Enumeration customerLevel)
    {
        if (customerLevel == null)
        {
            return false;
        }

        return IsDirectUplineOf(customerLevel.Value);
    }

    public bool IsNotDirectUplineOf(Enumeration customerLevel)
        => !IsDirectUplineOf(customerLevel);

    public bool IsDirectDownlineOf(Enumeration uplineLevel)
    {
        if (uplineLevel == null)
        {
            return false;
        }

        return GapBetweenTwoLevels(uplineLevel.Value, Value) == 1;
    }

    public bool IsNotDirectDownlineOf(Enumeration uplineLevel)
        => !IsDirectDownlineOf(uplineLevel);

    public bool Is(Enumeration level)
    {
        if (level != null)
        {
            return this == level;
        }

        return false;
    }

    public bool IsNot(Enumeration level) => !Is(level);

    public UserLevel GetDirectUplineLevel() => FromValue<UserLevel>((byte)(Value + 1));

    public UserLevel GetDirectDownLineLevel() => FromValue<UserLevel>((byte)(Value - 1));

    public bool IsEqualOrUplineOf(Enumeration custLevel)
        => Value == custLevel.Value || IsUplineOf(custLevel.Value);

    public bool IsUplineOf(byte downlineLevelValue)
        => GapBetweenTwoLevels(Value, downlineLevelValue) >= 1;

    public bool IsUplineOf(Enumeration downlineLevel)
        => (downlineLevel is not null) && IsUplineOf(downlineLevel.Value);

    private static int GapBetweenTwoLevels(byte uplineLevelValue, byte downlineLevelValue)
        => uplineLevelValue - downlineLevelValue;
}