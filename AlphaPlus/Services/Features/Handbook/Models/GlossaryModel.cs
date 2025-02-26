namespace AlphaPlus.Features.Handbook.API.Models
{
    public class GlossaryModel
    {
        char[] alpha = "#ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();

        public string Name { get; set; }
        public string Content { get; set; }
        public string Title { get; set; }

        public string Group
            => alpha.Any(a => Title.ToLowerInvariant().StartsWith(a.ToString().ToLowerInvariant())) ? Title.ElementAt(0).ToString() : "#";
    }
}