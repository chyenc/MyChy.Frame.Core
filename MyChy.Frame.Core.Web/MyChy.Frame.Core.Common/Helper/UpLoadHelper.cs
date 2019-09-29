using Microsoft.AspNetCore.Http;
using MyChy.Frame.Core.Common.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net.Http.Headers;
using System.Text;

namespace MyChy.Frame.Core.Common.Helper
{
    public class UpLoadHelper
    {
        private UpLoadPostModel Model { get; set; }

        private static readonly IList<string> ImageList = new List<string> { "png", "jpg", "jpeg", "bmp", "gif", "ico" };

        private static readonly IList<string> FileList = new List<string> { "txt", "xls", "xlsx", "ppt", "pptx", "doc", "docx" };

        private static readonly string UploadFormat = "yyyyMM";

        public UpLoadHelper(UpLoadPostModel PostModel)
        {
            Model = PostModel;
        }

        /// <summary>
        /// 保存文件
        /// </summary>
        /// <param name="File"></param>
        /// <returns></returns>
        public UploadReceiveModel Upload(IFormFile File)
        {
            var result = new UploadReceiveModel();
            var fileName = ContentDispositionHeaderValue.Parse(File.ContentDisposition).FileName.Trim('"');
            var checkfile = FileHelper.CheckFileNmae(fileName, out string name, out string exname);
            if (!checkfile)
            {
                result.Code = "001";
                result.Msg = "文件名错误，请重新上传"; return result;
            }
            var checkex = false;
            switch (Model.FileType)
            {
                case UpLoadFileType.Image:
                    checkex = ImageList.Contains(exname);
                    break;
                case UpLoadFileType.File:
                    checkex = FileList.Contains(exname);
                    break;
                case UpLoadFileType.Other:
                    if (Model.ExtensionName != null && Model.ExtensionName.Count > 0)
                    {
                        checkex = Model.ExtensionName.Contains(exname);
                    }
                    break;
            }
            if (!checkex)
            {
                result.Code = "002";
                result.Msg = "文件名错误，请重新上传"; return result;
            }
            result.FileName = name;
            result.ExtensionName = exname;
            var date = DateTime.Now.Ticks.ToString();
            var dateFormat = DateTime.Now.ToString(UploadFormat);
            FileHelper.CreatedFolderData(Model.SavePath, dateFormat, out string filedate);
            result.SavePath = filedate + date + "." + exname;

            using (FileStream fs = System.IO.File.Create(Model.SavePath + result.SavePath))
            {
                File.CopyTo(fs);
                fs.Flush();
            }

            result.Success = true;
            return result;

        }

        /// <summary>
        /// 保持文件
        /// </summary>
        /// <param name="ExtensionName">扩展名</param>
        /// <param name="ms"></param>
        /// <returns></returns>
        public UploadReceiveModel Save(string ExtensionName, MemoryStream ms)
        {
            var result = new UploadReceiveModel();

            var date = DateTime.Now.Ticks.ToString();
            var dateFormat = DateTime.Now.ToString(UploadFormat);
            FileHelper.CreatedFolderData(Model.SavePath, dateFormat, out string filedate);
            result.SavePath = filedate + date + "." + ExtensionName;

            using (FileStream fs = System.IO.File.Create(Model.SavePath + result.SavePath))
            {
                ms.Position = 0;
                var buffer = new byte[1024];
                var bytesRead = 0;
                while ((bytesRead = ms.Read(buffer, 0, buffer.Length)) != 0)
                {
                    fs.Write(buffer, 0, bytesRead);
                }
                fs.Flush();
            }
            result.Success = true;
            return result;
        }
    }

    public enum UpLoadFileType
    {
        /// <summary>
        /// 图片
        /// </summary>
        [Description("图片")]
        Image = 1,
        /// <summary>
        /// 文件
        /// </summary>
        [Description("文件")]
        File = 2,
        /// <summary>
        /// 其他
        /// </summary>
        [Description("其他")]
        Other = 3,
    }

    public class UpLoadPostModel
    {
        public UpLoadFileType FileType { get; set; } = UpLoadFileType.Image;

        /// <summary>
        /// 其他文件扩展名
        /// </summary>
        public IList<string> ExtensionName { get; set; }

        /// <summary>
        /// 默认图片大小
        /// </summary>
        public int MaxFileSize { get; set; } = 1 * 1024 * 1024;

        /// <summary>
        /// 是否生成缩微图
        /// </summary>
        public bool IsThumbnail { get; set; } = false;

        /// <summary>
        /// 保存地址
        /// </summary>
        public string SavePath { get; set; }

    }

    public class UploadReceiveModel : ResultBaseModel
    {
        /// <summary>
        /// 保存地址
        /// </summary>
        public string SavePath { get; set; }

        /// <summary>
        /// 原始文件名称
        /// </summary>
        public string FileName { get; set; }


        /// <summary>
        /// 原始扩展名
        /// </summary>
        public string ExtensionName { get; set; }

    }
}
