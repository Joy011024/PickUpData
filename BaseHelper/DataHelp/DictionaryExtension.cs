using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.AccessControl;
using System.IO;
using System.Security.Principal;
using System.Management;
namespace DataHelp
{
    public static class DictionaryExtension
    {
        /// <summary>
        /// 从字符数据中找出匹配的字典数据（字符匹配字典的关键字）
        /// </summary>
        /// <param name="define"></param>
        /// <param name="inFactColumn"></param>
        /// <returns></returns>
        public  static Dictionary<string, string> MapDictionary(this Dictionary<string, string> define, string[] inFactColumn)
        {
            Dictionary<string, string> exisit = new Dictionary<string, string>();//实际上匹配的列
            foreach (KeyValuePair<string, string> item in define)
            {
                for (int i = 0; i < inFactColumn.Length; i++)
                {
                    if (inFactColumn[i] == item.Key)
                    {
                        exisit.Add(item.Key, item.Value);
                        break;
                    }
                }
            }
            return exisit;
        }

        //C# 在进行io操作时经常遇到对于路径没有操作权限的情况，这个时候我们需要动态的提供权限进行IO处理
        public static void AddDirectorySecurity(string FileName, string Account, FileSystemRights[] rights)
        {//提供读写权限
            //读取该路径的原始操作权限
            //提供权限以实现功能操作,
            //还原权限
            FileSystemRights Rights = new FileSystemRights();
            foreach (FileSystemRights item in rights)
            {
                Rights = Rights | item;
            }
            //Read  ChangePermissions FullControl Write
            bool ok;
            DirectoryInfo dInfo = new DirectoryInfo(FileName);
            DirectorySecurity dSecurity = dInfo.GetAccessControl();
            InheritanceFlags iFlags = new InheritanceFlags();
            iFlags = InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit;
            FileSystemAccessRule AccessRule2 = new FileSystemAccessRule(Account, Rights, iFlags, PropagationFlags.None, AccessControlType.Allow);
            dSecurity.ModifyAccessRule(AccessControlModification.Add, AccessRule2, out ok);
            dInfo.SetAccessControl(dSecurity);
            //列出目标目录所具有的权限
            DirectorySecurity sec = Directory.GetAccessControl(FileName, AccessControlSections.All);
            foreach (FileSystemAccessRule rule in sec.GetAccessRules(true, true, typeof(System.Security.Principal.NTAccount)))
            {
                if ((rule.FileSystemRights & FileSystemRights.Read) != 0)
                    Console.WriteLine(rule.FileSystemRights.ToString());
            }
            Console.Read();
        }
        public static List<FileSystemAccessRule> GetAllSystemRights(this DirectoryInfo di) 
        {
            if (di == null) 
            {
                return null;
            }
            string path = di.FullName;//获取提供的目录信息
            //获取某个路径下全部权限
            DirectorySecurity ds = Directory.GetAccessControl(path, AccessControlSections.Access);
            List<FileSystemAccessRule> data = new List<FileSystemAccessRule>();
            AuthorizationRuleCollection ars=ds.GetAccessRules(true, true, typeof(System.Security.Principal.NTAccount));
            foreach (FileSystemAccessRule item in ars)
            {
                data.Add(item);
            }
            return data;
        }
        public static void CreateDirectory(this DirectoryInfo di) 
        {
            //是否可以直接创建
            //如果不能直接创建是否是没有操作的相关权限
            //提取修改前的权限列表
            //增加一个新增权限
            //新建文件夹
            //还原到之前的权限列表

            if (di.Exists) 
            {//已存在不需要处理
                return;
            }
            try 
            {
                di.Create();//创建目录
            }catch(Exception ex)
            {
                //获取当前执行程序的window用户
                string user= di.GetCurrentActiveUser();
                DirectoryInfo d = di.Parent;
                DirectorySecurity ds = d.GetAccessControl(AccessControlSections.Access);
                ds.AddAccessRule(new FileSystemAccessRule(user,FileSystemRights.Write,AccessControlType.Allow));
                d.SetAccessControl(ds);//出现异常  尝试执行未经授权的操作。
                d.Create(new DirectorySecurity(di.FullName, AccessControlSections.Access));
            }        }
        //判断是否以管理员身份运行程序
        public static bool IsAdministrator(this DirectoryInfo di)
        {
            WindowsIdentity identity = WindowsIdentity.GetCurrent();
            WindowsPrincipal principal = new WindowsPrincipal(identity);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }
        public static void CanCreate(DirectoryInfo di) 
        {//是否具备权限在当前提供的目录下新建文件或者文件夹
            FileSystemRights right = FileSystemRights.Write;
            DirectorySecurity ds = di.GetAccessControl();
            
        }
    }
}
