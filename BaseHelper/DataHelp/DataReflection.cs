using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Data;
using System.ComponentModel;
namespace DataHelp
{
    public static class DataReflection
    {
        /// <summary>
        /// 提供对象数据转换为目标对象
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <typeparam name="R">目标对象类型</typeparam>
        /// <param name="soure">原对象数据</param>
        /// <returns></returns>
        public static R ConvertMapModel<T, R>(this T soure)
            where T : class
            where R : class
        {
            R result = (R)System.Activator.CreateInstance(typeof(R));
            Type s = soure.GetType();
            Type r = result.GetType();
            PropertyInfo[] spi =s.GetProperties();
            PropertyInfo[] rps =r.GetProperties();
            List<string> common = new List<string>();
            foreach (PropertyInfo item in spi)
            {
                if (rps.Any(p => p.Name == item.Name))
                {//查找公共属性
                    common.Add(item.Name);
                }
            }
            foreach (string item in common)
            {
                PropertyInfo p = s.GetProperty(item);
                object value = p.GetValue(soure,null);
                if (value == null)
                {
                    continue;
                }
                PropertyInfo tp = r.GetProperty(item);
                MemberInfo mi= tp.GetSetMethod();
                if (mi == null) { continue; }
                r.GetProperty(item).SetValue(result, value, null);//未找到属性设置方法。【需要过滤只读属性】
            }
            return result;
        }
        /// <summary>
        ///转换为目标对象时不考虑大小写
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="R"></typeparam>
        /// <param name="soure"></param>
        /// <param name="ingnoreLowser">是否忽略字段大小写</param>
        /// <returns></returns>
        public static R ConvertMapModel<T, R>(this T soure,bool ingnoreLowser)
            where T : class
            where R : class
        {
            if (!ingnoreLowser) 
            {
                return soure.ConvertMapModel<T, R>();
            }
            R result = (R)System.Activator.CreateInstance(typeof(R));
            Type s = soure.GetType();
            Type r = result.GetType();
            PropertyInfo[] spi = s.GetProperties();
            PropertyInfo[] rps = r.GetProperties();
            List<string> common = new List<string>();
            List<string> sourceField = new List<string>();
            List<string> resultFiled = new List<string>();
            foreach (PropertyInfo item in spi)
            {
                if (rps.Any(p => p.Name.ToLower() == item.Name.ToLower()))
                {//查找公共属性
                    sourceField.Add(item.Name);
                    resultFiled.Add(rps.Where(sp => sp.Name.ToLower() == item.Name.ToLower()).FirstOrDefault().Name);
                    
                }
            }
            for (int i = 0; i < sourceField.Count; i++)
            {
                PropertyInfo p = s.GetProperty(sourceField[i]);
                object value = p.GetValue(soure, null);
                if (value == null)
                {
                    continue;
                }
                PropertyInfo tp = r.GetProperty(resultFiled[i]);
                MemberInfo mi = tp.GetSetMethod();
                if (mi == null) { continue; }
                r.GetProperty(resultFiled[i]).SetValue(result, value, null);//未找到属性设置方法。【需要过滤只读属性】
            }
            return result;
        }
        public static List<T> ConvertModels<T>(DataSet ds) where T:class
        {
            DataTable table = ds.Tables[0];
            DataColumnCollection columns = table.Columns;
            Dictionary<string, string> map = new Dictionary<string, string>();
            PropertyInfo[] pis = typeof(T).GetProperties();
            string[] pns=pis.Select(s=>s.Name.ToLower()).ToArray();
            List<T> list = new List<T>();
            //查找出公告的列
            foreach (DataColumn item in columns)
            {
                if (pns.Contains(item.ColumnName.ToLower()))
                {
                    map.Add(pis.Where(p => p.Name.ToLower() == item.ColumnName.ToLower()).FirstOrDefault().Name, item.ColumnName);
                }
                else
                { //移除非查询列
                    table.Columns.Remove(item);
                }
            }
            foreach (DataRow item in table.Rows)
            {
                T d = System.Activator.CreateInstance<T>();
                //根据匹配的列进行选择设置
                FillRowIntoEntity(d, item, map);
                list.Add(d);
            }
            return list;
        }

