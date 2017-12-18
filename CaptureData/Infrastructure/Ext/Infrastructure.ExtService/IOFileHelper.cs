using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
namespace Infrastructure.ExtService
{
    public class IOFileHelper
    {
        /// <summary>
        /// 文件拷贝【不能为相同路径】
        /// </summary>
        /// <param name="fileFullName">原路径</param>
        /// <param name="newFileFullName">新路径</param>
        public static void FileCopy(string fileFullName,string newFileDir,string newFileName)
        {
            fileFullName = fileFullName.Trim();
            string newFileFullName = newFileDir + "/" + newFileName;
            newFileFullName = newFileFullName.Trim();
            if (fileFullName.ToLower() == newFileFullName.ToLower())
            {
                return;
            }
            if (File.Exists(newFileFullName))
                File.Delete(newFileFullName);
            if (!Directory.Exists(newFileDir))
                Directory.CreateDirectory(newFileDir);
            File.Copy(fileFullName, newFileFullName);
        }
    }
}
