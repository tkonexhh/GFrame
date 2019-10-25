using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace GFrame
{

    public class TimeHelper
    {
        private static DateTime Jan1st1970Ms = new DateTime(1970, 1, 1, 0, 0, 0, 0);


        private static string ConvertToString(double num)
        {
            return Convert.ToInt64(num).ToString();
        }

        public static string GetTimeStamp()
        {
            TimeSpan ts = DateTime.Now - Jan1st1970Ms;
            return ConvertToString(ts.TotalSeconds);
        }

        public static string GetMilliTimeStamp()
        {
            TimeSpan ts = DateTime.Now - Jan1st1970Ms;
            return ConvertToString(ts.TotalMilliseconds);
        }

        public static string GetUTCTimeStamp()
        {
            TimeSpan ts = DateTime.Now.ToUniversalTime() - Jan1st1970Ms;
            return ConvertToString(ts.TotalSeconds);
        }

        public static bool IsBetween(DateTime date, DateTime from, DateTime to)
        {
            return ((from <= date) && (to >= date));
        }


        public static DateTime GetTimeFromTimestamp(string timestamp)
        {
            DateTime dtStart = Jan1st1970Ms;
            return dtStart.AddMilliseconds(long.Parse(timestamp));
        }

        public static long GetLeftFromTimestamp(string timestamp)
        {
            DateTime taget = GetTimeFromTimestamp(timestamp);
            return (long)(taget - DateTime.Now).TotalSeconds;
        }
    }
}




