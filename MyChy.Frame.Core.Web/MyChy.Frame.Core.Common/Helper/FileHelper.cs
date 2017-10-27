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

        public static string CreatedFolderData(string folder, string date, out string folderData)
        {
            CreatedFolder(folder);
            folderData = "/" + date;
            var res = folder + folderData;
            CreatedFolder(res);
            res = res + "/";
            folderData = folderData + "/";
            return res;
        }

        /// <summary>
        /// 是否存在不存在建立文件夹
        /// </summary>
        /// <param name="files"></param>
        public static void CreatedFolder(string files)
        {
            //是否存在
            if (IsFolder(files)) return;
            try
            {
                //建立文件夹
                Directory.CreateDirectory(files);
            }
            catch
            {
                // ignored
            }
        }

        /// <summary>
        /// 判断这个文件夹是否存在
        /// </summary>
        /// <param name="files"></param>
        /// <returns></returns>
        public static bool IsFolder(string files)
        {
            return files.Length != 0 && Directory.Exists(files);
        }


        /// <summary>
        /// 检查文件名称
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="Name"></param>
        /// <param name="ExtensionName"></param>
        /// <returns></returns>
        public static bool CheckFileNmae(string fileName, out string Name, out string ExtensionName)
        {
            Name = string.Empty;
            ExtensionName = string.Empty;
            var result = false;
            var leg = fileName.LastIndexOf('.');
            if (leg > 0 && leg < (fileName.Length-1))
            {
                result = true;
                Name = fileName.Substring(0, leg);
                ExtensionName = fileName.ToLower().Substring(leg + 1);
            }
            return result;

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
            var filename = Path.Combine(Directory.GetCurrentDirectory(), file);
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
