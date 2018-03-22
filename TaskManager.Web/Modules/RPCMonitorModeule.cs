using TaskManager.Utility;
using Nancy;
using Nancy.ModelBinding;
using System;
using TaskManager.Utility.Quartz;

namespace TaskManager.Modules
{
   public class RPCMonitorModeule: BaseModule
    {
        public RPCMonitorModeule():base("RPCMonitor")
        {
            Get["/Grid"] = r =>
            {
                return View["Grid"];
            };
            //保存数据
            Post["/"] = r =>
            {
                RPCMonitorUtil TaskUtil = this.Bind<RPCMonitorUtil>();
                return Response.AsJson(RPCMonitorHelp.SaveTask(TaskUtil));
            };
            //任务编辑界面
            Get["/Edit"] = r =>
            {
                return View["Edit"];
            };
            //列表查询接口
            Post["/PostQuery"] = r =>
            {
                QueryCondition condition = this.Bind<QueryCondition>();
                return Response.AsJson(RPCMonitorHelp.Query(condition));
            };

        }
    }
}
