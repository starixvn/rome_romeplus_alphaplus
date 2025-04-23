using Alpha.Announcements.Models;
using Alpha.Announcements.Specs;
using Newtonsoft.Json.Linq;

namespace AlphaPlus.Features.Messages.API.Client.Helpers
{
    public class SpecificationConverter : JsonCreationConverter<ISpecification<IParam>>
    {
        protected override Type? GetTargetType(Type type, JObject obj)
        {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            return Type.GetType(obj.GetValue("TypeName", StringComparison.OrdinalIgnoreCase).ToString());
#pragma warning restore CS8602 // Dereference of a possibly null reference.
        }
    }
}