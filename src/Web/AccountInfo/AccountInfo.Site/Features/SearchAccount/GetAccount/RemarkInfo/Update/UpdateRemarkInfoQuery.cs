using AlphaPlus.Core;
using MediatR;
using AlphaPlus.Core.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace AlphaPlus.AccountInfo.Site.Features.SearchAccount.GetAccount.RemarkInfo.Update
{
    public class UpdateRemarkInfoQuery : QueryBase, IRequest<Result>
    {
        public int SearchAccountId { get; set; }

        [StringLength(200, ErrorMessage = "Remark cannot exceed 200 characters")]
        public string Remark { get; set; }

        public bool IsValidRemark()
        {
            if (string.IsNullOrWhiteSpace(Remark))
            {
                return true;
            }

            var dangerousPatterns = new List<string>
            {
                @"<script\b[^<]*(?:(?!<\/script>)<[^<]*)*<\/script>",
                @"<meta[^>]*http-equiv\s*=\s*[""']refresh[""']",
                @"javascript:",
                @"on\w+\s*=",
                @"<iframe\b",
                @"<object\b",
                @"<embed\b"
            };

            return !dangerousPatterns.Exists(pattern =>
            {
                try
                {
                    return Regex.IsMatch(Remark, pattern, RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(1000));
                }
                catch (RegexMatchTimeoutException)
                {
                    return true;
                }
            });
        }
    }
}