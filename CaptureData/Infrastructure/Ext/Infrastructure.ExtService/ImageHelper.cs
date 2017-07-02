using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;
namespace Infrastructure.ExtService
{
    public class ImageHelper
    {
        /// <summary>
        /// 根据原图片的文件获取压缩后的图片文件流
        /// </summary>
        /// <param name="originImg">原图片文件流</param>
        /// <param name="zipQuality">压缩质量</param>
        /// <returns></returns>
        public MemoryStream ImageZip(Image originImg, long zipQuality) 
        {
            Bitmap source = new Bitmap(originImg);
            ImageCodecInfo code = GetImageCode("image/jpeg");//获取图片编码格式 
            System.Drawing.Imaging.Encoder my = System.Drawing.Imaging.Encoder.Quality;
            EncoderParameters ps = new EncoderParameters(1);
            EncoderParameter myparam = new EncoderParameter(my, zipQuality);
            ps.Param[0] = myparam;
            DateTime now = DateTime.Now;
            int filename = now.Millisecond;
            MemoryStream img = new MemoryStream();//获取处理后的图像文件流
            source.Save(img, ImageFormat.Jpeg);
            source.Dispose();
            return img;
        }
        /// <summary>
        /// 根据文件留获取图片
        /// </summary>
        /// <param name="originStram"></param>
        /// <param name="zipQuality"></param>
        /// <returns></returns>
        public MemoryStream ImageZip(Stream originStram, long zipQuality) 
        {
            System.Drawing.Image originImg = System.Drawing.Image.FromStream(originStram);
            return ImageZip(originImg, zipQuality);
        }
        public MemoryStream OriginImage(Stream imgStream)
        {
            System.Drawing.Image originImg = System.Drawing.Image.FromStream(imgStream);
            Bitmap source = new Bitmap(originImg);
            MemoryStream img = new MemoryStream();//获取处理后的图像文件流
            source.Save(img, ImageFormat.Jpeg);
            return img;
        }
        /// <summary>
        /// 生成缩略图
        /// </summary>
        public MemoryStream GenerateThumStream(Image originImage, long imgQuarity)
        {
            //首先获取原图片的文件流
            byte[] stream = ImageConvertByte(originImage);
            if (stream == null)
            {//读取文件流失败
                return null;
            }
            MemoryStream ms = new MemoryStream(stream);
            System.Drawing.Image originImg = System.Drawing.Image.FromStream(ms);
            int x, y, w, h;
            int imgw = originImg.Width, imgh = originImg.Height;
            //缩略图规格  <160*120
            int nw = 160, nh = 120;
            if (imgw > 160 && imgh > 120)
            {//整体像素大于
                w = nw;
                h = nw * imgh / imgw;
                if (h > nh)
                {
                    h = nh;
                    w = nh * imgw / imgh;
                    x = (nw - w) / 2;
                    y = 0;
                }
                else
                {
                    x = 0;
                    y = (nh - h) / 2;
                }
            }
            else if (imgw > nw)
            {//像素宽度大于
                w = nw;
                h = nw * imgh / imgw;
                x = 0;
                y = (nh - h) / 2;
            }
            else if (imgh > nh)
            {//像素高度大于
                h = nh;
                w = nh * (imgw / imgh);
                x = (160 - w) / 2;
                y = 0;
            }
            else 
            {
                w = imgw;
                h = imgh;
                x = (nw - w) / 2;
                y = (120 - h) / 2;
            }
            Bitmap bm = new Bitmap(nw, nh);
            Graphics g = Graphics.FromImage(bm);
            // 指定高质量、低速度呈现。
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            // 指定高质量的双三次插值法。执行预筛选以确保高质量的收缩。此模式可产生质量最高的转换图像。           
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            g.Clear(Color.White);
            g.DrawImage(originImg,
                new Rectangle(x, y, w, h),
                0, 0, imgw, imgh, GraphicsUnit.Pixel);
            long[] quality = new long[1];
            quality[0] = imgQuarity;
            System.Drawing.Imaging.EncoderParameters encodes = new EncoderParameters();
            System.Drawing.Imaging.EncoderParameter encodep = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);
            encodes.Param[0] = encodep;
            ImageCodecInfo arrenc = GetImageCode("image/jpeg");
            MemoryStream thumStream = new MemoryStream();
            bm.Save(thumStream, ImageFormat.Jpeg);
            bm.Dispose();
            originImg.Dispose();
            g.Dispose();
            return thumStream;
        }
        /// <summary>
        /// 获取图片文件流
        /// </summary>
        /// <param name="originImg"></param>
        /// <returns></returns>
        public byte[] ImageConvertByte(Image originImg) 
        {
            try
            {
                MemoryStream stream = new MemoryStream();
                originImg.Save(stream, ImageFormat.Bmp);
                BinaryReader br = new BinaryReader(stream);
                byte[] origin = stream.ToArray();
                stream.Close();
                originImg.Dispose();
                br.Close();
                return origin;
            }
            catch (Exception ex) 
            {
                return null;
            }
        }
        public Dictionary<string, string> GetImageCode() 
        {
            ImageCodecInfo[] encodes=ImageCodecInfo.GetImageEncoders();
            Dictionary<string,string> imgEncode=new Dictionary<string,string>();
            foreach (ImageCodecInfo item in encodes)
            {
                imgEncode.Add(item.CodecName, item.MimeType);
            }
            return imgEncode;
        }
        public ImageCodecInfo GetImageCode(string imageMimeType) 
        {
            //image/jpeg
            ImageCodecInfo[] encodes = ImageCodecInfo.GetImageEncoders();
            foreach (ImageCodecInfo item in encodes)
            {
                if (item.MimeType == imageMimeType) 
                {
                    return item;
                }
            }
            return null;
        }
        public string SaveImage(Stream stream, string dir,string fileName)
        {
            try
            {
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }
                string file = dir + "\\" + fileName;
                FileStream fs = new FileStream(file, FileMode.Create, FileAccess.Write);
                MemoryStream ms = (MemoryStream)stream;
                byte[] bs = ms.ToArray();
                fs.Write(bs, 0, bs.Length);
                fs.Close();
                ms.Close();
                return dir;
            }
            catch (Exception ex) 
            {
                return string.Empty;
            }
        }
        public string SaveImage(Image img,string dir)
        {
            try
            {
                FileStream fs = new FileStream(dir, FileMode.Create, FileAccess.Write);
                img.Save(fs, ImageFormat.Jpeg);
                return dir;
            }
            catch (Exception ex) 
            {
                return string.Empty;
            }
        }
    }
}
