namespace AlphaPlus.Configurations
{
    public interface IConfigurationHelper
    {
        T GetValue<T>(string key, T defaultValue);
    }
}