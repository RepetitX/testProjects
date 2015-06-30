using System.Globalization;

namespace Common.Core
{
    public static class FormattingExtensions
    {
        public static readonly NumberFormatInfo MoneyNumberFormat = new NumberFormatInfo
            {
                NumberGroupSeparator = ",",
                NumberDecimalSeparator = ".",
                NumberDecimalDigits = 2
            };

        public static string TrimTo(this string value, int characters)
        {
            if (value != null && characters > 0 && value.Length > characters) return value.Substring(0, characters - 3) + "...";

            return value;
        }

        public static string ToMoneyString(this decimal number)
        {
            return number.ToMoneyString(true);
        }

        public static string ToMoneyString(this decimal number, bool keepZero)
        {
            return number != 0 ? number.ToString("### ### ### ##0.##") : " ";
        }

        public static string ToHourString(this decimal number)
        {
            return number.ToString("#0.##");
        }

        public static string AsFormat(this string str, params string[] values)
        {
            return string.Format(str, values);
        }
    }
}
