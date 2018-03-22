using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Utility.Quartz;

namespace TaskManager.Task.Jobs
{
    public class BaseJob
    {
        /// <summary>
        /// 增加监控
        /// </summary>
        /// <param name="beg"></param>
        /// <param name="TaskName"></param>
        public void SaveMonitor(DateTime beg,string TaskName)
        {
            ///只能监控当前域的CPU 内存问题 不能指定到程上面
            var cpu = AppDomain.CurrentDomain.MonitoringTotalProcessorTime.Seconds;
            double memorysize = (double)AppDomain.CurrentDomain.MonitoringSurvivedMemorySize / 1024 / 1024;
            DateTime end = DateTime.Now;
            TaskMonitorUtil util = new TaskMonitorUtil()
            {
                Cpu = cpu,
                TaskName = TaskName,
                Memory = memorysize,
                ExecutionSecond = (end - beg).TotalSeconds,
            };
            TaskMonitorHelp.SaveTask(util);
        }
    }
}
