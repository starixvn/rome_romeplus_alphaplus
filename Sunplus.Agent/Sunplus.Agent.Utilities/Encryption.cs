namespace Sunplus.Agent.Utilities
{
    public class Encryption
    {
        public static string PearsonEncode(string key)
        {
            if (string.IsNullOrEmpty(key) || key.Length == 1) return key;

            var h = 0L;
            var c = key.Length;
            var keys = key.ToCharArray();

            for (var i = 0; i < c; i++)
            {
                h += keys[i].GetHashCode();
            }

            return h.ToString();
        }
    }
}