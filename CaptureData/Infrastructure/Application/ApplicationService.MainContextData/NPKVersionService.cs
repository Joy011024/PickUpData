using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Domain.MainContextData;
using InfrastructureRepository.IMainContext;
using Common.Data;
namespace ApplicationService.MainContextData
{
    public class NPKVersionService
    {
        public string SqlConnString { get; private set; }
        public IMainRepository<NPKVersion> Repository { get; set; }
        public NPKVersionService(string connString,IMainRepository<NPKVersion> respority) 
        {
            SqlConnString = connString;
            Repository = respority;
        }
        /// <summary>
        /// 录入一条补丁记录
        /// </summary>
        /// <param name="npk"></param>
        public ProcessResponse InsertNPK(NPKVersion npk) 
        {
            ProcessResponse response = new ProcessResponse();
            //备注不能为空，对于每一次的补丁修改必须提供补丁内容
            if (string.IsNullOrEmpty(npk.Note))
            {
                response.ErrorCode = ENPKErrorCategory.NPKNoteIsEmpty.GetHashCode();
            }
            if (npk.NPKType == ENPKCategory.DBStruct.GetHashCode() && string.IsNullOrEmpty(npk.DBStructCmd)) 
            {//如果是数据库变动但是没有提供变动的命令语句
                response.ErrorCode = ENPKErrorCategory.DBStructChangeNoCmd.GetHashCode();
            }
            else if (string.IsNullOrEmpty(npk.NPKPath) && string.IsNullOrEmpty(npk.DBStructCmd)) 
            {//补丁内容或者补丁路径必须存在一项内容
                response.ErrorCode = ENPKErrorCategory.CmdOrNkpPathBothEmpty.GetHashCode();
            }
            if (response.ErrorCode.HasValue) 
            {
                return response;
            }
            if (string.IsNullOrEmpty(npk.NPKEffectVersion))
            { //如果没有设置影响版本号，则添加一个时间戳
                npk.NPKEffectVersion = DateTime.Now.ToString(CommonFormat.DateTimeIntFormat);
            }
            bool succ= Repository.Insert(npk);
            if (succ)
            {
                return new ProcessResponse();
            }
            else 
            {
                response.ErrorMsg = Repository.Message;
                return response;
            }

        }

    }
}
