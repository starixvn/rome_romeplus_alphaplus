using System.Diagnostics;

namespace AlphaPlus.NetThrottling
{
    public interface IDisableNetThrottling { }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    [DebuggerDisplay("{ToString(),nq}")]
    public class DisableNetThrottlingAttribute : Attribute, IDisableNetThrottling
    {
        public override string ToString()
        {
            return "DisableNetThrottling";
        }
    }
}