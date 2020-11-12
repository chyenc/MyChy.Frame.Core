using Microsoft.AspNetCore.Http;
using MyChy.Frame.Core.Common.Model;
using MyChy.Frame.Core.Common.Spread;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
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
            Model.IsThumbnail = false;
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
                    if (Model.ThumbnailHigth.Count > 0 && Model.ThumbnailHigth.Count == Model.ThumbnailWith.Count)
                    {
                        if (Model.ThumbnailWith.Count == 1 && 
                            (Model.ThumbnailWith.Contains(0)|| Model.ThumbnailHigth.Contains(0)))
                        {

                        }
                        else
                        {
                            Model.IsThumbnail = true;
                        }
                    }
                    //IsThumbnail = true;
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
        

            if (Model.IsThumbnail)
            {
                result.SavePath = filedate + date + "{0}." + exname;
            }
            else
            {
                result.SavePath = filedate + date + "." + exname;
            }
            var savepath = string.Format(result.SavePath, "");
            var imagespath = Model.SavePath + savepath;
            using (FileStream fs = System.IO.File.Create(imagespath))
            {
                File.CopyTo(fs);
                fs.Flush();
            }
            Thumbnail(imagespath, result.SavePath);
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
            if (Model.IsThumbnail)
            {
                result.SavePath = filedate + date + "{0}." + ExtensionName;
            }
            else
            {
                result.SavePath = filedate + date + "." + ExtensionName;
            }
            var savepath= string.Format(result.SavePath, "");
            var imagespath = Model.SavePath + savepath;
            using (FileStream fs = File.Create(imagespath))
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
            Thumbnail(imagespath, result.SavePath);

            result.Success = true;
            return result;
        }

        /// <summary>
        /// 生成缩微图
        /// </summary>
        /// <param name="imagespath"></param>
        /// <param name="SavePath"></param>
        private void Thumbnail(string imagespath, string SavePath)
        {
            if (Model.FileType == UpLoadFileType.Image && Model.IsThumbnail)
            {
                if (Model.ThumbnailHigth.Count > 0 && Model.ThumbnailHigth.Count == Model.ThumbnailWith.Count)
                {
                    Thumbnail thumbnail = new Thumbnail(imagespath)
                    {
                        FilePath = Model.SavePath
                    };
                    for (int i = 0, count = Model.ThumbnailWith.Count; i < count; i++)
                    {
                        thumbnail.Height = Model.ThumbnailHigth[i];
                        thumbnail.Width = Model.ThumbnailWith[i];
                        var newfile = string.Format(SavePath, $"_{thumbnail.Width}_{thumbnail.Height}");
                        thumbnail.ThumbnailFile(newfile);
                    }
                }
            }
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
        /// 缩微图 宽
        /// </summary>
        public IList<int> ThumbnailWith { get; set; } = new List<int>();

        /// <summary>
        /// 缩微图 高
        /// </summary>
        public IList<int> ThumbnailHigth { get; set; } = new List<int>();

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
