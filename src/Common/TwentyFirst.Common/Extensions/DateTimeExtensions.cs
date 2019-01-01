namespace TwentyFirst.Common.Extensions
{
    using System;
    using System.Globalization;

    public static class DateTimeExtensions
    {
        /// <summary>
        /// Gets DateTime by UTC and convert it to EST.
        /// </summary>
        /// <param name="utcDateTime"></param>
        /// <returns></returns>
        public static DateTime UtcToEst(this DateTime utcDateTime)
        {
            var eEuropeZone = TimeZoneInfo.FindSystemTimeZoneById("E. Europe Standard Time");

            var eEuropeDateTime = TimeZoneInfo.ConvertTimeFromUtc(utcDateTime, eEuropeZone);
            if (eEuropeZone.IsDaylightSavingTime(utcDateTime))
            {
                eEuropeDateTime = eEuropeDateTime.AddHours(1);
            }

            return eEuropeDateTime;
        }

        /// <summary>
        /// Gets DateTime and convert it to formatted string.
        /// Format: "dd MMM yyyy hh:mm" in bulgarian
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static string ToFormattedString(this DateTime dateTime)
        {
            var formattedString = dateTime.ToString("hh:mm, dd MMMM yyyy ", new CultureInfo("bg"));
            return formattedString;
        }
    }
}
