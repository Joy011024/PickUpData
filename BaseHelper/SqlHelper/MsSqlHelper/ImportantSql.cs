using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MsSqlHelper
{
    public class ImportantSql
    {
        public static string GetUserTablesData = "select * from sysobjects where xtype='u' and status>=0 and category=0";
        public static string GetUserTablesName = "select name from sysobjects where xtype='u' and status>=0 and category=0";
        /// <summary>
        /// 读取指定表中的全部列信息
        /// </summary>
        public static string GetTargetTableAllFiledInfo = "select * from sys.columns where object_id=object_id('{0}')";
        /// <summary>
        /// 选择指定表的指定列进行导出操作：参数{0} 列（field），{1} 表{table}
        /// </summary>
        public static string GetSelectTbaleSelectFields = "select {0} from {1}";
    }
}
