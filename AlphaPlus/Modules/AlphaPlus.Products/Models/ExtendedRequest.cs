using System.Diagnostics;

namespace AlphaPlus.Products.Models
{
    public interface IExtendedRequest
    {
        IExtendedRequest AddData<T>(string key, T value);

        T GetData<T>(string key);

        bool ContainKey(string key);
    }

    public class ExtendedRequest : IExtendedRequest
    {
        protected Dictionary<string, object> dataBag = [];

        public IExtendedRequest AddData<T>(string key, T value)
        {
            if (!string.IsNullOrWhiteSpace(key))
            {
                dataBag.Add(key, value);
            }

            return this;
        }

        public T GetData<T>(string key)
        {
            if (!dataBag.ContainsKey(key))
            {
                ThrowException(key);
            }

            return (T)dataBag[key];
        }

        public bool ContainKey(string key) => dataBag.ContainsKey(key);

        private static void ThrowException(string key)
        {
            var stackFrame = Array.Find(
                new StackTrace(true).GetFrames(),
                frame =>
                {
                    if (frame != null
                        && frame.GetFileName() != null
                        && frame.GetFileName().Contains("ViewModel"))
                    {
                        return true;
                    }

                    return false;
                });

            throw new KeyNotFoundException($"Key {key} not found. Please review class file {stackFrame?.GetFileName()} at line number {stackFrame?.GetFileLineNumber()}");
        }
    }
}