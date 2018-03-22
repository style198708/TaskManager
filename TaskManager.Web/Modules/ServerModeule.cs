using Nancy;
using Nancy.ModelBinding;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Modules;
using TaskManager.Utility;
using TaskManager.Utility.Quartz;

namespace TaskManager.Web.Modules
{
    public class ServerModeule : BaseModule
    {
        public ServerModeule() : base("Server")
        {
            Get["/Grid"] = r =>
            {
                return View["Grid"];
            };
            //保存数据
            Post["/"] = r =>
            {
                ServerUtil TaskUtil = this.Bind<ServerUtil>();
                return Response.AsJson(ServerHelp.SaveServer(TaskUtil));
            };

            Get["/Install/{Id}"] = r =>
            {
                ApiResult<string> result = new ApiResult<string>();
                string ServerId = r.Id;
                try
                {
                    ServerUtil serverUtil = ServerHelp.GetServerByID(ServerId);
                    string Msg = string.Empty;
                    ServerHelp.InstallWindowService(serverUtil, ref Msg);
                    result.Message = Msg;
                }
                catch (Exception ex)
                {
                    result.HasError = true;
                    result.Message = ex.Message;
                }
                return Response.AsJson(result);


            };
            Get["/UnInstall/{Id}"] = r =>
            {
                ApiResult<string> result = new ApiResult<string>();
                string ServerId = r.Id;
                try
                {
                    ServerUtil serverUtil = ServerHelp.GetServerByID(ServerId);
                    string Msg = string.Empty;
                    ServerHelp.UnInstallWindowService(serverUtil.ServerName, ref Msg);
                    result.Message = Msg;
                }
                catch (Exception ex)
                {
                    result.HasError = true;
                    result.Message = ex.Message;
                }
                return Response.AsJson(result);
            };
            Get["/Start/{Id}"] = r =>
            {
                ApiResult<string> result = new ApiResult<string>();
                string ServerId = r.Id;
                try
                {

                    ServerUtil serverUtil = ServerHelp.GetServerByID(ServerId);
                    string Msg = string.Empty;
                    bool flag = ServerHelp.StartWindowsService(serverUtil.ServerName, ref Msg);
                    if (flag)
                    {
                        serverUtil.Status = true;
                        ServerHelp.UpdateServer(serverUtil);
                    }
                    result.HasError = false;
                    result.Message = Msg;
                }
                catch (Exception ex)
                {
                    result.HasError = true;
                    result.Message = ex.Message;
                }
                return Response.AsJson(result);

            };
            Get["/Stop/{Id}"] = r =>
            {
                ApiResult<string> result = new ApiResult<string>();
                string ServerId = r.Id;
                try
                {
                    ServerUtil serverUtil = ServerHelp.GetServerByID(ServerId);
                    string Msg = string.Empty;
                    bool flag = ServerHelp.StopWindowsService(serverUtil.ServerName, ref Msg);
                    if (flag)
                    {
                        serverUtil.Status = false;
                        ServerHelp.UpdateServer(serverUtil);
                    }
                    result.Message = Msg;
                }
                catch (Exception ex)
                {
                    result.HasError = true;
                    result.Message = ex.Message;
                }
                return Response.AsJson(result);
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
                return Response.AsJson(ServerHelp.Query(condition));
            };


        }
    }
}
