using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;

namespace MyChy.Frame.Core.Common.Spread
{
    internal static class ImageHelper
    {
        public static Image GetThumbnailImage(Image image, int width, int height)
        {
            if (image == null || width < 1 || height < 1)
                return null;

            // 新建一个bmp图片
            //
            Image bitmap = new Bitmap(width, height);

            // 新建一个画板
            //
            using (Graphics g = Graphics.FromImage(bitmap))
            {

                // 设置高质量插值法
                //
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

                // 设置高质量,低速度呈现平滑程度
                //
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

                // 高质量、低速度复合
                //
                g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;

                // 清空画布并以透明背景色填充
                //
                g.Clear(Color.Transparent);

                // 在指定位置并且按指定大小绘制原图片的指定部分
                //
                g.DrawImage(image, new Rectangle(0, 0, width, height),
                    new Rectangle(0, 0, image.Width, image.Height),
                    GraphicsUnit.Pixel);

                return bitmap;
            }

        }

        /// <summary>
        /// 获取图像编码解码器的所有相关信息
        /// </summary>
        /// <param name="mimeType">包含编码解码器的多用途网际邮件扩充协议 (MIME) 类型的字符串</param>
        /// <returns>返回图像编码解码器的所有相关信息</returns>
        public static ImageCodecInfo GetCodecInfo(string mimeType)
        {
            var codecInfo = ImageCodecInfo.GetImageEncoders();
            return codecInfo.FirstOrDefault(ici => ici.MimeType == mimeType);
        }

        public static ImageCodecInfo GetImageCodecInfo(ImageFormat format)
        {
            var encoders = ImageCodecInfo.GetImageEncoders();

            return encoders.FirstOrDefault(icf => icf.FormatID == format.Guid);
        }

        /// <summary>
        /// 高质量保存图片
        /// </summary>
        /// <param name="image"></param>
        /// <param name="savePath"></param>
        /// <param name="ici"></param>
        public static void SaveImage(Image image, string savePath, ImageCodecInfo ici)
        {
            // 设置 原图片 对象的 EncoderParameters 对象
            //
            var parms = new EncoderParameters(1);
            var parm = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, ((long)95));
            parms.Param[0] = parm;

            image.Save(savePath, ici, parms);
            parms.Dispose();
        }
    }
}
