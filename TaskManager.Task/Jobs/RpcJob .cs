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
//using TaskManager.Entity;
//using System.Reflection;
//using Thrift.Protocol;
//using Thrift.Transport;

//namespace TaskManager.Task.Jobs
//{
//    [DisallowConcurrentExecution]
//    public class RpcJob : BaseJob, IJob, IDisposable
//    {
//        /// <summary>
//        /// RPC监控
//        /// </summary>
//        /// <param name="context"></param>
//        public void Execute(IJobExecutionContext context)
//        {
//            LogHelper.WriteLog("开始RPC监控任务");
//            List<P_RPCMonitor> list = DMS.Create<P_RPCMonitor>().ToList();
//            string errMsg = string.Empty;
//            DateTime beg = DateTime.Now;
//            DMSTransactionScopeEntity tsEntity = new DMSTransactionScopeEntity();
//            foreach (P_RPCMonitor p in list)
//            {
//                try
//                {
//                    Ex(p);
//                    tsEntity.EditTS(new P_RPCMonitor() { IsSuccess = true }, q => q.ID == p.ID);
//                    LogHelper.WriteLog(string.Format("{0}.{1}调用成功", p.Type, p.Method));
//                }
//                catch (Exception ex)
//                {
//                    tsEntity.EditTS(new P_RPCMonitor() { IsSuccess = false }, q => q.ID == p.ID);
//                    LogHelper.WriteLog(string.Format("{0}.{1}调用失败 异常：{2}", p.Type, p.Method, ex.Message));
//                    continue;
//                }
//            }
//            if (new DMSTransactionScopeHandler().Update(tsEntity, ref errMsg))
//            {
//                LogHelper.WriteLog("状态更新成功");
//            }
//            SaveMonitor(beg, "RPC监控任务");
//            LogHelper.WriteLog("结束RPC监控任务");
//        }

//        /// <summary>
//        /// i 
//        /// </summary>
//        /// <param name="monitor"></param>
//        public void Ex(P_RPCMonitor monitor)
//        {

//            TTransport transport = new TSocket(monitor.IP, monitor.Port.Value);

//            transport.Open();

//            TProtocol protocol = new TBinaryProtocol(transport);

//            Assembly tempAssembly = Assembly.LoadFrom(monitor.Assembly);

//            Type type = tempAssembly.GetType(monitor.Type);

//            Object obj = Activator.CreateInstance(type, protocol);

//            MethodInfo info = type.GetMethod(monitor.Method);

//            ParameterInfo[] parameter = info.GetParameters();

//            object[] ps = new object[parameter.Length];

//            int index = 0;
//            foreach (ParameterInfo pinfo in parameter)
//            {
//                if (pinfo.ParameterType == typeof(Int32) || pinfo.ParameterType == typeof(double) || pinfo.ParameterType == typeof(float) || pinfo.ParameterType == typeof(decimal))
//                {
//                    ps[index] = 0;
//                }
//                if (pinfo.ParameterType == typeof(bool))
//                {
//                    ps[index] = true;

//                }
//                if (pinfo.ParameterType == typeof(char))
//                {
//                    ps[index] = 'a';

//                }
//                if (pinfo.ParameterType == typeof(string))
//                {
//                    ps[index] = "test";
//                }
//            }
//            object ss = info.Invoke(obj, ps);

//        }

//        public void Dispose()
//        {
//            GC.Collect();
//        }
//    }

//}
