using System;
using System.Collections.Generic;
using System.Text;

namespace X_Service.Util {
    public class TimeHelper {


        public static DateTime UTCToDateTime ( double l ) {
            DateTime dtZone = new DateTime(1970 , 1 , 1 , 0 , 0 , 0);
            dtZone = dtZone.AddSeconds(l);
            return dtZone.ToLocalTime();
        }
        //WINDOWS时间转UTC时间
        public static DateTime DateTimeToUTC ( DateTime vDate ) {
            TimeZone tz = TimeZone.CurrentTimeZone;
            vDate = vDate.ToUniversalTime();
            DateTime dtZone = new DateTime(1970 , 1 , 1 , 0 , 0 , 0);
            //return vDate.Subtract(dtZone).TotalSeconds;
            return vDate;
        }
        
        /// <summary>
        /// 将Unix时间戳转换为DateTime类型时间
        /// </summary>
        /// <param name="d">double 型数字</param>
        /// <returns>DateTime</returns>
        public static DateTime ConvertIntDateTime ( double d ) {
            DateTime time = System.DateTime.MinValue;
            DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970 , 1 , 1));
            time = startTime.AddSeconds(d);
            return time;
        }

        /// <summary>
        /// 将c# DateTime时间格式转换为Unix时间戳格式
        /// </summary>
        /// <param name="time">时间</param>
        /// <returns>double</returns>
        public static int ConvertDateTimeInt ( DateTime time ) {
            int intResult = 0;
            DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970 , 1 , 1));
            intResult = (int)( time - startTime ).TotalSeconds;
            return intResult;
        }


    }
}