        public static List<T> DataSetConvert<T>(this DataSet ds) where T : class
        {/* 特性名称定义转换*/

            List<T> items = new List<T>();
            if (ds == null || ds.Tables.Count == 0)
            {//空数据
                return items;
            }
            Type t = typeof(T);
            List<PropertyInfo> pis = t.GetProperties().ToList();
            if (pis == null || pis.Count == 0)
            {//数据异常
                return null;
            }
            #region 筛选出查询集合映射属性
            List<string> columns = new List<string>();
            // start 判断属性是否存储
            DataTable dt = ds.Tables[0];
            foreach (DataColumn item in dt.Columns)
            {
                string column = item.ColumnName.ToLower();
                //是否对于不存在的列进行数据移除
                columns.Add(column);
            }
            List<PropertyInfo> targetProperty = new List<PropertyInfo>();
            foreach (PropertyInfo p in pis)
            {
                if (columns.Contains(p.Name.ToLower()) && p.GetSetMethod()!= null)
                {//移除定义的只读属性 //不能在进行遍历是移除属性 集合已修改；可能无法执行枚举操作。
                    targetProperty.Add(p);
                }
            }//end 过滤之后字段全部存在于集合中
            pis.Clear();
            pis = targetProperty;
            if (pis == null || pis.Count == 0)
            {//数据异常:过滤之后没有映射的字段
                return null;
            }
            #region 移除不需要匹配的列
            foreach (string item in columns)
            {
                if (!pis.Where(p => p.Name.ToLower() == item).Any())
                {
                    dt.Columns.Remove(item);
                }
            }
            #endregion 移除不需要匹配的列
            #endregion 筛选出查询集合映射属性
            string[] fields = pis.Select(p => p.Name).ToArray();

            foreach (DataRow item in dt.Rows)
            {
                T entity = System.Activator.CreateInstance<T>();
                foreach (string f in fields)
                {
                    PropertyInfo pi = pis.Where(p => p.Name == f).First();
                    object value = item[f];
                    if (value == null)
                    {
                        continue;
                    }
                    //如果属性定义的数据类型为可空情形，需要特殊处理
                    if (pi.PropertyType.Name == typeof(Nullable<>).Name) 
                    {
                        NullableConverter nullableConverter = new NullableConverter(pi.PropertyType);//如何获取可空类型属性非空时的数据类型
                        Type nt = nullableConverter.UnderlyingType;
                        pi.SetValue(entity, Convert.ChangeType(value, nt), null);
                    }
                    else if (value.GetType().Name != typeof(DBNull).Name&&value.GetType().Name != pi.PropertyType.Name)
                    {
                       object  pv= Convert.ChangeType(value, pi.PropertyType);
                       pi.SetValue(entity, pv, null);
                    }
                    else if (value.GetType().Name != typeof(DBNull).Name) //如果此字段数据为空是的处理
                    {
                        pi.SetValue(entity, value, null);
                    }
                }
                items.Add(entity);
            }
            return items;
        }
        public static List<T> DataSetConvert<T>(this DataTable table, Dictionary<string, string> columnMapProperty) where T : class
        {
            List<T> data = new List<T>();
            DataColumnCollection columns=table.Columns;
            string[] convert = columnMapProperty.Keys.Select(s=>s.ToLower()).ToArray();
            string[] key = columnMapProperty.Keys.ToArray();
            Dictionary<string, string> propertyMapColumn = new Dictionary<string, string>();//属性匹配列
            foreach (DataColumn item in columns)
            {//移除无关数据
                if (!convert.Contains(item.ColumnName.ToLower()))
                {//转换为小写匹配
                    table.Columns.Remove(item);
                    continue;
                }
                //收集匹配关系
                int index = Array.IndexOf(convert, item.ColumnName.ToLower());
                propertyMapColumn.Add( columnMapProperty[key[index]],item.ColumnName);
            }
            foreach (DataRow item in table.Rows)
            {
                T rec = System.Activator.CreateInstance<T>();
                FillRowIntoEntity(rec, item, propertyMapColumn);
                data.Add(rec);
            }
            return data;
        }
        /// <summary>
        /// 数据填充到实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="rec"></param>
        /// <param name="row"></param>
        /// <param name="propertyMapcolumn">属性匹配列的关系字典</param>
        static void FillRowIntoEntity<T>(T rec, DataRow row, Dictionary<string, string> propertyMapcolumn)
        {
            Type targetClass=rec.GetType();
            foreach (var item in propertyMapcolumn)
            {
                object column = row[item.Value];
                if (column == null)
                {
                    continue;  
                }
                PropertyInfo pi = targetClass.GetProperty(item.Key);
                SetValueIntoProperty(pi, rec, column);
            }
        }
        /// <summary>
        /// 将数据写入到实体中
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pi"></param>
        /// <param name="entity"></param>
        /// <param name="value"></param>
        static void SetValueIntoProperty<T>(PropertyInfo pi, T entity,object value)
        {
            if (value == null)
            {
                return;
            }
            if (pi.PropertyType.Name == typeof(Nullable<>).Name)
            {
                NullableConverter nullableConverter = new NullableConverter(pi.PropertyType);//如何获取可空类型属性非空时的数据类型
                Type nt = nullableConverter.UnderlyingType;
                pi.SetValue(entity, Convert.ChangeType(value, nt), null);
            }
            else if (value.GetType().Name != typeof(DBNull).Name && value.GetType().Name != pi.PropertyType.Name)
            {
                object pv = Convert.ChangeType(value, pi.PropertyType);
                pi.SetValue(entity, pv, null);
            }
            else if (value.GetType().Name != typeof(DBNull).Name) //如果此字段数据为空是的处理
            {
                pi.SetValue(entity, value, null);
            }
        }
    }
}
