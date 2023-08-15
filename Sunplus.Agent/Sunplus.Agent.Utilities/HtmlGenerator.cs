namespace Sunplus.Agent.Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class HtmlGenerator
    {
        #region Combobox

        public static string ComboBox(string id, object selectedValue, IEnumerable<object> dataSource, string onchange)
        {
            StringBuilder builder = new StringBuilder(200);

            builder.Append("<select id=\"" + id + "\" name=\"" + id + "\"");

            if (null != onchange)
            {
                builder.Append(" onchange=\"");
                builder.Append(onchange);
            }

            builder.Append("\">");

            foreach (object value in dataSource)
            {
                builder.Append("<option value=\"");
                builder.Append(value);
                builder.Append("\"");

                if (value.Equals(selectedValue))
                {
                    builder.Append(" selected");
                }

                builder.Append(">");
                builder.Append(value);
                builder.Append("</option>");
            }

            builder.Append("</select>");

            return builder.ToString();
        }

        public static string ComboBox(string id, double selectedValue, double min, double max, double step, bool withButton, string onchange)
        {
            return ComboBox(id, selectedValue, min, max, step, withButton, onchange);
        }

        public static string ComboBox(string id, double selectedValue, bool disabled, double min, double max, double step, string onchange)
        {
            if (0 == step) throw new DivideByZeroException();

            StringBuilder builder = new StringBuilder(200);

            builder.Append("<select id=\"" + id + "\" name=\"" + id + "\"" + (disabled == true ? "disabled='true'" : string.Empty));

            if (null != onchange)
            {
                builder.Append(" onchange=\"");
                builder.Append(onchange);
            }

            builder.Append("\">");

            var hasSelected = false;
            var maxEqualMin = false;

            if (min <= max)
            {

                if (max == min)
                    maxEqualMin = true;

                var tmp = max == min ? min + step : max;

                int c = (int)(Math.Round((tmp - min) / step));

                for (int i = c; i >= 0; i--)
                {
                    double f = min + (i * step);

                    if (maxEqualMin)
                    {
                        f = max + (i * step) - step;
                    }

                    if (f < 0 || f < min)
                        break;

                    builder.Append("<option value=\"");
                    builder.Append(f);
                    builder.Append("\"");

                    if (Math.Abs(selectedValue - f) < 0.000000001)
                    {
                        builder.Append(" selected");
                        hasSelected = true;
                    }

                    builder.Append(">");
                    builder.Append(f);
                    builder.Append("</option>");
                }
            }

            if (selectedValue > max || !hasSelected)
            {
                builder.Append("<option value=\"");
                builder.Append(selectedValue);
                builder.Append("\"");
                builder.Append(" selected");
                builder.Append(">");
                builder.Append(selectedValue);
                builder.Append("</option>");
            }

            builder.Append("</select>");

            return builder.ToString();
        }

        #endregion
    }
}
