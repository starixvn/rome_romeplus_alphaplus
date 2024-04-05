// using System.ComponentModel;
// using AlphaPlus.Core.Configurations;

// namespace AlphaPlus.Features.Messages.Api.Helpers
// {
//     public class ConfigurationHelper : IConfigurationHelper
//     {
//         public T GetValue<T>(string key, T defaultValue)
//         {
//             try
//             {
//                 var value = ConfigurationManager.GetValue<T>(key);
//                 if (value == null)
//                 {
//                     return defaultValue;
//                 }

//                 TypeConverter typeConverter = TypeDescriptor.GetConverter(typeof(T));

//                 return (T)typeConverter.ConvertFromString(value);
//             }
//             catch (Exception ex)
//             {
//                 throw new InvalidCastException($"Key: {key}", ex);
//             }
//         }
//     }
// }