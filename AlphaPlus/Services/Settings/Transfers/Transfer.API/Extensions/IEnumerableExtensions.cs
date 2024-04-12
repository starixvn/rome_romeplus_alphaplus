namespace AlphaPlus.Settings.Transfer.API.Extensions
{
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public static class IEnumerableExtensions
    {
        public static IEnumerable<SelectListItem> GetSelectListItems(this string[] columns)
            => columns.Select((c, i) => new SelectListItem { Value = (i + 1).ToString(), Text = c });

        public static string ToSeparatedString(this IEnumerable<string> l)
            => string.Join(", ", l);
    }
}