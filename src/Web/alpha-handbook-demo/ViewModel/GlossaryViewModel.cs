using Alpha.Handbook.Demo.Models;

namespace Alpha.Handbook.Demo.ViewModels
{
    public class GlossaryViewModel
    {
        public string Group { get; set; }
        public IEnumerable<GlossaryModel> Glossaries { get; set; }
        public Dictionary<string, List<GlossaryModel>> Groups
        {
            get
            {
                var groups = new Dictionary<string, List<GlossaryModel>>();
                if (Glossaries == null)
                {
                    return groups;
                }

                foreach (var g in Glossaries)
                {
                    if (groups.ContainsKey(g.Group))
                    {
                        groups[g.Group].Add(g);
                        continue;
                    }

                    groups.Add(g.Group, new List<GlossaryModel> { g });
                }

                return groups;
            }
        }
    }
}