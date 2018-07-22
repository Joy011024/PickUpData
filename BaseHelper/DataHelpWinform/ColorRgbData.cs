using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataHelpWinform
{
    public class ColorRgbData
    {
        /// <summary>
        /// 透明度
        /// </summary>
        public int Alpha { get; set; }
        public int Red { get; set; }
        public int Blue { get; set; }
        public int Green { get; set; }
        private System.Drawing.Color? _MapColor { get; set; }
        public System.Drawing.Color? MapColor
        {
            get
            {
                if (!_MapColor.HasValue)
                {
                    _MapColor = System.Drawing.Color.FromArgb(Alpha, Red, Green, Blue);
                }
                return _MapColor;
            }
        }
    }
}
