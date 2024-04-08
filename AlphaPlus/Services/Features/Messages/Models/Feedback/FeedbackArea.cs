namespace AlphaPlus.Features.Messages.API.Models.Feedback
{
    using AlphaPlus.Features.Messages.API.Constants;

    public class FeedbackArea
    {
        public int AreaId { get; set; }

        public string AreaNameEN { get; set; }

        public string AreaNameCN3 { get; set; }

        public string AreaNameCS { get; set; }

        public string AreaNameCH { get; set; }

        public string AreaNameTH { get; set; }

        public string AreaNameJP { get; set; }

        public string AreaNameKO { get; set; }

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
