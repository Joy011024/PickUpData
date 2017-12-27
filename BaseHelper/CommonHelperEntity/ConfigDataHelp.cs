using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using Common.Data;
namespace CommonHelperEntity
{
    public class ConfigDataHelp
    {
        /// <summary>
        /// 设置配置文件的数据
        /// </summary>
        /// <param name="appSetting">配置项和值的键值对</param>
        /// <param name="configPath">配置文件的路径【默认查找exe文件的配置】</param>
        /// <returns></returns>
        public static bool SetAppsettingValue(Dictionary<string,string> appSetting,string configPath="") 
        {
            try
            {
                Configuration cfg;
                if (!string.IsNullOrEmpty(configPath))
                {
                    ExeConfigurationFileMap fileMap = new ExeConfigurationFileMap();
                    fileMap.ExeConfigFilename = configPath;
                    cfg = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);
                }
                else 
                {
                    cfg = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                }
                foreach (KeyValuePair<string, string> kv in appSetting)
                {
                    KeyValueConfigurationElement element = cfg.AppSettings.Settings[kv.Key];
                    if (element == null)
                    {//不存在配置
                        continue;
                    }
                    cfg.AppSettings.Settings[kv.Key].Value = kv.Value;
                }
                cfg.Save();
                return true;
            }
            catch (Exception ex) 
            {
                LogHelper help = new LogHelper();
                help.AppendLogUsingProjectPath(ex.ToString(), "Log/Config", "Config" + CommonFormat.DateToMinuteIntFormat + FileSuffix.Log, new int?());
                return false;
            }
        }
    }
}
