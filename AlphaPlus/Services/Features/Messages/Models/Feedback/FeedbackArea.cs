using AlphaPlus.Features.Messages.API.Constants;

namespace AlphaPlus.Features.Messages.API.Models.Feedback
{
    public class FeedbackArea
    {
        public int AreaId { get; set; }

        public string AreaNameEN { get; set; } = string.Empty;

        public string AreaNameCN3 { get; set; } = string.Empty;

        public string AreaNameCS { get; set; } = string.Empty;

        public string AreaNameCH { get; set; } = string.Empty;

        public string AreaNameTH { get; set; } = string.Empty;

        public string AreaNameJP { get; set; } = string.Empty;

        public string AreaNameKO { get; set; } = string.Empty;

        public string GetAreaName(string language)
        {
            switch (language)
            {
                case Language.ChineseSimplified:
                    return AreaNameCS;

                case Language.ChineseTraditional:
                    return AreaNameCH;

                case Language.Thai:
                    return AreaNameTH;

                case Language.Vietnamese:
                    return AreaNameCN3;

                case Language.Japanese:
                    return string.IsNullOrWhiteSpace(AreaNameJP) ? AreaNameEN : AreaNameJP;

                case Language.Korean:
                    return string.IsNullOrWhiteSpace(AreaNameKO) ? AreaNameEN : AreaNameKO;

                default:
                    return AreaNameEN;
            }
        }
    }
}
