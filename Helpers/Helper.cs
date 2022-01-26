using System;
using System.Globalization;
using System.Threading;

namespace phd_api.Helpers
{
    public class Helper
    {
        public Helper() { }
        public static long ConvertToTimestamp(DateTime value)
        {
            long epoch = (value.Ticks - 621355968000000000) / 10000000;
            return epoch;
        }

        public static string StrtTimestampToDateStr(string timestamp)
        {
            long timestampLong = long.Parse(timestamp);
            
            return DateTimeOffset.FromUnixTimeMilliseconds(timestampLong).DateTime.ToString("dd-MMM-yyyy HH:mm:ss", new CultureInfo("en-US"));
        }
    }
}
