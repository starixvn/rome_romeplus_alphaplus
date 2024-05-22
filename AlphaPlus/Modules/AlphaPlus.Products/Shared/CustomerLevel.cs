namespace AlphaPlus.Products.Shared
{
    [Serializable]
    public class CustomerLevel : Enumeration
    {
        private const int SuperLevelValue = 4;
        private const int MasterLevelValue = 3;
        private const int AgentLevelValue = 2;
        private const int MemberLevelValue = 1;

        public static readonly CustomerLevel Super = new CustomerLevel(SuperLevelValue, "Super");

        public static readonly CustomerLevel Master = new CustomerLevel(MasterLevelValue, "Master");

        public static readonly CustomerLevel Agent = new CustomerLevel(AgentLevelValue, "Agent");
        public static readonly CustomerLevel Member = new CustomerLevel(MemberLevelValue, "Member");

        public CustomerLevel()
        {
        }

        private CustomerLevel(byte value, string displayName)
            : base(value, displayName)
        {
        }

        public bool IsAgent() => this == Agent;

        public bool IsNotAgent() => !IsAgent();

        public bool IsMaster() => this == Master;

        public bool IsNotMaster() => !IsMaster();

        public bool IsSuper() => this == Super;

        public bool IsMasterOrSuper() => IsMaster() || IsSuper();

        public bool IsNotSuper() => !IsSuper();

        public bool IsMember() => this == Member;

        public bool IsNotMember() => !IsMember();

        public bool IsDirectUplineOf(byte downlineLevelValue)
            => GapBetweenTwoLevels(Value, downlineLevelValue) == 1;

        public bool IsDirectUplineOf(Enumeration downlineLevel)
            => downlineLevel != null && IsDirectUplineOf(downlineLevel.Value);

        public bool IsUplineOf(byte downlineLevelValue)
            => GapBetweenTwoLevels(Value, downlineLevelValue) >= 1;

        public bool IsUplineOf(Enumeration downlineLevel)
            => downlineLevel != null && IsUplineOf(downlineLevel.Value);

        public bool IsEqualOrUplineOf(Enumeration custLevel)
            => Value == custLevel.Value || IsUplineOf(custLevel.Value);

        public CustomerLevel GetDirectDownLineLevel()
            => FromValue<CustomerLevel>((byte)(Value - 1));

        public bool IsDirectDownlineOf(Enumeration uplineLevel)
            => uplineLevel != null && GapBetweenTwoLevels(uplineLevel.Value, Value) == 1;

        public bool Is(Enumeration level)
            => level != null && this == level;

        public bool IsNot(Enumeration level) => !Is(level);

        private static int GapBetweenTwoLevels(byte uplineLevelValue, byte downlineLevelValue)
            => uplineLevelValue - downlineLevelValue;

        public override bool Equals(object obj)
            => !(obj is null) && ReferenceEquals(this, obj);

        public override int GetHashCode() => Value.GetHashCode();

        public static bool operator ==(CustomerLevel left, CustomerLevel right)
            => (left is null) ? right is null : left.Equals(right);

        public static bool operator !=(CustomerLevel left, CustomerLevel right)
            => !(left == right);

        public static bool operator <(CustomerLevel left, CustomerLevel right)
            => left is null ? !(right is null) : left.CompareTo(right) < 0;

        public static bool operator <=(CustomerLevel left, CustomerLevel right)
            => left is null || left.CompareTo(right) <= 0;

        public static bool operator >(CustomerLevel left, CustomerLevel right)
            => !(left is null) && left.CompareTo(right) > 0;

        public static bool operator >=(CustomerLevel left, CustomerLevel right)
            => left is null ? right is null : left.CompareTo(right) >= 0;
    }
}