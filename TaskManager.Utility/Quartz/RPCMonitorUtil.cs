using System;
using System.Collections;
using System.Collections.Generic;

namespace TaskManager.Utility.Quartz
{
    public class RPCMonitorUtil
    {
        /// <summary>
        /// 编号
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// Rpc地址
        /// </summary>
        public string IP { get; set; }
        /// <summary>
        /// 端口
        /// </summary>
        public int Port { get; set; }
        /// <summary>
        /// 程序集
        /// </summary>
        public string Assembly { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// 执行方法
        /// </summary>
        public string Method { get; set; }
        /// <summary>
        /// 是否正常
        /// </summary>
        public bool IsSuccess { get; set; }
    }

    public class RPCMonitorHelp
    {

        private static string InsertSQL = @"INSERT INTO P_RPCMonitor(IP,Port,Assembly,Type,Method,IsSuccess,CreatedOn) VALUES(@IP,@Port,@Assembly,@Type,@Method,0,getdate())";

        /// <summary>
        /// 根据条件查询任务
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <returns>符合条件的任务</returns>
        public static ApiResult<List<RPCMonitorUtil>> Query(QueryCondition condition)
        {
            ApiResult<List<RPCMonitorUtil>> result = new ApiResult<List<RPCMonitorUtil>>();
            if (string.IsNullOrEmpty(condition.SortField))
            {
                condition.SortField = "CreatedOn";
                condition.SortOrder = "DESC";
            }
            Hashtable ht = Pagination.QueryBase<RPCMonitorUtil>("select * from P_RPCMonitor", condition);
            result.Result = ht["data"] as List<RPCMonitorUtil>;
            result.TotalCount = Convert.ToInt32(ht["total"]);
            result.TotalPage = result.CalculateTotalPage(condition.PageSize, result.TotalCount.Value, condition.IsPagination);
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static ApiResult<string> SaveTask(RPCMonitorUtil value)
        {
            ApiResult<string> result = new ApiResult<string>();
            if (value == null)
            {
                result.Message = "参数空异常";
                return result;
            }
            SQLHelper.ExecuteNonQuery(InsertSQL, value);
            result.HasError = true;
            result.Message = "保存成功";
            result.Result = value.ID.ToString();
            return result;
           
        }
    }
}
