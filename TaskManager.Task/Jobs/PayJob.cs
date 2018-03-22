//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Quartz;
//using TaskManager.Utility;
//using System.Threading;
//using TaskManager.Utility.Quartz;
//using DMSFrame;

//namespace TaskManager.Task.Jobs
//{
//    /// <summary>
//    /// 退款任务1111
//    /// </summary>
//    [DisallowConcurrentExecution]
//    public class PayJob : BaseJob, IJob, IDisposable
//    {
//        public void Execute(IJobExecutionContext context)
//        {
//            LogHelper.WriteLog("开始退款任务");
//            DateTime beg = DateTime.Now;
//            int successCount = 0, errCount = 0;
//            string errMsg = string.Empty;
//            try
//            {
//                bool resultFlag = false;
//                SaveMonitor(beg, "退款任务");
//                if (resultFlag && errCount == 0)
//                {
//                    LogHelper.WriteLog(string.Format("【退款任务】: 成功:{0},错误:{1}", successCount, errCount));
//                }
//                else
//                {
//                    LogHelper.WriteLog(string.Format("【退款任务】:成功:{0},错误:{1},信息:{2}", successCount, errCount, errMsg));
//                }
//            }
//            catch (Exception ex)
//            {
//                while (ex.InnerException != null)
//                {
//                    ex = ex.InnerException;
//                }
//                LogHelper.WriteLog(string.Format("【退款任务】成功:{0},错误:{1},信息:{2}", successCount, errCount, errMsg));

//                JobExecutionException e2 = new JobExecutionException(ex);
//                LogHelper.WriteLog("错误信息", ex);
//                //1.立即重新执行任务 
//                e2.RefireImmediately = true;
//            }
//            LogHelper.WriteLog("结束退款任务");
//        }
//        public void Dispose()
//        {
//            GC.Collect();
//        }
//    }
//}
