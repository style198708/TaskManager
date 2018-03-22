using TaskManager.Utility;
using Nancy;
using Nancy.ModelBinding;
using System;

namespace TaskManager.Modules
{
    public class SystemMonitorModule: BaseModule
    {
        public SystemMonitorModule():base("SystemMonitor")
        {
            Get["/Grid"] = r =>
            {
                return View["Grid"];
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
                return Response.AsJson(TaskHelper.Query(condition));
            };
        }
    }
}
