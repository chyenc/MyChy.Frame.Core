using MyChy.Frame.Core.Common.Helper;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using System.Threading;

namespace MyChy.Frame.Core.Common.Spread
{
    public sealed class Thumbnail
    {
        /// <summary>
        /// 历史图片地址
        /// </summary>
        private string ImagesPath { get; set; }

        /// <summary>
        /// 压缩图片地址
        /// </summary>
        private string ThumbnailPath { get; set; } 

        public Thumbnail(string file)
        {
            ImagesPath = file;
        }

        #region 属性

        /// <summary>
        /// 是否使用缩微图文件夹
        /// </summary>
        public bool IsthumbnailImage { get; set; } = true;


        /// <summary>
        /// 图片宽
        /// </summary>
        public int Width { get; set; } = 100;

        /// <summary>
        /// 图片高
        /// </summary>
        public int Height { get; set; } = 100;




        /// <summary>
        /// 图片文件地址
        /// </summary>
        public string FilePath { get; set; } = string.Empty;

        /// <summary>
        /// 缩微图文件夹
        /// </summary>
        public string ThumbnailImage { get; set; } = "Thumbnail";

        public Thumbnail()
        {

        }

        #endregion

        #region 共有方法

        /// <summary>
        /// 生成缩略图 提供文件
        /// </summary>
        /// <param name="newfile">文件</param>
        public void ThumbnailFile(string newfile)
        {
            ThumbnailFileThread(newfile, Width, Height);
        }

        /// <summary>
        /// 生成缩略图 提供文件
        /// </summary>
        /// <param name="newfile"></param>
        /// <param name="width">生成的缩微图的宽</param>
        /// <param name="height">生成的缩微图的高</param>
        public void ThumbnailFile(string newfile, int width, int height)
        {
            ThumbnailFileThread(newfile, width, height);
        }

        #endregion

        #region 私有方法

        private void ThumbnailFileThread(string newfile, int width, int height)
        {
            Width = width;
            Height = height;

            if (IsthumbnailImage)
            {

                ThumbnailPath = Path.Combine(FilePath, ThumbnailImage)+ newfile;
            }
            else
            {
                ThumbnailPath = FilePath+newfile;
            }
            if (string.IsNullOrEmpty(ThumbnailPath)) return;

            var ss=Path.GetDirectoryName(ThumbnailPath);

            FileHelper.CreatedFolder(ss);

          

           // var trd = new Thread(new ThreadStart(TimedProgress));
            //trd.Start();

            TimedProgress();
        }

        //根据原图片,缩微图大小等比例缩放 文件
        private void TimedProgress()
        {
            var icf = ImageHelper.GetImageCodecInfo(ImageFormat.Jpeg);

            //if (!SafeCheckHelper.IsJpgStr(FilePath)) return;
            //var extension = IoFiles.GetExtension(File);
            try
            {
                Image image;
                using (image = Image.FromFile(ImagesPath))
                {
                    var imageSize = GetImageSize(image);

                    Image thumbnailImage;
                    using (thumbnailImage = ImageHelper.GetThumbnailImage(image, imageSize.Width, imageSize.Height))
                    {
                        //var thumbnailImageFilename = IsthumbnailImage ? FilePath : Newfile;

                        ImageHelper.SaveImage(thumbnailImage, ThumbnailPath, icf);
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Log(ex);
            }
        }

        /// <summary>
        /// 等比例求出缩微图大小
        /// </summary>
        /// <param name="picture"></param>
        /// <returns></returns>
        private Size GetImageSize(Image picture)
        {
            Size imageSize;

            imageSize = new Size(Width, Height);

            if ((picture.Height > imageSize.Height) || (picture.Width > imageSize.Width))
            {

                double heightRatio = (double)picture.Height / picture.Width;
                double widthRatio = (double)picture.Width / picture.Height;

                int desiredHeight = imageSize.Height;
                int desiredWidth = imageSize.Width;


                imageSize.Height = desiredHeight;
                if (widthRatio > 0)
                    imageSize.Width = Convert.ToInt32(imageSize.Height * widthRatio);

                if (imageSize.Width > desiredWidth)
                {
                    imageSize.Width = desiredWidth;
                    imageSize.Height = Convert.ToInt32(imageSize.Width * heightRatio);
                }
            }
            else
            {
                imageSize.Width = picture.Width;
                imageSize.Height = picture.Height;
            }

            return imageSize;
        }

        #endregion

    }
}
