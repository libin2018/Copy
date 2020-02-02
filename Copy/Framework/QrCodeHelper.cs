using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Web;
using ThoughtWorks.QRCode.Codec;

namespace Copy
{
    /// <summary>
    /// 二维码助手
    /// </summary>
    public class QrCodeHelper
    {
        /// <summary>
        /// 创建二维码
        /// </summary>
        /// <param name="strData"></param>
        /// <param name="qrEncoding">ALPHA_NUMERIC = 0,NUMERIC = 1,BYTE = 2</param>
        /// <param name="level"></param>
        /// <param name="version"></param>
        /// <param name="scale"></param>
        /// <param name="fileName"></param>
        /// <param name="iconPath"></param>
        /// <returns></returns>
        public static QrCodeResult BuildQrCode(string strData, int qrEncoding, string level, int version, int scale, string fileName, string iconPath = "")
        {
            var image = CreateCode(strData, qrEncoding, level, version, scale, iconPath);
            fileName = fileName + ".jpg";
            string filepath = GetVirPath(@"~/QrCode");
            if (!Directory.Exists(filepath))
            {
                Directory.CreateDirectory(filepath);
            }
            filepath = filepath + "\\" + fileName;
            System.IO.FileStream fs = new System.IO.FileStream(filepath, System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.Write);
            image.Save(fs, System.Drawing.Imaging.ImageFormat.Jpeg);
            fs.Close();
            image.Dispose();
            string virPath = string.Format("~/PayQrCode/{0}", fileName);
            string absPath = GetAbsoluteUrl(virPath);
            var res = new QrCodeResult { VirPath = virPath, AbsPath = absPath };
            return res;
        }

        /// <summary>
        /// 创建二维码
        /// </summary>
        /// <param name="strData"></param>
        /// <param name="qrEncoding"></param>
        /// <param name="level"></param>
        /// <param name="version"></param>
        /// <param name="scale"></param>
        /// <param name="outStream"></param>
        /// <param name="iconPath"></param>
        public static void BuildQrCode(string strData, Stream outStream, string iconPath = "", int qrEncoding = 2, string level = "H", int version = 8, int scale = 4)
        {
            var image = CreateCode(strData, qrEncoding, level, version, scale, iconPath);
            image.Save(outStream, System.Drawing.Imaging.ImageFormat.Jpeg);
        }

        /// <summary>
        /// 创建二维码
        /// </summary>
        /// <param name="strData"></param>
        /// <param name="qrEncoding"></param>
        /// <param name="level"></param>
        /// <param name="version"></param>
        /// <param name="scale"></param>
        /// <param name="iconPath"></param>
        /// <returns></returns>
        public static System.Drawing.Image CreateCode(string strData, int qrEncoding, string level, int version, int scale, string iconPath)
        {
            QRCodeEncoder qrCodeEncoder = new QRCodeEncoder();
            switch (qrEncoding)
            {
                case 2:
                    qrCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;
                    break;
                case 0:
                    qrCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.ALPHA_NUMERIC;
                    break;
                case 1:
                    qrCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.NUMERIC;
                    break;
                default:
                    qrCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;
                    break;
            }

            qrCodeEncoder.QRCodeScale = scale;
            qrCodeEncoder.QRCodeVersion = version;
            switch (level)
            {
                case "L":
                    qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.L;
                    break;
                case "M":
                    qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.M;
                    break;
                case "Q":
                    qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.Q;
                    break;
                default:
                    qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.H;
                    break;
            }
            var image = qrCodeEncoder.Encode(strData);
            if (!string.IsNullOrEmpty(iconPath))
            {
                try
                {
                    var icon = GetVirPath(iconPath);
                    image = CombinImage(image, icon);
                }
                catch (Exception ex)
                {
                    //LogHelper.Log.Error("合并二维码出错");
                }
            }
            return image;
        }

        private static System.Drawing.Bitmap CombinImage(System.Drawing.Bitmap imgBack, string destImg)
        {
            var img = System.Drawing.Image.FromFile(destImg);        //照片图片    
            if (img.Height != 40 || img.Width != 40)
            {
                img = KiResizeImage(img, 40, 40, 0);
            }
            Graphics g = Graphics.FromImage(imgBack);

            g.DrawImage(imgBack, 0, 0, imgBack.Width, imgBack.Height);      //g.DrawImage(imgBack, 0, 0, 相框宽, 相框高);   

            g.DrawImage(img, imgBack.Width / 2 - img.Width / 2, imgBack.Width / 2 - img.Width / 2, img.Width, img.Height);
            GC.Collect();
            return imgBack;
        }

        private static System.Drawing.Image KiResizeImage(System.Drawing.Image bmp, int newW, int newH, int Mode)
        {
            try
            {
                System.Drawing.Image b = new Bitmap(newW, newH);
                Graphics g = Graphics.FromImage(b);
                // 插值算法的质量  
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.DrawImage(bmp, new System.Drawing.Rectangle(0, 0, newW, newH), new System.Drawing.Rectangle(0, 0, bmp.Width, bmp.Height), GraphicsUnit.Pixel);
                g.Dispose();
                return b;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 获取虚拟路径
        /// </summary>
        /// <param name="strUrl"></param>
        /// <returns></returns>
        private static string GetVirPath(string strUrl)
        {
            if (!string.IsNullOrEmpty(strUrl))
            {
                var urll = HttpContext.Current.Request.MapPath(strUrl);
                return urll;
            }
            return string.Empty;
        }

        /// <summary>
        /// 生成完整地址
        /// </summary>
        /// <param name="strUrl">strUrl</param>
        /// <returns>string</returns>
        private static string GetAbsoluteUrl(string strUrl)
        {
            if (!string.IsNullOrEmpty(strUrl))
            {
                var urll = new Uri(HttpContext.Current.Request.Url, VirtualPathUtility.ToAbsolute(strUrl)).AbsoluteUri;
                return urll;
            }
            return string.Empty;
        }
    }

    /// <summary>
    /// 二维码地址
    /// </summary>
    public class QrCodeResult
    {
        /// <summary>
        /// 相对地址
        /// </summary>
        public string VirPath { get; set; }

        /// <summary>
        /// 绝对地址
        /// </summary>
        public string AbsPath { get; set; }
    }
}