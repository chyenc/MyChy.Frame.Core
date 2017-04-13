using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MyChy.Frame.Core.Common.Helper
{
    public class FileHelper
    {
        /// <summary>
        /// 根据完整文件路径获取FileStream
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static FileStream GetFileStream(string fileName)
        {
            FileStream fileStream = null;
            if (!string.IsNullOrEmpty(fileName) && File.Exists(fileName))
            {
                fileStream = new FileStream(fileName, FileMode.Open);
            }
            return fileStream;
        }

        /// <summary>
        /// 根据完整文件路径获取FileStream
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static FileStream GetReadFileStream(string fileName)
        {
            FileStream fileStream = null;
            if (!string.IsNullOrEmpty(fileName) && File.Exists(fileName))
            {
                fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            }
            return fileStream;
        }

        ///// <summary>
        ///// 从Url下载文件
        ///// </summary>
        ///// <param name="url"></param>
        ///// <param name="fullFilePathAndName"></param>
        //public void DownLoadFileFromUrl(string url, string fullFilePathAndName)
        //{
        //    using (FileStream fs = new FileStream(fullFilePathAndName, FileMode.OpenOrCreate))
        //    {
        //        HttpUtility.Get.Download(url, fs);
        //        fs.Flush(true);
        //    }
        //}

        /// <summary>
        /// 判断文件是否存在 真 存在
        /// </summary>
        /// <param name="files"></param>
        /// <returns></returns>
        public static bool IsFile(string files)
        {
            return files.Length != 0 && File.Exists(files);
        }


        /// <summary>
        /// 返回文件的物理地址
        /// </summary>
        /// <param name="file"></param>
        public static string GetFileMapPath(string file)
        {
            //var context = HttpContext.Current;
            //string filename;
            //if (context != null)
            //{
            //    filename = context.Server.MapPath(file);
            //    if (!IsFile(filename))
            //    {
            //        filename = context.Server.MapPath("/" + file);
            //    }
            //}
            //else
            //{
              var  filename = Path.Combine(Directory.GetCurrentDirectory(), file);
          //  }
            return filename;
        }

        /// <summary>
        /// 返回文件扩展名
        /// </summary>
        /// <param name="files"></param>
        /// <returns></returns>
        public static string GetExtension(string files)
        {
            return (Path.GetExtension(files));
        }

    }
}
