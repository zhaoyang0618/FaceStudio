using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Face.Web.Utils
{
    public class TimeHelper
    {
        public static DateTime UnixTime2DateTime(long time)
        {
            var startTime = new System.DateTime(1970, 1, 1);//TimeZone.CurrentTimeZone.ToLocalTime();
            var TranslateDate = startTime.AddMilliseconds(time);
            return TranslateDate;
        }

        public static double DateTime2Unix(DateTime time)
        {
            return ((time.ToUniversalTime().Ticks - 621355968000000000) / 10000000);
        }
    }
}