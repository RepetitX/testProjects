using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Core
{
    using System;

    /// <summary>
    /// Static class containing Fluent <see cref="DateTime"/> extension methods.
    /// </summary>
    public static class NumberExtensions
    {
        public static double FromBytesToMegabytes(this int bytesLength)
        {
            return FromBytesToMegabytes(bytesLength, null);
        }

        public static double FromBytesToMegabytes(this int bytesLength, int? digits)
        {
            const int bytesInMegabyte = 1048576;

            return digits.HasValue ? Math.Round(((double)bytesLength / bytesInMegabyte), digits.Value) : Math.Round(((double)bytesLength / bytesInMegabyte));
        }

        public static string ToNonZeroString(this int number)
        {
            return number > 0 ? number.ToString() : "";
        }

        public static string ToNonZeroString(this double number)
        {
            return number > 0 ? number.ToString() : "";
        }

        public static string ToNonZeroString(this int number, string format)
        {
            return number > 0 ? string.Format(format, number.ToString()) : "";
        }

        public static string ToNonZeroString(this double number, string format)
        {
            return number > 0 ? string.Format(format, number.ToString()) : "";
        }

        /// <summary>
        /// Generates <see cref="TimeSpan"/> value for given number of Years.
        /// </summary>
        public static FluentTimeSpan Years(this int years)
        {
            return new FluentTimeSpan { Years = years };
        }

        /// <summary>
        /// Returns <see cref="TimeSpan"/> value for given number of Months.
        /// </summary>
        public static FluentTimeSpan Months(this int months)
        {
            return new FluentTimeSpan { Months = months };
        }

        /// <summary>
        /// Returns <see cref="TimeSpan"/> for given number of Weeks (number of weeks * 7).
        /// </summary>
        public static FluentTimeSpan Weeks(this int weeks)
        {
            return new FluentTimeSpan { TimeSpan = TimeSpan.FromDays(weeks * 7) };
        }

        /// <summary>
        /// Returns <see cref="TimeSpan"/> for given number of Weeks (number of weeks * 7).
        /// </summary>
        public static FluentTimeSpan Weeks(this double weeks)
        {
            return new FluentTimeSpan { TimeSpan = TimeSpan.FromDays(weeks * 7) };
        }

        /// <summary>
        /// Returns <see cref="TimeSpan"/> for given number of Days.
        /// </summary>
        public static FluentTimeSpan Days(this int days)
        {
            return new FluentTimeSpan { TimeSpan = TimeSpan.FromDays(days) };
        }

        /// <summary>
        /// Returns <see cref="TimeSpan"/> for given number of Days.
        /// </summary>
        public static FluentTimeSpan Days(this double days)
        {
            return new FluentTimeSpan { TimeSpan = TimeSpan.FromDays(days) };
        }

        /// <summary>
        /// Returns <see cref="TimeSpan"/> for given number of Hours.
        /// </summary>
        public static FluentTimeSpan Hours(this int hours)
        {
            return new FluentTimeSpan { TimeSpan = TimeSpan.FromHours(hours) };
        }

        /// <summary>
        /// Returns <see cref="TimeSpan"/> for given number of Hours.
        /// </summary>
        public static FluentTimeSpan Hours(this double hours)
        {
            return new FluentTimeSpan { TimeSpan = TimeSpan.FromHours(hours) };
        }

        /// <summary>
        /// Returns <see cref="TimeSpan"/> for given number of Minutes.
        /// </summary>
        public static FluentTimeSpan Minutes(this int minutes)
        {
            return new FluentTimeSpan { TimeSpan = TimeSpan.FromMinutes(minutes) };
        }

        /// <summary>
        /// Returns <see cref="TimeSpan"/> for given number of Minutes.
        /// </summary>
        public static FluentTimeSpan Minutes(this double minutes)
        {
            return new FluentTimeSpan { TimeSpan = TimeSpan.FromMinutes(minutes) };
        }

        /// <summary>
        /// Returns <see cref="TimeSpan"/> for given number of Seconds.
        /// </summary>
        public static FluentTimeSpan Seconds(this int seconds)
        {
            return new FluentTimeSpan { TimeSpan = TimeSpan.FromSeconds(seconds) };
        }

        /// <summary>
        /// Returns <see cref="TimeSpan"/> for given number of Seconds.
        /// </summary>
        public static FluentTimeSpan Seconds(this double seconds)
        {
            return new FluentTimeSpan { TimeSpan = TimeSpan.FromSeconds(seconds) };
        }

        /// <summary>
        /// Returns <see cref="TimeSpan"/> for given number of Milliseconds.
        /// </summary>
        public static FluentTimeSpan Milliseconds(this int milliseconds)
        {
            return new FluentTimeSpan { TimeSpan = TimeSpan.FromMilliseconds(milliseconds) };
        }

        /// <summary>
        /// Returns <see cref="TimeSpan"/> for given number of Milliseconds.
        /// </summary>
        public static FluentTimeSpan Milliseconds(this double milliseconds)
        {
            return new FluentTimeSpan { TimeSpan = TimeSpan.FromMilliseconds(milliseconds) };
        }

        /// <summary>
        /// Returns <see cref="TimeSpan"/> for given number of ticks.
        /// </summary>
        public static FluentTimeSpan Ticks(this int ticks)
        {
            return new FluentTimeSpan { TimeSpan = TimeSpan.FromTicks(ticks) };
        }

        /// <summary>
        /// Returns <see cref="TimeSpan"/> for given number of ticks.
        /// </summary>
        public static FluentTimeSpan Ticks(this long ticks)
        {
            return new FluentTimeSpan { TimeSpan = TimeSpan.FromTicks(ticks) };
        }
    }
}
