using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApiTester
{
    public class UtcHelper
    {
        private static readonly DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        public static DateTime FromUnixTime(long unixTime)
        {
            return epoch.AddMilliseconds(unixTime);
        }

        public static long ToUnixTime(DateTime date)
        {
            return Convert.ToInt64((date - epoch).TotalMilliseconds);
        }
    }
}