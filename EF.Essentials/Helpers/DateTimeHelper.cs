using System;

namespace EF.Essentials.Helpers
{
    public class DateTimeHelper
    {
        public static DateTime UnixTimeStampToDateTime(long unixTimeStamp)
        {
            return DateTime.UnixEpoch.AddSeconds(unixTimeStamp).ToUniversalTime();
        }

        public static uint DateTimeToUnix(DateTime dt)
        {
            return (uint) dt.ToUniversalTime().Subtract(DateTime.UnixEpoch).TotalSeconds;
        }

        public static string DateTimeToBase64(DateTime dt)
        {
            var unix = DateTimeToUnix(dt);
            return Convert.ToBase64String(BitConverter.GetBytes(unix)).TrimEnd('=', 'A');
        }

        public static DateTime NextDateForDay(DayOfWeek day, DateTime? start = null, bool includeToday = false)
        {
            var date = (start ?? DateTime.UtcNow).AddDays(includeToday ? 0 : 1);
            while (date.DayOfWeek != day)
            {
                date = date.AddDays(1);
            }

            return new DateTime(date.Year, date.Month, date.Day, 0, 0, 0);
        }

        public static DateTime LastDateForDay(DayOfWeek day, DateTime? start = null, bool includeToday = false)
        {
            var date = (start ?? DateTime.UtcNow).AddDays(includeToday ? 0 : -1);
            while (date.DayOfWeek != day)
            {
                date = date.AddDays(-1);
            }

            return new DateTime(date.Year, date.Month, date.Day, 0, 0, 0);
        }

        public static DateTime ThisMonth
        {
            get
            {
                var d = DateTime.UtcNow;
                return new DateTime(d.Year, d.Month, 1);
            }
        }

        public static DateTime Today
        {
            get
            {
                var d = DateTime.UtcNow;
                return new DateTime(d.Year, d.Month, d.Day);
            }
        }
    }
}
