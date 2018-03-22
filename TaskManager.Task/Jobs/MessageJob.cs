//using CSTJR.Message.OutService;
//using Quartz;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using TaskManager.Utility;

//namespace TaskManager.Task.Jobs
//{
//    public class MessageJob : BaseJob, IJob
//    {
//        /// <summary>
//        ///  消息执行任务
//        /// </summary>
//        /// <param name="context"></param>
//        public void Execute(IJobExecutionContext context)
//        {
//            LogHelper.WriteLog("开始活动推送任务");
//            DateTime beg = DateTime.Now;
//            int successCount = 0, errCount = 0;
//            string errMsg = string.Empty;
//            try
//            {
//                MessageService service = new MessageService();
//                service.SendTaskMessage(true, ref successCount, ref errCount, ref errMsg);

//                //MessageService.Client client = new MessageService.Client(RPCHelp.GetTransport("MessagePort"));
//                //JobResult result = client.SendTaskMessage();

//                SaveMonitor(beg, "活动推送任务");
//                if (errCount == 0)
//                {
//                    LogHelper.WriteLog(string.Format("【活动推送任务】: 成功:{0},错误:{1}", successCount, errCount));
//                }
//                else
//                {
//                    LogHelper.WriteLog(string.Format("【活动推送任务】:成功:{0},错误:{1},信息:{2}", successCount, errCount, errMsg));
//                }
//            }
//            catch (Exception ex)
//            {
//                while (ex.InnerException != null)
//                {
//                    ex = ex.InnerException;
//                }
//                LogHelper.WriteLog(string.Format("【活动推送任务】成功:{0},错误:{1},信息:{2}", successCount, errCount, errMsg));

//                JobExecutionException e2 = new JobExecutionException(ex);
//                LogHelper.WriteLog("错误信息", ex);
//                //1.立即重新执行任务 
//                e2.RefireImmediately = true;
//            }
//            LogHelper.WriteLog("结束活动推送任务");
//        }
//    }
//}
