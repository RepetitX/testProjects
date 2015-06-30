using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Common.Core
{
    public enum RoundTo
    {
        Second, Minute, Hour, Day
    }

    /// <summary>
    /// Static class containing Fluent <see cref="DateTime"/> extension methods.
    /// </summary>
    public static class DateTimeExtensions
    {
        public static int BusinessStartHour = 10;
        public static int BusinessEndHour = 18;

        public static int GetBusinessDays(DateTime start, DateTime end)
        {
            if (start.DayOfWeek == DayOfWeek.Saturday)
            {
                start = start.AddDays(2);
            }
            else if (start.DayOfWeek == DayOfWeek.Sunday)
            {
                start = start.AddDays(1);
            }

            if (end.DayOfWeek == DayOfWeek.Saturday)
            {
                end = end.AddDays(-1);
            }
            else if (end.DayOfWeek == DayOfWeek.Sunday)
            {
                end = end.AddDays(-2);
            }

            int diff = (int)end.Subtract(start).TotalDays;

            int result = diff / 7 * 5 + diff % 7 + 1;

            if (end.DayOfWeek < start.DayOfWeek)
            {
                return result - 2;
            }
            else
            {
                return result;
            }
        }

        /// <summary>
        /// Returns the very end of the given day (the last millisecond of the last hour for the given <see cref="DateTime"/>).
        /// </summary>
        public static DateTime EndOfDay(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day, 23, 59, 59, 999);
        }

        /// <summary>
        /// Returns the Start of the given day (the first millisecond of the given <see cref="DateTime"/>).
        /// </summary>
        public static DateTime BeginningOfDay(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day, 0, 0, 0, 0);
        }

        public static DateTime BusinessDate(this DateTime date)
        {
            if (date.DayOfWeek != DayOfWeek.Sunday && date.DayOfWeek != DayOfWeek.Saturday && date.Hour >= BusinessStartHour && date.Hour < BusinessEndHour)
                return date;

            if (date.Hour >= BusinessEndHour)
                date = date.AddDays(1);
            if (date.DayOfWeek == DayOfWeek.Saturday)
                date = date.AddDays(2);
            if (date.DayOfWeek == DayOfWeek.Sunday)
                date = date.AddDays(1);

            return new DateTime(date.Year, date.Month, date.Day, BusinessStartHour, 0, 0);
        }

        /// <summary>
        /// Returns the same date (same Day, Month, Hour, Minute, Second etc) in the next calendar year. 
        /// If that day does not exist in next year in same month, number of missing days is added to the last day in same month next year.
        /// </summary>
        public static DateTime NextYear(this DateTime start)
        {
            var nextYear = start.Year + 1;
            var numberOfDaysInSameMonthNextYear = DateTime.DaysInMonth(nextYear, start.Month);

            if (numberOfDaysInSameMonthNextYear < start.Day)
            {
                var differenceInDays = start.Day - numberOfDaysInSameMonthNextYear;
                var dateTime = new DateTime(nextYear, start.Month, numberOfDaysInSameMonthNextYear, start.Hour, start.Minute, start.Second, start.Millisecond);
                return dateTime + differenceInDays.Days();
            }
            return new DateTime(nextYear, start.Month, start.Day, start.Hour, start.Minute, start.Second, start.Millisecond);
        }

        /// <summary>
        /// Returns the same date (same Day, Month, Hour, Minute, Second etc) in the previous calendar year.
        /// If that day does not exist in previous year in same month, number of missing days is added to the last day in same month previous year.
        /// </summary>
        public static DateTime PreviousYear(this DateTime start)
        {
            var previousYear = start.Year - 1;
            var numberOfDaysInSameMonthPreviousYear = DateTime.DaysInMonth(previousYear, start.Month);

            if (numberOfDaysInSameMonthPreviousYear < start.Day)
            {
                var differenceInDays = start.Day - numberOfDaysInSameMonthPreviousYear;
                var dateTime = new DateTime(previousYear, start.Month, numberOfDaysInSameMonthPreviousYear, start.Hour, start.Minute, start.Second, start.Millisecond);
                return dateTime + differenceInDays.Days();
            }
            return new DateTime(previousYear, start.Month, start.Day, start.Hour, start.Minute, start.Second, start.Millisecond);
        }

        /// <summary>
        /// Returns <see cref="DateTime"/> increased by 24 hours ie Next Day.
        /// </summary>
        public static DateTime NextDay(this DateTime start)
        {
            return start + 1.Days();
        }

        /// <summary>
        /// Returns <see cref="DateTime"/> decreased by 24h period ie Previous Day.
        /// </summary>
        public static DateTime PreviousDay(this DateTime start)
        {
            return start - 1.Days();
        }

        /// <summary>
        /// Returns first next occurrence of specified <see cref="DayOfWeek"/>.
        /// </summary>
        public static DateTime Next(this DateTime start, DayOfWeek day)
        {
            do
            {
                start = start.NextDay();
            }
            while (start.DayOfWeek != day);

            return start;
        }

        /// <summary>
        /// Returns first next occurrence of specified <see cref="DayOfWeek"/>.
        /// </summary>
        public static DateTime Previous(this DateTime start, DayOfWeek day)
        {
            do
            {
                start = start.PreviousDay();
            }
            while (start.DayOfWeek != day);

            return start;
        }


        /// <summary>
        /// Increases supplied <see cref="DateTime"/> for 7 days ie returns the Next Week.
        /// </summary>
        public static DateTime WeekAfter(this DateTime start)
        {
            return start + 1.Weeks();
        }

        /// <summary>
        /// Decreases supplied <see cref="DateTime"/> for 7 days ie returns the Previous Week.
        /// </summary>
        public static DateTime WeekEarlier(this DateTime start)
        {
            return start - 1.Weeks();
        }


        /// <summary>
        /// Increases the <see cref="DateTime"/> object with given <see cref="TimeSpan"/> value.
        /// </summary>
        public static DateTime IncreaseTime(this DateTime startDate, TimeSpan toAdd)
        {
            return startDate + toAdd;
        }

        /// <summary>
        /// Decreases the <see cref="DateTime"/> object with given <see cref="TimeSpan"/> value.
        /// </summary>
        public static DateTime DecreaseTime(this DateTime startDate, TimeSpan toSubtract)
        {
            return startDate - toSubtract;
        }

        /// <summary>
        /// Returns the original <see cref="DateTime"/> with Hour part changed to supplied hour parameter.
        /// </summary>
        public static DateTime SetTime(this DateTime originalDate, int hour)
        {
            return new DateTime(originalDate.Year, originalDate.Month, originalDate.Day, hour, originalDate.Minute, originalDate.Second, originalDate.Millisecond);
        }

        /// <summary>
        /// Returns the original <see cref="DateTime"/> with Hour and Minute parts changed to supplied hour and minute parameters.
        /// </summary>
        public static DateTime SetTime(this DateTime originalDate, int hour, int minute)
        {
            return new DateTime(originalDate.Year, originalDate.Month, originalDate.Day, hour, minute, originalDate.Second, originalDate.Millisecond);
        }

        /// <summary>
        /// Returns the original <see cref="DateTime"/> with Hour, Minute and Second parts changed to supplied hour, minute and second parameters.
        /// </summary>
        public static DateTime SetTime(this DateTime originalDate, int hour, int minute, int second)
        {
            return new DateTime(originalDate.Year, originalDate.Month, originalDate.Day, hour, minute, second, originalDate.Millisecond);
        }

        /// <summary>
        /// Returns the original <see cref="DateTime"/> with Hour, Minute, Second and Millisecond parts changed to supplied hour, minute, second and millisecond parameters.
        /// </summary>
        public static DateTime SetTime(this DateTime originalDate, int hour, int minute, int second, int millisecond)
        {
            return new DateTime(originalDate.Year, originalDate.Month, originalDate.Day, hour, minute, second, millisecond);
        }

        /// <summary>
        /// Returns <see cref="DateTime"/> with changed Hour part.
        /// </summary>
        public static DateTime SetHour(this DateTime originalDate, int hour)
        {
            return new DateTime(originalDate.Year, originalDate.Month, originalDate.Day, hour, originalDate.Minute, originalDate.Second, originalDate.Millisecond);
        }

        /// <summary>
        /// Returns <see cref="DateTime"/> with changed Minute part.
        /// </summary>
        public static DateTime SetMinute(this DateTime originalDate, int minute)
        {
            return new DateTime(originalDate.Year, originalDate.Month, originalDate.Day, originalDate.Hour, minute, originalDate.Second, originalDate.Millisecond);
        }

        /// <summary>
        /// Returns <see cref="DateTime"/> with changed Second part.
        /// </summary>
        public static DateTime SetSecond(this DateTime originalDate, int second)
        {
            return new DateTime(originalDate.Year, originalDate.Month, originalDate.Day, originalDate.Hour, originalDate.Minute, second, originalDate.Millisecond);
        }

        /// <summary>
        /// Returns <see cref="DateTime"/> with changed Millisecond part.
        /// </summary>
        public static DateTime SetMillisecond(this DateTime originalDate, int millisecond)
        {
            return new DateTime(originalDate.Year, originalDate.Month, originalDate.Day, originalDate.Hour, originalDate.Minute, originalDate.Second, millisecond);
        }

        /// <summary>
        /// Returns original <see cref="DateTime"/> value with time part set to midnight (alias for <see cref="BeginningOfDay"/> method).
        /// </summary>
        public static DateTime Midnight(this DateTime value)
        {
            return value.BeginningOfDay();
        }

        /// <summary>
        /// Returns original <see cref="DateTime"/> value with time part set to Noon (12:00:00h).
        /// </summary>
        /// <param name="value">The <see cref="DateTime"/> find Noon for.</param>
        /// <returns>A <see cref="DateTime"/> value with time part set to Noon (12:00:00h).</returns>
        public static DateTime Noon(this DateTime value)
        {
            return value.SetTime(12, 0, 0, 0);
        }

        /// <summary>
        /// Returns <see cref="DateTime"/> with changed Year part.
        /// </summary>
        public static DateTime SetDate(this DateTime value, int year)
        {
            return new DateTime(year, value.Month, value.Day, value.Hour, value.Minute, value.Second, value.Millisecond);
        }

        /// <summary>
        /// Returns <see cref="DateTime"/> with changed Year and Month part.
        /// </summary>
        public static DateTime SetDate(this DateTime value, int year, int month)
        {
            return new DateTime(year, month, value.Day, value.Hour, value.Minute, value.Second, value.Millisecond);
        }

        /// <summary>
        /// Returns <see cref="DateTime"/> with changed Year, Month and Day part.
        /// </summary>
        public static DateTime SetDate(this DateTime value, int year, int month, int day)
        {
            return new DateTime(year, month, day, value.Hour, value.Minute, value.Second, value.Millisecond);
        }

        /// <summary>
        /// Returns <see cref="DateTime"/> with changed Year part.
        /// </summary>
        public static DateTime SetYear(this DateTime value, int year)
        {
            return new DateTime(year, value.Month, value.Day, value.Hour, value.Minute, value.Second, value.Millisecond);
        }

        /// <summary>
        /// Returns <see cref="DateTime"/> with changed Month part.
        /// </summary>
        public static DateTime SetMonth(this DateTime value, int month)
        {
            return new DateTime(value.Year, month, value.Day, value.Hour, value.Minute, value.Second, value.Millisecond);
        }

        /// <summary>
        /// Returns <see cref="DateTime"/> with changed Day part.
        /// </summary>
        public static DateTime SetDay(this DateTime value, int day)
        {
            return new DateTime(value.Year, value.Month, day, value.Hour, value.Minute, value.Second, value.Millisecond);
        }

        /// <summary>
        /// Determines whether the specified <see cref="DateTime"/> is before then current value.
        /// </summary>
        /// <param name="current">The current value.</param>
        /// <param name="toCompareWith">Value to compare with.</param>
        /// <returns>
        /// 	<c>true</c> if the specified current is before; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsBefore(this DateTime current, DateTime toCompareWith)
        {
            return current < toCompareWith;
        }

        /// <summary>
        /// Determines whether the specified <see cref="DateTime"/> value is After then current value.
        /// </summary>
        /// <param name="current">The current value.</param>
        /// <param name="toCompareWith">Value to compare with.</param>
        /// <returns>
        /// 	<c>true</c> if the specified current is after; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsAfter(this DateTime current, DateTime toCompareWith)
        {
            return current > toCompareWith;
        }

        /// <summary>
        /// Returns the given <see cref="DateTime"/> with hour and minutes set At given values.
        /// </summary>
        /// <param name="current">The current <see cref="DateTime"/> to be changed.</param>
        /// <param name="hour">The hour to set time to.</param>
        /// <param name="minute">The minute to set time to.</param>
        /// <returns><see cref="DateTime"/> with hour and minute set to given values.</returns>
        public static DateTime At(this DateTime current, int hour, int minute)
        {
            return current.SetTime(hour, minute);
        }

        /// <summary>
        /// Returns the given <see cref="DateTime"/> with hour and minutes and seconds set At given values.
        /// </summary>
        /// <param name="current">The current <see cref="DateTime"/> to be changed.</param>
        /// <param name="hour">The hour to set time to.</param>
        /// <param name="minute">The minute to set time to.</param>
        /// <param name="second">The second to set time to.</param>
        /// <returns><see cref="DateTime"/> with hour and minutes and seconds set to given values.</returns>
        public static DateTime At(this DateTime current, int hour, int minute, int second)
        {
            return current.SetTime(hour, minute, second);
        }

        /// <summary>
        /// Sets the day of the <see cref="DateTime"/> to the first day in that month.
        /// </summary>
        /// <param name="current">The current <see cref="DateTime"/> to be changed.</param>
        /// <returns>given <see cref="DateTime"/> with the day part set to the first day in that month.</returns>
        public static DateTime FirstDayOfMonth(this DateTime current)
        {
            return current.SetDay(1);
        }

        /// <summary>
        /// Sets the day of the <see cref="DateTime"/> to the last day in that month.
        /// </summary>
        /// <param name="current">The current DateTime to be changed.</param>
        /// <returns>given <see cref="DateTime"/> with the day part set to the last day in that month.</returns>
        public static DateTime LastDayOfMonth(this DateTime current)
        {
            return current.SetDay(DateTime.DaysInMonth(current.Year, current.Month));
        }


        /// <summary>
        /// Adds the given number of business days to the <see cref="DateTime"/>.
        /// </summary>
        /// <param name="current">The date to be changed.</param>
        /// <param name="days">Number of business days to be added.</param>
        /// <returns>A <see cref="DateTime"/> increased by a given number of business days.</returns>
        public static DateTime AddBusinessDays(this DateTime current, int days)
        {
            var sign = Math.Sign(days);
            var unsignedDays = Math.Abs(days);
            for (var i = 0; i < unsignedDays; i++)
            {
                do
                {
                    current = current.AddDays(sign);
                }
                while (current.DayOfWeek == DayOfWeek.Saturday ||
                       current.DayOfWeek == DayOfWeek.Sunday);
            }
            return current;
        }

        public static DateTime AddBusinessHours(this DateTime current, int hours)
        {
            var sign = Math.Sign(hours);
            var unsignedHours = Math.Abs(hours);
            for (var i = 0; i < unsignedHours; i++)
            {
                current = current.AddHours(sign);

                if (sign > 0 && current.Hour > BusinessEndHour)
                    current = current.AddBusinessDays(1).SetHour(BusinessStartHour + 1);
                if (sign < 0 && current.Hour < BusinessStartHour)
                    current = current.AddBusinessDays(-1).SetHour(BusinessEndHour - 1);
            }
            return current;
        }

        public static int GetBusinessHoursDuration(DateTime startdate, DateTime enddate)
        {
            if (startdate > enddate)
                throw new ArgumentException("Дата начала должна быть раньше даты окончания!");

            startdate = startdate.SetMinute(0).SetSecond(0);
            enddate = enddate.SetMinute(0).SetSecond(0);
            if (enddate.Hour > BusinessEndHour) enddate = enddate.SetHour(BusinessEndHour);

            var current = startdate;
            var hours = 0;
            
            if (current.DayOfWeek == DayOfWeek.Saturday)
                current = current.AddDays(1);
            if (current.DayOfWeek == DayOfWeek.Sunday)
                current = current.AddDays(1);

            while (current < enddate)
            {
                current = current.AddHours(1);

                if (current.Hour > BusinessEndHour)
                    current = current.AddBusinessDays(1).SetHour(BusinessStartHour);
                else
                    hours++;
            }
            return hours;
        }

        /// <summary>
        /// Subtracts the given number of business days to the <see cref="DateTime"/>.
        /// </summary>
        /// <param name="current">The date to be changed.</param>
        /// <param name="days">Number of business days to be subtracted.</param>
        /// <returns>A <see cref="DateTime"/> increased by a given number of business days.</returns>
        public static DateTime SubtractBusinessDays(this DateTime current, int days)
        {
            return AddBusinessDays(current, -days);
        }


        /// <summary>
        /// Determine if a <see cref="DateTime"/> is in the future.
        /// </summary>
        /// <param name="dateTime">The date to be checked.</param>
        /// <returns><c>true</c> if <paramref name="dateTime"/> is in the future; otherwise <c>false</c>.</returns>
        public static bool IsInFuture(this DateTime dateTime)
        {
            return dateTime > DateTime.Now;
        }


        /// <summary>
        /// Determine if a <see cref="DateTime"/> is in the past.
        /// </summary>
        /// <param name="dateTime">The date to be checked.</param>
        /// <returns><c>true</c> if <paramref name="dateTime"/> is in the past; otherwise <c>false</c>.</returns>
        public static bool IsInPast(this DateTime dateTime)
        {
            return dateTime < DateTime.Now;
        }



        public static DateTime Round(this DateTime dateTime, RoundTo rt)
        {
            DateTime rounded;

            switch (rt)
            {
                case RoundTo.Second:
                    {
                        rounded = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, dateTime.Second);
                        if (dateTime.Millisecond >= 500)
                        {
                            rounded = rounded.AddSeconds(1);
                        }
                        break;
                    }
                case RoundTo.Minute:
                    {
                        rounded = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, 0);
                        if (dateTime.Second >= 30)
                        {
                            rounded = rounded.AddMinutes(1);
                        }
                        break;
                    }
                case RoundTo.Hour:
                    {
                        rounded = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, 0, 0);
                        if (dateTime.Minute >= 30)
                        {
                            rounded = rounded.AddHours(1);
                        }
                        break;
                    }
                case RoundTo.Day:
                    {
                        rounded = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 0, 0, 0);
                        if (dateTime.Hour >= 12)
                        {
                            rounded = rounded.AddDays(1);
                        }
                        break;
                    }
                default:
                    {
                        throw new NotImplementedException();
                    }
            }

            return rounded;
        }

        /// <summary>
        /// Returns a DateTime adjusted to the beginning of the week.
        /// </summary>
        /// <param name="dateTime">The DateTime to adjust</param>
        /// <returns>A DateTime instance adjusted to the beginning of the current week</returns>
        /// <remarks>the beginning of the week is controlled by the current Culture</remarks>
        public static DateTime StartOfWeek(this DateTime dateTime)
        {
            var ci = Thread.CurrentThread.CurrentCulture;
            var fdow = ci.DateTimeFormat.FirstDayOfWeek;
            return DateTime.Today.AddDays(-(DateTime.Today.DayOfWeek - fdow));
        }

        public static int Quarter(this DateTime date)
        {
            return (int)Math.Floor((((decimal)date.Month - 1) / 3)) + 1;
        }

        public static string ToHumanDateTimeString(this DateTime date)
        {
            if (date.Year == DateTime.Now.Year)
                return date.ToString("dd MMM, HH:mm");
            else
                return date.ToString("dd.MM.yyyy, HH:mm");
        }

        public static int MonthDifference(this DateTime dateTime, DateTime compareDateTime)
        {
            return (dateTime.Month - compareDateTime.Month) + 12 * (dateTime.Year - compareDateTime.Year);
        }

        public static int AbsoluteMonthDifference(this DateTime dateTime, DateTime compareDateTime)
        {
            return Math.Abs((dateTime.Month - compareDateTime.Month) + 12 * (dateTime.Year - compareDateTime.Year));
        }
    }
}
