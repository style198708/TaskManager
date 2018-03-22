using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using TaskManager.Utility;
using Nancy.Hosting.Self;
using Owin_Nancy;
using TaskManager.Utility.Mef;
using TaskManager.Utility.ConfigHandler;
using TaskManager.Utility.Command;
using System.Runtime.InteropServices;
using TaskManager.Utility.RabbitMQ;
using TaskManager.Utility.Extensions.Xml;
using TaskManager.Service;
using System.Threading;

namespace TaskManager.WindowService
{
    partial class TaskService : ServiceBase
    {
        public delegate bool ControlCtrlDelegate(int CtrlType);
        [DllImport("kernel32.dll")]
        private static extern bool SetConsoleCtrlHandler(ControlCtrlDelegate HandlerRoutine, bool Add);
        private static ControlCtrlDelegate cancelHandler = new ControlCtrlDelegate(HandlerRoutine);

        public static bool HandlerRoutine(int CtrlType)
        {
            switch (CtrlType)
            {
                case 0:
                case 2:
                    return true;
            }
            return false;
        }

        public TaskService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            AppDomain.MonitoringIsEnabled = true;

            AdminRun.Run();

            DebuggableAttribute att = System.Reflection.Assembly.GetExecutingAssembly().GetCustomAttribute<DebuggableAttribute>();
            if (att.IsJITTrackingEnabled)
            {
                //Debug模式才让线程停止10s,方便附加到进程调试
                Thread.Sleep(10000);
            }
            if (!SetConsoleCtrlHandler(cancelHandler, true))
            {
                LogHelper.WriteLog("程序监听系统按键异常");
            }
            try
            {
                //1.MEF初始化
                MefConfig.Init();
                LogHelper.WriteLog("MEF初始化");

                //2.数据库初始化连接
                ConfigInit.InitConfig();
                LogHelper.WriteLog("数据库初始化连接");

                //3.系统参数配置初始化
                ConfigManager configManager = MefConfig.TryResolve<ConfigManager>();
                configManager.Init();
                LogHelper.WriteLog("系统参数配置初始化");

                //4.任务启动
                QuartzHelper.InitScheduler();
                QuartzHelper.StartScheduler();
                LogHelper.WriteLog("任务启动");

                //5.加载SQL信息到缓存中
                XmlCommandManager.LoadCommnads(SysConfig.XmlCommandFolder);
                LogHelper.WriteLog("加载SQL信息到缓存中");

                //测试dapper orm框架
                //DapperDemoService.Test();

                #region 控制台用死循环监听

                ////启动站点
                //using (NancyHost host = Startup.Start(SystemConfig.WebPort))
                //{
                //    //调用系统默认的浏览器   
                //    string url = string.Format("http://127.0.0.1:{0}", 9059);
                //    Process.Start(url);
                //    LogHelper.WriteLog( string.Format("系统已启动，当前监听站点地址:{0}", url));

                //    //4.消息队列启动
                //    RabbitMQClient.InitClient();

                //    LogHelper.WriteLog("消息队列启动");

                //    ////5.系统命令初始化

                //    CommandHelp.Init();
                //    LogHelper.WriteLog("系统命令初始化");
                //}

                #endregion

                ///Window服务用线程池
                ThreadPool.QueueUserWorkItem((o) =>
                {
                    //启动站点
                    string url = string.Format("http://127.0.0.1:{0}", SysConfig.WebPort);
                    Startup.Start(SysConfig.WebPort);
                    LogHelper.WriteLog(string.Format("系统已启动，当前监听站点地址:{0}", url));

                    //4.消息队列启动
                    //RabbitMQClient.InitClient();
                    LogHelper.WriteLog("消息队列启动");
                });
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(ex.Message);
            }
        }

        protected override void OnStop()
        {
            LogHelper.WriteLog("服务已停止");
        }
    }
}
