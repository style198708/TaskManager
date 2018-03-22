using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Utility.Quartz
{

    /// <summary>
    /// 任务性能
    /// </summary>
    public class SystemMonitorUtil
    {
        /// <summary>
        /// 任务性能ID
        /// </summary>
        public int TaskMonitorID { get; set; }

        /// <summary>
        /// 任务名
        /// </summary>
        public string TaskName { get; set; }

        /// <summary>
        /// Cpu占比
        /// </summary>
        public string Cpu { get; set; }

        /// <summary>
        /// 内存占用
        /// </summary>
        public string Memory { get; set; }

        /// <summary>
        /// 执行时间
        /// </summary>
        public string ExecutionSecond { get; set; }

    }
    public class SystemMonitorHelp
    {
        /// <summary>
        /// 根据条件查询任务
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <returns>符合条件的任务</returns>
        public static ApiResult<List<SystemMonitorUtil>> Query(QueryCondition condition)
        {
            ApiResult<List<SystemMonitorUtil>> result = new ApiResult<List<SystemMonitorUtil>>();
            if (string.IsNullOrEmpty(condition.SortField))
            {
                condition.SortField = "CreatedOn";
                condition.SortOrder = "DESC";
            }
            Hashtable ht = Pagination.QueryBase<SystemMonitorUtil>("SELECT * FROM p_SystemMonitorUtil", condition);
            result.Result = ht["data"] as List<SystemMonitorUtil>;
            result.TotalCount = Convert.ToInt32(ht["total"]);
            result.TotalPage = result.CalculateTotalPage(condition.PageSize, result.TotalCount.Value, condition.IsPagination);
            return result;
        }
    }



}
