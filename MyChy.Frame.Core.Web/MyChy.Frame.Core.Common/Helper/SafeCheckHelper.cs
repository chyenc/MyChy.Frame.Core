using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyChy.Frame.Core.Common.Helper
{
    public sealed class SafeCheckHelper
    {
        #region 图片验证

        /// <summary>
        /// 判断是否为图片
        /// </summary>
        /// <param name="files"></param>
        /// <returns></returns>
        public static bool IsJpgStr(string files)
        {
            const string namex = ".jpg|.gif|.jpeg|.png|.bmp";
            return IsJpgStr(files, namex);
        }

        public static bool IsJpgStr(string files, string ext)
        {
            return IsStr(files, ext);
        }

        #endregion

        #region Flash  验证

        /// <summary>
        /// 判断是否为其他文件
        /// </summary>
        /// <param name="files"></param>
        /// <param name="namex"></param>
        /// <returns></returns>
        public static bool IsOther(string files, string namex)
        {
            const string name = ".txt|.zip|.rar|.doc|.docx|.xlsx|.xls|.ppt|.pdf|.inf";
            var result = IsStr(files, string.IsNullOrEmpty(namex) ? name : namex);
            return result;

        }

        #endregion

        #region 文件验证

        /// <summary>
        /// 判断是否为Falsh
        /// </summary>
        /// <param name="files"></param>
        /// <returns></returns>
        public static bool IsSwfstr(string files)
        {
            const string namex = ".swf|.fla";
            return IsStr(files, namex);
        }

        #endregion


        #region 私有方法
        /// <summary>
        /// 文件验证
        /// </summary>
        /// <param name="Files">文件名</param>
        /// <param name="ext">扩展名</param>
        /// <returns></returns>
        private static bool IsStr(string Files, string ext)
        {
            try
            {
                var namex = ext;
                var ex = (FileHelper.GetExtension(Files)).ToLower();
                return namex.Split('|').Any(extname => ex == extname);
            }
            catch
            {
                return false;
            }

        }


        #endregion

    }
}
