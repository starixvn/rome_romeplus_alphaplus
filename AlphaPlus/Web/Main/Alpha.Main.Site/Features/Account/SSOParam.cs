using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Alpha.Main.Site.Features.Account
{
    public class SSOParam
    {
        public int CustId { get; set; }

        public int SubAccId { get; set; }

        public long LoginTime { get; set; }

        public string EncryptPassword { get; set; }

        public string Nickname { get; set; }

        public bool IsNickname { get; set; }

        public string Language { get; set; }

        public bool NumberCrossCookie { get; set; }

        public string DeviceId { get; set; }

        public string BrowserSize { get; set; }

        public string EncryptCode { get; set; }

        public string DarkModeName { get; set; }

        public string AssetAppPath { get; set; }

        public string UMToken { get; set; }
    }
}