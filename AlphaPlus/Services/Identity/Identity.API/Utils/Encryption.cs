using Microsoft.VisualBasic;

namespace AlphaPlus.Identity.API.Utils
{
    public interface IEncryption
    {
        string CfsEnCode(string codeStr);
    }

    public class Encryption : IEncryption
    {
        public string CfsEnCode(string codeStr)
        {
            var num = 30;
            var num2 = num - codeStr.Length;
            if (num2 > 1)
            {
                for (var m = 1; m <= num2; m++)
                {
                    codeStr += Strings.Chr(0x15);
                }
            }
            var num3 = 1.0;
            for (var i = 1; i <= num; i++)
            {
                var num5 = num + Strings.Asc(Strings.Mid(codeStr, i, 1)) * i;
                num3 *= num5;
            }
            codeStr = num3.ToString("G15");
            var str = "";
            for (var j = 1; j <= codeStr.Length; j++)
            {
                str += CfsCode(Strings.Mid(codeStr, j, 3));
            }
            var str2 = "";
            for (var k = 20; k <= str.Length - 0x12; k += 2)
            {
                str2 += Strings.Mid(str, k, 1);
            }
            return str2;
        }

        private static string CfsCode(string nWord)
        {
            var number = "";
            for (var i = 1; i <= nWord.Length; i++)
            {
                number += Strings.Asc(Strings.Mid(nWord, i, 1));
            }
            return Conversion.Hex(number);
        }
    }
}