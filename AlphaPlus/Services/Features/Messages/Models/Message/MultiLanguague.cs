namespace AlphaPlus.Features.Messages.API.Models.Message
{
    using System.Collections.Generic;
    using AlphaPlus.Features.Messages.API.Constants;

    public class MultiLanguague
    {
        internal readonly Dictionary<string, string> Contents = new Dictionary<string, string>();

        public MultiLanguague()
        {
        }

        public MultiLanguague(
            string contentEN,
            string contentCS,
            string contentCH,
            string contentTH,
            string contentJP,
            string contentKO,
            string contentVI)
        {
            Contents.Add(Language.English, contentEN ?? string.Empty);
            Contents.Add(Language.ChineseSimplified, contentCS ?? string.Empty);
            Contents.Add(Language.ChineseTraditional, contentCH ?? string.Empty);
            Contents.Add(Language.Japanese, contentJP ?? string.Empty);
            Contents.Add(Language.Thai, contentTH ?? string.Empty);
            Contents.Add(Language.Korean, contentKO ?? string.Empty);
            Contents.Add(Language.Vietnamese, contentVI ?? string.Empty);
        }

        public string ContentEN
        {
            get
            {
                return GetContentByKey(Language.English);
            }

            set { Contents[Language.English] = value; }
        }

        public string ContentCS
        {
            get
            {
                return GetContentByKey(Language.ChineseSimplified);
            }

            set { Contents[Language.ChineseSimplified] = value; }
        }

        public string ContentCH
        {
            get
            {
                return GetContentByKey(Language.ChineseTraditional);
            }
            
            set
            {
                Contents[Language.ChineseTraditional] = value;
            }
        }

        public string ContentJP
        {
            get
            {
                return GetContentByKey(Language.Japanese);
            }
            
            set
            {
                Contents[Language.Japanese] = value;
            }
        }

        public string ContentTH
        {
            get
            {
                return GetContentByKey(Language.Thai);
            }

            set
            {
                Contents[Language.Thai] = value;
            }
        }

        public string ContentKO
        {
            get
            {
                return GetContentByKey(Language.Korean);
            }
            
            set
            {
                Contents[Language.Korean] = value;
            }
        }

        public string ContentVI
        {
            get
            {
                return GetContentByKey(Language.Vietnamese);
            }

            set
            {
                Contents[Language.Vietnamese] = value;
            }
        }

        public string GetContentByKey(string languageKey)
        {
            if (Contents.ContainsKey(languageKey) && !string.IsNullOrEmpty(Contents[languageKey])) {
                return Contents[languageKey];
            }

            return Contents.ContainsKey(Language.English) ? Contents[Language.English] : string.Empty;
        }
    }
}