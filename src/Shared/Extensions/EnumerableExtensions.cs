using System.Diagnostics.CodeAnalysis;

[SuppressMessage("SonarQube", "S3903:Types should be defined in named namespaces", Justification = "This is a static class that contains extension methods for IEnumerable<T>.")]
internal static class EnumerableExtensions
{
    public static string JoinWithComma<T>(this IEnumerable<T> array, string separator = ",")
        => array == null ? string.Empty : string.Join(separator, array);
}
