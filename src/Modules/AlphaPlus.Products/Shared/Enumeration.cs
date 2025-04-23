using System.Collections;
using System.Reflection;

namespace AlphaPlus.Products.Shared
{
    [Serializable]
    public abstract class Enumeration : IComparable
    {
        protected Enumeration()
        {
        }

        protected Enumeration(byte value, string displayName)
        {
            Value = value;
            DisplayName = displayName;
        }

        public string DisplayName { get; }

        public byte Value { get; }

        public static byte AbsoluteDifference(Enumeration firstValue, Enumeration secondValue)
            => (byte)Math.Abs(firstValue.Value - secondValue.Value);

        public static T FromDisplayName<T>(string displayName) where T : Enumeration, new()
            => Parse<T, string>(displayName, "display name", item => item.DisplayName == displayName);

        public static T FromValue<T>(byte value) where T : Enumeration, new()
            => Parse<T, byte>(value, "value", item => item.Value == value);

        public static IEnumerable<T> GetAll<T>() where T : Enumeration, new()
        {
            var type = typeof(T);
            foreach (var info in type.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly))
            {
                var instance = new T();
                var locatedValue = info.GetValue(instance) as T;

                if (locatedValue != null)
                {
                    yield return locatedValue;
                }
            }
        }

        public static IEnumerable GetAll(Type type)
        {
            foreach (var info in type.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly))
            {
                object instance = Activator.CreateInstance(type);
                yield return info.GetValue(instance);
            }
        }

        public static bool operator ==(Enumeration left, Enumeration right)
        {
            if (left is null)
            {
                return right is null;
            }

            if (right is null)
            {
                return false;
            }

            return left.Value == right.Value;
        }

        public static bool operator !=(Enumeration left, Enumeration right)
        {
            return !(left == right);
        }

        public static bool operator >(Enumeration left, Enumeration right)
        {
            ValidateInputArguments(left, right);
            return left.Value > right.Value;
        }

        public static bool operator >=(Enumeration left, Enumeration right)
        {
            ValidateInputArguments(left, right);
            return left.Value >= right.Value;
        }

        public static bool operator <(Enumeration left, Enumeration right)
        {
            ValidateInputArguments(left, right);
            return left.Value < right.Value;
        }

        public static bool operator <=(Enumeration left, Enumeration right)
        {
            ValidateInputArguments(left, right);
            return left.Value <= right.Value;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            if (!(obj is Enumeration otherValue))
            {
                return false;
            }

            var typeMatches = GetType().Equals(obj.GetType());
            var valueMatches = Value.Equals(otherValue.Value);

            return typeMatches && valueMatches;
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override string ToString()
        {
            return DisplayName;
        }

        public virtual int CompareTo(object obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            return Value.CompareTo(((Enumeration)obj).Value);
        }

        private static void ValidateInputArguments(Enumeration left, Enumeration right)
        {
            if (left is null)
            {
                throw new ArgumentNullException(nameof(left));
            }

            if (right is null)
            {
                throw new ArgumentNullException(nameof(right));
            }
        }

        private static T Parse<T, K>(K value, string description, Func<T, bool> predicate) where T : Enumeration, new()
        {
            var matchingItem = GetAll<T>().FirstOrDefault(predicate);

            if (matchingItem == null)
            {
                var message = $"'{value}' is not a valid {description} in {typeof(T)}";
                throw new ArgumentOutOfRangeException(message);
            }

            return matchingItem;
        }
    }
}