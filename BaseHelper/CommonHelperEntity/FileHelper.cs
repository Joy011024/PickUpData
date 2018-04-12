using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
namespace CommonHelperEntity
{
    public  class FileHelper
    {
        public static string Message { get; private set; }
        public static byte[] GetFileBytes(Stream stream) 
        {
            if (stream == null) 
            {
                Message = "the param of stream is null,is error";
                return new  byte[0];
            }
            byte[] bytes=new byte[stream.Length];
            stream.Read(bytes, 0, bytes.Length);
            return bytes;
        }
        public  byte[] GetFileStream(string path) 
        {
            //读取文件流总耗时计算
            Console.WriteLine("start\t"+DateTime.Now.ToString());
            if (!File.Exists(path)) 
            {
                  return new byte[0];
            }
            byte[] bytes = new byte[0];
            FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(stream);
            br.BaseStream.Seek(0, SeekOrigin.Begin);
            int length = (int)br.BaseStream.Length;
            bytes = br.ReadBytes(length);
            Console.WriteLine("end\t" + DateTime.Now.ToString());
            br.Close();
            stream.Close();
            return bytes;
        }
        public byte[] GetDirectFileStream(string path)
        {
            Console.WriteLine("start\t" + DateTime.Now.ToString());
            if (!File.Exists(path))
            {
                return new byte[0];
            }
            byte[] bytes = new byte[0];
            FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read);
            int length = (int)stream.Length;
            stream.Read(bytes, 0, length);
            Console.WriteLine("end\t" + DateTime.Now.ToString());
            stream.Close();
            return bytes;
        }
        public List<string> ReadText(string path) 
        {
            FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
            StreamReader sr = new StreamReader(fs,Encoding.Default);//在读取text出现中文设置utf-8解析乱码，但是为default可以解析。
            List<string> sb = new List<string>();
            string line=string.Empty;
            while(!string.IsNullOrEmpty(( line=sr.ReadLine())))
            {
                sb.Add(line);
            }
            sr.Close();
            fs.Close();
            return sb;
        }
        /// <summary>
        /// 读取第一行非空数据
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public string ReadFirstIsNoEmptyRow(string path) 
        {
            FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
            StreamReader sr = new StreamReader(fs, Encoding.Default);//在读取text出现中文设置utf-8解析乱码，但是为default可以解析。
            string line = sr.ReadLine();
            if (!string.IsNullOrEmpty(line)) 
            {
                sr.Close();
                fs.Close();
                return line;
            }
            while (sr.Peek()>0)
            {
                line = sr.ReadLine();
                if (!string.IsNullOrEmpty(line)) 
                {
                    return line;
                }
            }
            sr.Close();
            fs.Close();
            return line;;
        }
    }
    public  class FileFormatExt
    {
        public static string ReadFileUtf8Text(string filePath)
        {
            if (!File.Exists(filePath))
            {//文件不存在
                return null;
            }
            FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            StreamReader sr = new StreamReader(fs, GetFileEncode(fs));
            string txt = sr.ReadToEnd();
            sr.Close();
            fs.Close();
            return txt;
        }
        public static Encoding GetFileEncode(FileStream fs)
        {
            byte[] Unicode = new byte[] { 0xFF, 0xFE, 0x41 };
            byte[] UnicodeBIG = new byte[] { 0xFE, 0xFF, 0x00 };
            byte[] UTF8 = new byte[] { 0xEF, 0xBB, 0xBF };//带BOM
            Encoding reVal = Encoding.Default;
            BinaryReader br = new BinaryReader(fs);
            int length;
            int.TryParse(fs.Length.ToString(), out length);
            byte[] ss = br.ReadBytes(length);
            if (IsUTF8Bytes(ss) ||
                (ss[0] == UTF8[0] && ss[1] == UTF8[1] && ss[2] == UTF8[2]))
                reVal = Encoding.UTF8;
            else if (ss[0] == UnicodeBIG[0] && ss[1] == UnicodeBIG[1] && ss[2] == UnicodeBIG[2])
                reVal = Encoding.BigEndianUnicode;
            else if (ss[0] == Unicode[0] && ss[1] == Unicode[1] && ss[2] == Unicode[2])
                reVal = Encoding.Unicode;
            br.Close();

            return reVal;
        }
        private static bool IsUTF8Bytes(byte[] data)
        {//判断是否是不带BOM的UTF8格式
            int charByteCounter = 1;//计算当前正分析的字符应还有的字节数
            byte curByte;//当前分析的字节
            for (int i = 0; i < data.Length; i++)
            {
                curByte = data[i];
                if (charByteCounter == 1)
                {
                    if (curByte >= 0x80)
                    {
                        //判断当前
                        while (((curByte <<= 1) & 0x80) != 0)
                        {
                            charByteCounter++;
                        }
                        //标记位首位若为非0 则至少以2个1开始，如：110XXXXX.....1111110X
                        if (charByteCounter == 1 || charByteCounter > 6)
                        {
                            return false;
                        }
                    }
                }
                else
                {
                    //若是UTF-8 此时第一位必须为1
                    if ((curByte & 0xC0) != 0x80)
                    {
                        return false;
                    }
                    charByteCounter--;
                }
            }
            if (charByteCounter > 1)
            {
                throw new Exception("非预期的byte格式");
            }
            return true;
        }
    }
}
