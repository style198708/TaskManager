using System;
using System.Collections;
using System.Collections.Generic;

namespace TaskManager.Utility.Quartz
{

    /// <summary>
    /// 任务性能
    /// </summary>
    public class TaskMonitorUtil
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
        public double Cpu { get; set; }

        /// <summary>
        /// 内存占用
        /// </summary>
        public double Memory { get; set; }

        /// <summary>
        /// 执行时间
        /// </summary>
        public double ExecutionSecond { get; set; }

    }

    public class TaskMonitorHelp
    {
        private static string InsertSQL = @"INSERT INTO P_TaskMonitor(TaskName,Cpu,Memory,ExecutionSecond,CreatedOn) VALUES(@TaskName,@Cpu,@Memory,@ExecutionSecond,getdate())";

        /// <summary>
        /// 根据条件查询任务
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <returns>符合条件的任务</returns>
        public static ApiResult<List<TaskMonitorUtil>> Query(QueryCondition condition)
        {
            ApiResult<List<TaskMonitorUtil>> result = new ApiResult<List<TaskMonitorUtil>>();
            if (string.IsNullOrEmpty(condition.SortField))
            {
                condition.SortField = "CreatedOn";
                condition.SortOrder = "DESC";
            }
            Hashtable ht = Pagination.QueryBase<TaskMonitorUtil>("select * from [P_TaskMonitor] where TaskMonitorID in(select MAX(TaskMonitorID) from  [dbo].[P_TaskMonitor] group by TaskName)", condition);
            result.Result = ht["data"] as List<TaskMonitorUtil>;
            result.TotalCount = Convert.ToInt32(ht["total"]);
            result.TotalPage = result.CalculateTotalPage(condition.PageSize, result.TotalCount.Value, condition.IsPagination);
            return result;
        }

        public static int SaveTask(TaskMonitorUtil value)
        {
            if (value == null)
            {
                return 0;
            }
            SQLHelper.ExecuteNonQuery(InsertSQL, value);
            return value.TaskMonitorID;
        }
    }
}
