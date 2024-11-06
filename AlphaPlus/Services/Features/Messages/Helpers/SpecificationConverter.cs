using Alpha.Announcements.Models;
using Alpha.Announcements.Specs;
using Newtonsoft.Json.Linq;

namespace AlphaPlus.Features.Messages.API.Client.Helpers
{
    public class SpecificationConverter : JsonCreationConverter<ISpecification<IParam>>
    {
        protected override Type? GetTargetType(Type type, JObject obj)
        {
            return Type.GetType(obj.GetType().Name);
        }
    }
}