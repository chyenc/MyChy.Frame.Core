//using QRCoder;
//using System.Drawing;
//using System.Drawing.Imaging;
//using System.IO;

//namespace MyChy.Frame.Core.Common.Helper
//{
//    public class QRCodeHelper
//    {
//        /// <summary>
//        /// Bitmap 图片格式
//        /// </summary>
//        /// <param name="url">存储内容</param>
//        /// <param name="pixel">像素大小</param>
//        /// <param name="png">中间图片-服务器绝对地址</param>
//        /// <returns></returns>
//        public static Bitmap GetQRCode(string url, int pixel, string icon = "")
//        {

//            QRCodeGenerator generator = new QRCodeGenerator();
//            QRCodeData codeData = generator.CreateQrCode(url, QRCodeGenerator.ECCLevel.M, true);
//            QRCode qrcode = new QRCode(codeData);
//            Bitmap qrImage = null;
//            if (string.IsNullOrEmpty(icon))
//            {
//                qrImage = qrcode.GetGraphic(pixel);
//            }
//            else
//            {
//                qrImage = qrcode.GetGraphic(pixel, Color.Black, Color.White, (Bitmap)Bitmap.FromFile(icon));
//            }
//            return qrImage;
//        }

//        /// <summary>
//        /// 内存流
//        /// </summary>
//        /// <param name="url"></param>
//        /// <param name="pixel"></param>
//        /// <param name="icon"></param>
//        /// <returns></returns>
//        public static MemoryStream GetQRCodeMs(string url, int pixel, string icon = "")
//        {
//            MemoryStream ms = null;
//            var bitmap = GetQRCode(url,pixel,icon);
//            ms = new MemoryStream();//生成内存流对象  
//            bitmap.Save(ms, ImageFormat.Jpeg);
//            bitmap.Dispose();
//            return ms;

//        }
//    }
//}
