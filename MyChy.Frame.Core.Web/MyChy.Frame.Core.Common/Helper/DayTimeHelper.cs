using MyChy.Frame.Core.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyChy.Frame.Core.Common.Helper
{
    public static class DayTimeHelper
    {
        /// <summary>
        /// 计算星期几
        /// </summary>
        /// <param name="oneWeek"></param>
        /// <param name="week"></param>
        /// <returns></returns>
        public static DateTime OneDayOfWeek(int oneWeek, DayOfWeek week)
        {
            var weekday = oneWeek * 7;
            var days = (int)week;
            if (days == 0) days = 7;
            var dayweek = DateTime.Now.DayOfWeek;
            var days1 = (int)dayweek;
            if (days1 == 0) days1 = 7;
            var addday = weekday + days - days1;
            return DateTime.Now.AddDays(addday).Date;
        }

        /// <summary>
        /// 当天剩余秒数
        /// </summary>
        /// <returns></returns>
        public static int SecondsRemainingDay()
        {
            var ts1 = new TimeSpan(DateTime.Now.Ticks);
            var ts2 = new TimeSpan(DateTime.Now.AddDays(1).Date.Ticks);
            var ts = ts1.Subtract(ts2).Duration();
            return ts.TotalSeconds.To<int>() - 1;
        }


        /// <summary>
        /// Ticks 时间截转换成时间
        /// </summary>
        /// <param name="ticks"></param>
        /// <returns></returns>
        public static DateTime ChangeTicks(long ticks)
        {
            if (ticks < 621356256000000000)
            {
                if (ticks.ToString().Length > 16)
                {
                    ticks = (ticks * 10000);
                }
                else if (ticks.ToString().Length >= 13)
                {
                    ticks = (ticks * 10000);
                }
                else
                {
                    ticks = (ticks * 10000 * 1000);
                }
                ticks += 621356256000000000;
            }
            var datetime = new DateTime(ticks);
            return datetime;
        }


        /// <summary>
        /// 判断时间截 兼容Java 
        /// </summary>
        /// <param name="ticks"></param>
        /// <param name="minutes"></param>
        /// <returns></returns>
        public static bool CheckTicks(long ticks, int minutes = 10)
        {
            var datetime = ChangeTicks(ticks);
            return (datetime <= DateTime.Now.AddMinutes(minutes) 
                && datetime >= DateTime.Now.AddMinutes(0 - minutes));
        }


        /// <summary>
        /// 计算时间截差值 秒
        /// </summary>
        /// <param name="ticks"></param>
        /// <returns></returns>
        public static int CalculatingDifferenceTicksSecond(long ticks)
        {
            var ts1 = new TimeSpan(DateTime.Now.Ticks);
            var ts2 = new TimeSpan(ticks);
            var ts = ts2.Subtract(ts1).Duration();
            return ts.TotalSeconds.To<int>();

        }


        /// <summary>
        /// 计算时间截差值 毫秒
        /// </summary>
        /// <param name="ticks"></param>
        /// <returns></returns>
        public static int CalculatingDifferenceTicksMillisecond(long ticks)
        {
            var ts1 = new TimeSpan(DateTime.Now.Ticks);
            var ts2 = new TimeSpan(ticks);
            var ts = ts2.Subtract(ts1).Duration();
            return ts.TotalMilliseconds.To<int>();

        }

        /// <summary>
        /// 计算时间截差值 秒
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static int CalculatingDifferenceSecond(DateTime time)
        {
            var ts1 = new TimeSpan(DateTime.Now.Ticks);
            var ts2 = new TimeSpan(time.Ticks);
            var ts = ts2.Subtract(ts1).Duration();
            return ts.TotalSeconds.To<int>();

        }


        /// <summary>
        /// 计算时间截差值 TimeSpan
        /// </summary>
        /// <param name="seconds">秒</param>
        /// <returns></returns>
        public static TimeSpan CalculatingDifferenceSecond(double seconds)
        {
            var ts1 = new TimeSpan(DateTime.Now.Ticks);
            var ts2 = new TimeSpan(DateTime.Now.AddSeconds(seconds).Ticks);
            var ts = ts2.Subtract(ts1).Duration();
            return ts;

        }
    }
}
