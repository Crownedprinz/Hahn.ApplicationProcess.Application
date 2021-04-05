using System;

namespace  HAF.Domain
{
    /// <summary>Helper class that provides common values for the cron expressions.</summary>
    public static class Cron
    {
        /// <summary>Returns cron expression that fires every day at the specified hour and minute in UTC.</summary>
        /// <param name="hour">The hour in which the schedule will be activated (0-23).</param>
        /// <param name="minute">The minute in which the schedule will be activated (0-59).</param>
        public static string Daily(int hour = 0, int minute = 0) => $"{minute} {hour} * * *";

        /// <summary>Returns cron expression that fires every &lt;<paramref name="interval"></paramref>&gt; days.</summary>
        /// <param name="interval">The number of days to wait between every activation.</param>
        public static string DayInterval(int interval) => $"0 0 */{interval} * *";

        /// <summary>Returns cron expression that fires every &lt;<paramref name="interval"></paramref>&gt; hours.</summary>
        /// <param name="interval">The number of hours to wait between every activation.</param>
        public static string HourInterval(int interval) => $"0 */{interval} * * *";

        /// <summary>Returns cron expression that fires every hour at the specified minute.</summary>
        /// <param name="minute">The minute in which the schedule will be activated (0-59).</param>
        public static string Hourly(int minute = 0) => $"{minute} * * * *";

        /// <summary>Returns cron expression that fires every &lt;<paramref name="interval"></paramref>&gt; minutes.</summary>
        /// <param name="interval">The number of minutes to wait between every activation.</param>
        public static string MinuteInterval(int interval) => $"*/{interval} * * * *";

        /// <summary>Returns cron expression that fires every minute.</summary>
        public static string Minutely() => "* * * * *";

        /// <summary>Returns cron expression that fires every &lt;<paramref name="interval"></paramref>&gt; months.</summary>
        /// <param name="interval">The number of months to wait between every activation.</param>
        public static string MonthInterval(int interval) => $"0 0 1 */{interval} *";

        /// <summary>Returns cron expression that fires every month at the specified day of month, hour and minute in UTC.</summary>
        /// <param name="day">The day of month in which the schedule will be activated (1-31).</param>
        /// <param name="hour">The hour in which the schedule will be activated (0-23).</param>
        /// <param name="minute">The minute in which the schedule will be activated (0-59).</param>
        public static string Monthly(int day = 1, int hour = 0, int minute = 0) => $"{minute} {hour} {day} * *";

        /// <summary>Returns cron expression that fires every week at the specified day of week, hour and minute in UTC.</summary>
        /// <param name="dayOfWeek">The day of week in which the schedule will be activated.</param>
        /// <param name="hour">The hour in which the schedule will be activated (0-23).</param>
        /// <param name="minute">The minute in which the schedule will be activated (0-59).</param>
        public static string Weekly(DayOfWeek dayOfWeek = DayOfWeek.Monday, int hour = 0, int minute = 0) =>
            $"{minute} {hour} * * {dayOfWeek}";

        /// <summary>Returns cron expression that fires every year at the specified month, day, hour and minute in UTC.</summary>
        /// <param name="month">The month in which the schedule will be activated (1-12).</param>
        /// <param name="day">The day of month in which the schedule will be activated (1-31).</param>
        /// <param name="hour">The hour in which the schedule will be activated (0-23).</param>
        /// <param name="minute">The minute in which the schedule will be activated (0-59).</param>
        public static string Yearly(int month = 1, int day = 1, int hour = 0, int minute = 0) =>
            $"{minute} {hour} {day} {month} *";
    }
}