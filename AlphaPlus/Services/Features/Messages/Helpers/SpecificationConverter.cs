namespace Alpha.Messages.Client.Helpers
{
    using System;
    using Announcements.Models;
    using Announcements.Specs;
    using Newtonsoft.Json.Linq;

    public class SpecificationConverter : JsonCreationConverter<ISpecification<IParam>>
    {
        protected override Type GetTargetType(Type type, JObject obj)
        {
            return Type.GetType(obj.GetValue("TypeName", StringComparison.OrdinalIgnoreCase).ToString());
        }
    }
}