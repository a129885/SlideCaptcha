using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SlideCaptcha.Tools
{
    public class ImageTool
    {
        public string ImageToBase64(string fileFullName)
        {
            try
            {
                Bitmap bmp = new Bitmap(fileFullName);
                MemoryStream ms = new MemoryStream();
                var suffix = fileFullName.Substring(fileFullName.LastIndexOf('.') + 1,
                    fileFullName.Length - fileFullName.LastIndexOf('.') - 1).ToLower();
                var suffixName = suffix == "png"
                    ? ImageFormat.Png
                    : suffix == "jpg" || suffix == "jpeg"
                        ? ImageFormat.Jpeg
                        : suffix == "bmp"
                            ? ImageFormat.Bmp
                            : suffix == "gif"
                                ? ImageFormat.Gif
                                : ImageFormat.Jpeg;

                bmp.Save(ms, suffixName);
                byte[] arr = new byte[ms.Length]; ms.Position = 0;
                ms.Read(arr, 0, (int)ms.Length); ms.Close();
                return Convert.ToBase64String(arr);
            }
            catch (Exception ex)
            {
                return null;
            }

        }



        public ImageAndXY GetNewValidateImageBase(string fileFullName = "C:\\Test\\Test.png")
        {
            var res = new ImageAndXY();
            Bitmap b = new Bitmap(fileFullName);
            int targetWidth = 350;
            int targetHeight = 213;

            var snipSize = 50;

            //图像缩小或者拉伸
            //计算宽度的缩放比例
            var nPercentW = b.Size.Width / targetWidth;
            //计算高度的缩放比例
            var nPercentH = b.Size.Height / targetHeight;
            decimal nPercent = 0;
            if (nPercentH < nPercentW)
                nPercent = nPercentH;
            else
                nPercent = nPercentW;
            //期望的宽度
            int destWidth = (int)(targetWidth);
            //int destWidth = (int)(targetWidth * nPercent);
            //期望的高度
            int destHeight = (int)(targetHeight);
            //int destHeight = (int)(targetHeight * nPercent);

            Bitmap bmp = new Bitmap(destWidth, destHeight);
            Graphics g = Graphics.FromImage((System.Drawing.Image)bmp);
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            //绘制图像
            g.DrawImage(b, 0, 0, destWidth, destHeight);
            g.Dispose();

            res.X1 = new Random().Next(50, bmp.Size.Width - snipSize);
            res.Y1 = new Random().Next(50, bmp.Size.Height - snipSize);
            res.X2 = res.X1 + snipSize;
            res.Y2 = res.Y1 + snipSize;

            //定义截取矩形
            System.Drawing.Rectangle cropArea = new System.Drawing.Rectangle(res.X1, res.Y1, snipSize, snipSize);
            Bitmap bmpCrop = bmp.Clone(cropArea, bmp.PixelFormat);
            //透明度处理
            for (int w = res.X1; w < res.X2; w++)
            {
                for (int h = res.Y1; h < res.Y2; h++)
                {
                    Color c = bmp.GetPixel(w, h);
                    Color newC;
                    if (!c.Equals(Color.FromArgb(0, 0, 0, 0)))
                    {
                        newC = Color.FromArgb(128, c);
                    }
                    else
                    {
                        newC = c;
                    }
                    bmp.SetPixel(w, h, newC);
                }
            }

            var suffix = fileFullName.Substring(fileFullName.LastIndexOf('.') + 1,
                fileFullName.Length - fileFullName.LastIndexOf('.') - 1).ToLower();
            var suffixName = suffix == "png"
                ? ImageFormat.Png
                : suffix == "jpg" || suffix == "jpeg"
                    ? ImageFormat.Jpeg
                    : suffix == "bmp"
                        ? ImageFormat.Bmp
                        : suffix == "gif"
                            ? ImageFormat.Gif
                            : ImageFormat.Jpeg;
            MemoryStream ms = new MemoryStream();
            bmp.Save(ms, suffixName);
            byte[] arr = new byte[ms.Length]; ms.Position = 0;
            ms.Read(arr, 0, (int)ms.Length); ms.Close();
            res.OriginalImageBase64 = Convert.ToBase64String(arr);

            MemoryStream ms2 = new MemoryStream();
            bmpCrop.Save(ms2, suffixName);
            arr = new byte[ms2.Length]; ms2.Position = 0;
            ms2.Read(arr, 0, (int)ms2.Length); ms2.Close();
            res.SnipImageBase64 = Convert.ToBase64String(arr);

            return res;
        }

        public class ImageAndXY
        {
            public string OriginalImageBase64 { get; set; }
            public string SnipImageBase64 { get; set; }

            public int X1 { get; set; }
            public int X2 { get; set; }
            public int Y1 { get; set; }
            public int Y2 { get; set; }

        }
    }
}
