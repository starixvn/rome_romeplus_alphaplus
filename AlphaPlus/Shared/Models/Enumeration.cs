using System.Collections;
using System.Reflection;

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

    public byte Value { get; }

    public string DisplayName { get; }

    public static byte AbsoluteDifference(Enumeration firstValue, Enumeration secondValue)
        => (byte)Math.Abs(firstValue.Value - secondValue.Value);

    public static T FromDisplayName<T>(string displayName) where T : Enumeration, new()
        => Parse<T, string>(displayName, "display name", item => item.DisplayName == displayName);

    public static T FromValue<T>(byte value) where T : Enumeration, new()
        => Parse<T, byte>(value, "value", item => item.Value == value);

    public static IEnumerable<T> GetAll<T>() where T : Enumeration, new()
    {
        var type = typeof(T);
        var fields = type.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly);

        foreach (var info in fields)
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
        var fields = type.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly);

        foreach (var info in fields)
        {
            object instance = Activator.CreateInstance(type);
            yield return info.GetValue(instance);
        }
    }

    public static bool operator ==(Enumeration left, Enumeration right)
    {
        if (ReferenceEquals(left, null))
        {
            return ReferenceEquals(right, null);
        }

        return ReferenceEquals(right, null) ? false : left.Value == right.Value;
    }

    public static bool operator !=(Enumeration left, Enumeration right)
        => !(left == right);

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

        var otherValue = obj as Enumeration;

        if (otherValue == null)
        {
            return false;
        }

        var typeMatches = GetType().Equals(obj.GetType());
        var valueMatches = Value.Equals(otherValue.Value);

        return typeMatches && valueMatches;
    }

    public override int GetHashCode() => Value.GetHashCode();

    public override string ToString() => DisplayName;

    public virtual int CompareTo(object obj)
    {
        if (obj == null)
        {
            throw new ArgumentNullException("obj");
        }

        return Value.CompareTo(((Enumeration)obj).Value);
    }

    private static void ValidateInputArguments(Enumeration left, Enumeration right)
    {
        if (ReferenceEquals(left, null))
        {
            throw new ArgumentNullException("left");
        }

        if (ReferenceEquals(right, null))
        {
            throw new ArgumentNullException("right");
        }
    }

    private static T Parse<T, K>(K value, string description, Func<T, bool> predicate) where T : Enumeration, new()
    {
        var matchingItem = GetAll<T>().FirstOrDefault(predicate);

        if (matchingItem == null)
        {
            var message = string.Format("'{0}' is not a valid {1} in {2}", value, description, typeof(T));
            throw new ArgumentOutOfRangeException(message, CancellationToken cancellation);
        }

        return matchingItem;
    }
}
