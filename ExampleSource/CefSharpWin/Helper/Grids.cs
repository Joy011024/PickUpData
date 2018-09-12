using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
namespace CefSharpWin
{
    [Serializable]
    [XmlRoot(ElementName = "Grids")]
    public class Grids
    {
        [XmlElement(ElementName = "Grid")]
        public List<Grid> GridList { get; set; }
        public Grid this[string gridName]
        {
            get
            {
                foreach (var item in GridList)
                {
                    if (item.Name == gridName)
                    {
                        return item;
                    }
                }
                return null;
            }
        }
    }
    [Serializable]
    [XmlRoot(ElementName = "Grid")]
    public class Grid
    {
        [System.ComponentModel.Description("表格名")]
        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }
        [XmlElement(ElementName = "Columns")]
        public Columns Columns { get; set; }
        public int HeadHeight { get; set; }
    }
    [Serializable]
    [XmlRoot(ElementName = "Columns")]
    public class Columns
    {
        [XmlElement(ElementName = "Column")]
        public List<Column> Heads { get; set; }
        public Column this[string columnName]
        {
            get
            {
                foreach (var item in Heads)
                {
                    if (item.Name == columnName)
                    {
                        return item;
                    }
                }
                return null;
            }
        }
    }
    [Serializable]
    [XmlRoot(ElementName = "Column")]
    public class Column
    {
        [XmlAttribute(AttributeName = "Name")]
        public string Name { get; set; }
        [XmlAttribute(AttributeName = "Text")]
        public string Text { get; set; }
        [XmlAttribute(AttributeName = "Width")]
        public int Width { get; set; }
        [XmlAttribute(AttributeName = "Hidden")]
        public bool Hidden { get; set; }
        [System.ComponentModel.Description("代码生成该列文本")]
        [XmlAttribute(AttributeName = "CodeGenerate")]
        public string CodeGenerate { get; set; }
    }
}
