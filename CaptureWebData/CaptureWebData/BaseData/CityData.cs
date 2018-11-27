using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Domain.CommonData;
using DataHelp;
//using ApplicationService.IPDataService;
namespace CaptureWebData
{
    public class CityData:ItemData
    {
        public int Id { get; set; }
        public int? ParentId { get; set; }
        public string ParentCode { get; set; }
        public int NodeLevel { get;set;}
    }
    public class CityDataManage 
    {
        int GetDbInt(int hashcode) 
        {
            if (hashcode <= 0) 
            {
                hashcode=0 - hashcode;
            }
            return hashcode;
        }
        public void ImportDB(string path)
        {
            string text = FileHelper.ReadFile(path);
            List<CityData> cts = new List<CityData>();
            string[] country = text.Split(new string[] { ";;;" }, StringSplitOptions.None);//国家
            foreach (var item in country)
            {//准备提取国家数据=国家&&省
                Guid gid = Guid.NewGuid();
                string[] gjs = item.Split(new string[] { "&&&" }, StringSplitOptions.None);//省
                CityData gj = PickUpNode(gjs[0]);
                gj.Id = GetDbInt(gid.GetHashCode());
                gj.NodeLevel = 1;
                string provice = gjs[1];
                cts.Add(gj);
                string[] provices = provice.Split(new string[] { ";;" }, StringSplitOptions.None);//市
                foreach (var cityList in provices)
                {//北京市
                    string[] citys = cityList.Split(new string[] { "&&" }, StringSplitOptions.None);
                    CityData pro = PickUpNode(citys[0]);
                    pro.ParentId = gj.Id;
                    pro.Id = GetDbInt(Guid.NewGuid().GetHashCode());
                    pro.NodeLevel = 2;
                    pro.ParentCode = gj.Code;
                    cts.Add(pro);
                    string[] areas = citys[1].Split(new string[] { "||" }, StringSplitOptions.None);//区县
                    foreach (var node in areas)
                    {
                        if (string.IsNullOrEmpty(node))
                        {
                            continue;
                        }
                        string[] distincts = node.Split('&');//区县集合列表
                        CityData city = PickUpNode(distincts[0]);//海淀区
                        city.Id = GetDbInt(Guid.NewGuid().GetHashCode());
                        city.ParentId = pro.Id;
                        city.NodeLevel = 3;
                        city.ParentCode = pro.Code;
                        cts.Add(city);
                        if (distincts.Length == 1)
                        {
                            continue;
                        }
                        foreach (var dist in distincts[1].Split('|'))
                        {
                            CityData d = PickUpNode(dist);
                            d.Id = GetDbInt(Guid.NewGuid().GetHashCode());
                            d.ParentId = city.Id;
                            d.NodeLevel = 4;
                            d.ParentCode = city.Code;
                            cts.Add(d);
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
            //CategoryDataService cds = new CategoryDataService(new ConfigurationItems().TecentDA);
            //cds.SaveCategoryNode(nodes);
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
