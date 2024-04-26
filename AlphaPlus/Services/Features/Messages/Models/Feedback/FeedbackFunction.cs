namespace AlphaPlus.Features.Messages.API.Models.Feedback
{
    using AlphaPlus.Features.Messages.API.Constants;

    public class FeedbackFunction
    {
        public int FuncId { get; set; }

        public string FuncNameEN { get; set; } = string.Empty;

        public string FuncNameCN3 { get; set; } = string.Empty;

        public string FuncNameCS { get; set; } = string.Empty;

        public string FuncNameCH { get; set; } = string.Empty;

        public string FuncNameTH { get; set; } = string.Empty;

        public string FuncNameJP { get; set; } = string.Empty;

        public string FuncNameKO { get; set; } = string.Empty;

        public string GetFunctionName(string language)
        {
            switch (language)
            {
                case Language.ChineseSimplified:
                    return FuncNameCS;

                case Language.ChineseTraditional:
                    return FuncNameCH;

                case Language.Thai:
                    return FuncNameTH;

                case Language.Vietnamese:
                    return FuncNameCN3;

                case Language.Japanese:
                    return string.IsNullOrWhiteSpace(FuncNameJP) ? FuncNameEN : FuncNameJP;

                case Language.Korean:
                    return string.IsNullOrWhiteSpace(FuncNameKO) ? FuncNameEN : FuncNameKO;

                default:
                    return FuncNameEN;
            }
        }
    }
}
