using Microsoft.VisualBasic;

namespace Sunplus.Agent.Utilities
{
    public class Encryption
    {
        public static string PearsonEncode(string key)
        {
            if (string.IsNullOrEmpty(key) || key.Length == 1) return key;

            var h = 0L;
            var c = key.Length;
            var keys = key.ToCharArray();

            for (var i = 0; i < c; i++)
            {
                h += keys[i].GetHashCode();
            }

            return h.ToString();
        }

        public static string CfsEnCode(string codeStr)
        {
            var num = 30;
            var num2 = num - codeStr.Length;
            if (num2 > 1)
            {
                for (var m = 1; m <= num2; m++)
                {
                    codeStr = codeStr + Strings.Chr(0x15);
                }
            }
            var num3 = 1.0;
            for (var i = 1; i <= num; i++)
            {
                var num5 = num + Strings.Asc(Strings.Mid(codeStr, i, 1)) * i;
                num3 *= num5;
            }
            codeStr = num3.ToString();
            var str = "";
            for (var j = 1; j <= codeStr.Length; j++)
            {
                str = str + CfsCode(Strings.Mid(codeStr, j, 3));
            }
            var str2 = "";
            for (var k = 20; k <= str.Length - 0x12; k += 2)
            {
                str2 = str2 + Strings.Mid(str, k, 1);
            }
            return str2;
        }

        private static string CfsCode(string nWord)
        {
            var number = "";
            for (var i = 1; i <= nWord.Length; i++)
            {
                number = number + Strings.Asc(Strings.Mid(nWord, i, 1));
            }
            return Conversion.Hex(number);
        }
    }
}