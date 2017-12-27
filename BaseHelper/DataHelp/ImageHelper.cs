using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;
namespace DataHelp
{
    public static  class ImageHelper
    {
        public class ImageInfo
        {
            public int X;
            public int Y;
            private float _HalfX;
            public float HalfX { get { if (_HalfX == 0) { _HalfX = X / 2.0f; } return _HalfX; } }
            private float _HalfY;
            public float HalfY { get { if (_HalfY == 0) { _HalfY = Y / 2.0f; } return _HalfY; } }
            /// <summary>
            /// 水印文本
            /// </summary>
            public string Watertext { get; set; }
            /// <summary>
            /// 水印字体色
            /// </summary>
            public Color? WatertextFontColor { get; set; }
            /// <summary>
            /// 设置的水印字体大小
            /// </summary>
            public int WatertextFontSize { get; set; }
            /// <summary>
            /// 默认水印字体大小[30]
            /// </summary>
            public int DefaultWatertextFontSize = 30;
            public void SetWatertextColor(int red,int green,int blue) 
            {
                WatertextFontColor = Color.FromArgb(red, green, blue);
            }
        }
        static ImageInfo GetImageInfo(Image img) 
        {
            ImageInfo info = new ImageInfo();
            info.X = img.Width;
            info.Y = img.Height;
            return info;
        }
        public static Image ImageRomate(this Image obj, float angle)
        {
            ImageInfo info = GetImageInfo(obj);
            Bitmap romate = new Bitmap(info.X, info.Y);
            romate.SetResolution(obj.HorizontalResolution, obj.VerticalResolution);//设置旋转图片的分辨率
            Graphics g = Graphics.FromImage(romate);
            g.TranslateTransform(info.HalfX, info.HalfY);
            g.RotateTransform(angle);
            g.TranslateTransform(-info.HalfX, -info.HalfY);
            g.DrawImage(obj, new Point(0, 0));
            g.Dispose();
            return romate;
        }

        public static Image InsertWaterText(this Image obj, string text, int startX, int startY)
        {
            ImageInfo info = GetImageInfo(obj);
            Bitmap bit = new Bitmap(info.X, info.Y);
            //新建画布
            Graphics g = Graphics.FromImage(obj);
            //g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Default;
            //g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.Default;
            //g.Clear(System.Drawing.Color.White);
            //g.DrawImage(obj,
            //    new System.Drawing.Rectangle(0, 0, info.X, info.Y),
            //    new Rectangle(0, 0, info.X, info.Y),
            //    System.Drawing.GraphicsUnit.Pixel);
            Font f = new Font("宋体", 30);
            Brush b = new SolidBrush(Color.Black);
            g.DrawString(text, f, b, new Point(startX, startY));
            g.Dispose();
            return obj;
        }
        /// <summary>
        /// 批量设置水印文件
        /// </summary>
        /// <param name="obj">图片对象</param>
        /// <param name="watertexts">水印信息【水印文本，显示的水印坐标】</param>
        /// <param name="fontSize"></param>
        /// <returns></returns>
        public static Image InsertWatertexts(this Image obj, List<ImageInfo> watertexts) 
        {
            ImageInfo info = GetImageInfo(obj);
            Bitmap bit = new Bitmap(info.X, info.Y);
            //新建画布
            Graphics g = Graphics.FromImage(obj);
            foreach (ImageInfo item in watertexts)
            {
                //计算字符内容是否会超出行显示，超出则进行自动换行
                Font f = new Font("宋体", item.WatertextFontSize>0?item.WatertextFontSize:item.DefaultWatertextFontSize);
                SizeF sf= g.MeasureString(item.Watertext, f);
                Brush b = new SolidBrush(!item.WatertextFontColor.HasValue ? Color.Black : item.WatertextFontColor.Value);
                if (info.X < item.X + sf.Width)
                {//需要换行显示
                    int length = (int)((info.X - item.X) * item.Watertext.Length / sf.Width);//每行可以显示
                    int line = 1;
                    int total = item.Watertext.Length / length + (item.Watertext.Length % length > 0 ? 1 : 0);
                    while (line <=total)
                    {
                        int start = (line-1) * length;
                        int end =line*length;
                        end = end > item.Watertext.Length ? item.Watertext.Length : end;
                        g.DrawString(item.Watertext.Substring(start,end-start),f,b,new Point(item.X,item.Y+(int)(line*sf.Height)));
                        line++;
                    }
                }
                else
                {
                    
                    g.DrawString(item.Watertext, f, b, new Point(item.X, item.Y));
                }
            }
            g.Dispose();
            return obj;
        }
        public static string SaveImage(this Image obj, string fileFullName) 
        {
            if(string.IsNullOrEmpty(fileFullName))
            {
                return "File Path Is Empty";
            }
            if (File.Exists(fileFullName))
            {
                return "Same Name Of File Exists";
            }
            try
            {
                obj.Save(fileFullName);
                return string.Empty;
            }
            catch (Exception ex) 
            {
                obj.Tag = ex;
                return ex.ToString(); ;
            }
        }
       
    }
}
