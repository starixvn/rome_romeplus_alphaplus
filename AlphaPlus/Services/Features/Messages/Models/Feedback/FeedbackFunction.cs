namespace AlphaPlus.Features.Messages.API.Models.Feedback
{
    using AlphaPlus.Features.Messages.API.Constants;

    public class FeedbackFunction
    {
        public int FuncId { get; set; }

        public string FuncNameEN { get; set; }

        public string FuncNameCN3 { get; set; }

        public string FuncNameCS { get; set; }

        public string FuncNameCH { get; set; }

        public string FuncNameTH { get; set; }

        public string FuncNameJP { get; set; }

        public string FuncNameKO { get; set; }

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
