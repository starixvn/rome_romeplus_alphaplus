namespace AlphaPlus.Features.Messages.Client.Helpers
{
    using System;
    using Alpha.Announcements.Models;
    using Alpha.Announcements.Specs;
    using Newtonsoft.Json.Linq;

    public class SpecificationConverter : JsonCreationConverter<ISpecification<IParam>>
    {
        protected override Type GetTargetType(Type type, JObject obj)
        {
            return Type.GetType(obj.GetValue("TypeName", StringComparison.OrdinalIgnoreCase).ToString());
        }
    }
}