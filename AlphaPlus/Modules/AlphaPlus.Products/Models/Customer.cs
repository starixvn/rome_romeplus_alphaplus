using AlphaPlus.Products.Shared;

namespace AlphaPlus.Products.Models
{
    public interface ICustomer
    {
        int Id { get; }
        byte CustomerLevelValue { get; }
        CustomerLevel CustomerLevel { get; }
        bool IsAgent { get; }
        bool IsMainAccount { get; }
        bool IsMaster { get; }
        bool IsMasterOrSuper { get; }
        bool IsMember { get; }
        bool IsNotAgent { get; }
        bool IsNotMaster { get; }
        bool IsNotMember { get; }
        bool IsNotSuper { get; }
        bool IsSubAccount { get; }
        bool IsSuper { get; }
        int SubAccountId { get; }
        ICustomer Upline { get; }
        string Username { get; }

        bool CanSeeProduct(int productId);

        bool IsDirectDownlineOf(ICustomer customer);

        bool IsDirectUplineOf(ICustomer customer);

        bool IsNotDirectDownlineOf(ICustomer customer);

        bool IsNotDirectUplineOf(ICustomer customer);

        bool IsUplineOf(ICustomer customer);

        CustomerProductStatus ProductStatusOf(int productId);

        IEnumerable<CustomerProductStatus> GetProductStatus();
    }

    public class Customer : ICustomer
    {
        private readonly IEnumerable<CustomerProductStatus> customerProductStatus;

        public Customer()
        { }

        public Customer(
            int id,
            int subAccountId,
            string username,
            byte customerLevelValue,
            IEnumerable<CustomerProductStatus> customerProductStatus)
        {
            Id = id;
            SubAccountId = subAccountId;
            Username = username;
            CustomerLevelValue = customerLevelValue;
            this.customerProductStatus = customerProductStatus;
        }

        public Customer(
            int id,
            int subAccountId,
            string username,
            byte userLevelValue,
            IEnumerable<CustomerProductStatus> customerProductStatus,
            ICustomer upline)
            : this(id, subAccountId, username, userLevelValue, customerProductStatus)
        {
            Upline = upline;
        }

        public int Id { get; }

        public string Username { get; }

        public byte CustomerLevelValue { get; }

        public CustomerLevel CustomerLevel => Enumeration.FromValue<CustomerLevel>(CustomerLevelValue);

        public int SubAccountId { get; }

        public bool IsSubAccount => SubAccountId > 0;

        public bool IsMainAccount => !IsSubAccount;

        public bool CanSeeProduct(int productId)
        {
            var userProductStatus = ProductStatusOf(productId);

            return userProductStatus?.Hidden == false;
        }

        public CustomerProductStatus ProductStatusOf(int productId)
            => customerProductStatus.First(ps => ps.ProductId == productId);

        public ICustomer Upline { get; }

        public bool IsAgent => CustomerLevel.IsAgent();

        public bool IsNotAgent => !IsAgent;

        public bool IsMaster => CustomerLevel.IsMaster();

        public bool IsNotMaster => !IsMaster;

        public bool IsSuper => CustomerLevel.IsSuper();

        public bool IsMasterOrSuper => IsMaster || IsSuper;

        public bool IsNotSuper => !IsSuper;

        public bool IsMember => CustomerLevel.IsMember();

        public bool IsNotMember => !IsMember;

        public bool IsUplineOf(ICustomer customer)
            => CustomerLevel.Value > customer.CustomerLevel.Value;

        public bool IsDirectUplineOf(ICustomer customer)
            => CustomerLevel.IsDirectUplineOf(customer.CustomerLevel);

        public bool IsNotDirectUplineOf(ICustomer customer)
            => !IsDirectUplineOf(customer);

        public bool IsDirectDownlineOf(ICustomer customer)
            => CustomerLevel.IsDirectDownlineOf(customer.CustomerLevel);

        public bool IsNotDirectDownlineOf(ICustomer customer)
            => !IsDirectDownlineOf(customer);

        public IEnumerable<CustomerProductStatus> GetProductStatus() => customerProductStatus;
    }

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