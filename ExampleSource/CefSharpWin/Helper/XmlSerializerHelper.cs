using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
namespace CefSharpWin
{
    /// <summary>
    /// 提供XML和二进制的序列化与反序列化实现
    /// </summary>
    public class XmlSerializerHelper
    {
        /// <summary>
        /// 反序列化指定的XML文件为指定类型对象
        /// </summary>
        /// <typeparam name="T">指定类型</typeparam>
        /// <param name="pXMLFullName">文件全名</param>
        /// <returns></returns>
        public static T LoadFromXML<T>(string pXMLFullName)
        {
            T obj = default(T);

            try
            {

                XmlSerializer ser = new XmlSerializer(typeof(T));

                using (XmlReader xr = XmlReader.Create(pXMLFullName))
                {
                    obj = (T)ser.Deserialize(xr);
                    xr.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return obj;
        }

        /// <summary>
        /// 把对象持久化到指定文件
        /// </summary>
        /// <typeparam name="T">指定类型</typeparam>
        /// <param name="pObject">指定对象</param>
        /// <param name="pXMLFullName"></param>
        public static void SaveObjToXML<T>(T pObject, string pXMLFullName)
        {
            try
            {
                XmlSerializer ser = new XmlSerializer(typeof(T));

                XmlWriterSettings xws = new XmlWriterSettings();
                xws.Encoding = Encoding.UTF8;
                xws.Indent = true;
                xws.IndentChars = "    ";
                xws.NewLineChars = Environment.NewLine;
                xws.OmitXmlDeclaration = false;

                using (XmlWriter xw = XmlWriter.Create(pXMLFullName, xws))
                {
                    XmlSerializerNamespaces xsn = new XmlSerializerNamespaces();
                    xsn.Add(String.Empty, String.Empty);

                    ser.Serialize(xw, pObject, xsn);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 把内存对象保存到指定binary文件
        /// </summary>
        /// <typeparam name="T">指定类型</typeparam>
        /// <param name="pObject">内存对象</param>
        /// <param name="pBinaryFullName">目标binary文件全名</param>
        public static void SaveObjToBinaryFile<T>(T pObject, string pBinaryFullName)
        {
            try
            {
                using (FileStream fs = new FileStream(pBinaryFullName, FileMode.Create))
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    formatter.Serialize(fs, pObject);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 把指定binary文件实例化为指定类型的对象
        /// </summary>
        /// <typeparam name="T">指定类型</typeparam>
        /// <param name="pBinaryFullName">指定binary文件全名</param>
        /// <returns></returns>
        public static T LoadFromBinaryFile<T>(string pBinaryFullName)
        {
            T obj = default(T);

            try
            {
                using (FileStream fs = new FileStream(pBinaryFullName, FileMode.Open))
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    obj = (T)formatter.Deserialize(fs);
                    fs.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return obj;
        }
    }
}
