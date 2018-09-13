using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CefSharpWin
{
    /// <summary>
    /// 表格列属性
    /// </summary>
    public class GridColumn
    {
        public string Name { get; set; }
        public string Text { get; set; }
        public int Width { get; set; }
    }

    public class Station
    {
        public string Code { get; set; }
        public string Name { get; set; }
        [System.ComponentModel.Description("12306 内部的编码，不知道有啥用")]
        public string Code12306 { get; set; }
        [System.ComponentModel.Description("简写")]
        public string ShorthandCode { get; set; }
        [System.ComponentModel.Description("拼音")]
        public string SpellName { get; set; }
        public int Sort { get; set; }

        public void SetEntity(string[] arr)
        {
            Code = arr[0];
            Name = arr[1];
            Code12306 = arr[2];
            SpellName = arr[3];
            ShorthandCode = arr[4];
            Sort = int.Parse(arr[5]);
        }
    }
}
