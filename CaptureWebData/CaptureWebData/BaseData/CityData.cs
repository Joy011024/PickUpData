using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Domain.CommonData;
using DataHelp;
using ApplicationService.IPDataService;
namespace CaptureWebData
{
    public class CityData:ItemData
    {
        public string ParentCode { get; set; }
        public int NodeLevel { get;set;}
    }
    public class CityDataManage 
    {
        public void ImportDB(string path)
        {
            string text = FileHelper.ReadFile(path);
            List<CityData> cts = new List<CityData>();
            string[] country = text.Split(new string[] { ";;" }, StringSplitOptions.None);//国家
            foreach (var item in country)
            {//准备提取国家数据=国家&&省
                string[] gjs = item.Split(new string[] { "&&" }, StringSplitOptions.None);//省
                CityData gj = PickUpNode(gjs[0]);
                gj.NodeLevel=1;
                string provice = gjs[1];
                cts.Add(gj);
                string[] provices = provice.Split(new string[] { ";" }, StringSplitOptions.None);//市
                foreach (var city in provices)
                {//北京市
                    string[] citys = city.Split('&');
                    CityData pro = PickUpNode(citys[0]);
                    pro.NodeLevel=2;
                    pro.ParentCode = gj.Code;
                    cts.Add(pro);
                    string[] areas = citys[1].Split(new string[] { "&" }, StringSplitOptions.None);//区县
                    foreach (var node in areas)
                    {
                        string[] distincts = node.Split(',');
                        foreach (var dis in distincts)
                        {
                            CityData area = PickUpNode(dis);//海淀区
                            area.NodeLevel=3;
                            area.ParentCode = pro.Code;
                            cts.Add(area);
                        }
                    }
                }
            }
            List<CategoryData> nodes = cts.Select(s => s.ConvertMapModel<CityData, CategoryData>(true)).
                Where(n =>
                {
                    n.ItemType = "City";
                    return true;
                }).ToList();
            CategoryDataService cds = new CategoryDataService(new ConfigurationItems().TecentDA);
            cds.SaveCategoryNode(nodes);
        }
        private CityData PickUpNode(string item) 
        {
            item = item.Replace("[", "").Replace("{", "").Replace("}", "").Replace("]", "");
            string[] data = item.Split('=');
            CityData cd = new CityData();
            cd.Name = data[0];
            cd.Code = data[1];
            return cd;
        }
    }
}
