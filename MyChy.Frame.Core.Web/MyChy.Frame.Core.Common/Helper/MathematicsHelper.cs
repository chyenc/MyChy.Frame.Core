using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyChy.Frame.Core.Common.Helper
{
    public class MathematicsHelper
    {
        /// <summary>
        /// 角度转换为弧度
        /// </summary>
        /// <param name="lat"></param>
        /// <returns></returns>
        public static double Deg2Rad(double lat)
        {
            return Math.PI * lat / 180.0;
        }

        /// <summary>
        /// 弧度数转换为角度数
        /// </summary>
        /// <param name="lat"></param>
        /// <returns></returns>
        public static double Rad2Deg(double lat)
        {
            return (180.0 / Math.PI) * lat;
        }
    }
}
