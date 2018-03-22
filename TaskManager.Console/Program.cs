using TaskManager.Utility;
using Nancy.Hosting.Self;
using Owin_Nancy;
using System;
using System.Diagnostics;
using TaskManager.Utility.Mef;
using TaskManager.Utility.ConfigHandler;
using TaskManager.Utility.Command;
using System.Runtime.InteropServices;
using TaskManager.Utility.RabbitMQ;
using TaskManager.Utility.Extensions.Xml;

namespace TaskManager.Console
{
    class Program
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

        static void Main(string[] args)
        {
            AppDomain.MonitoringIsEnabled = true;

            AdminRun.Run();

            if (!SetConsoleCtrlHandler(cancelHandler, true))
            {
                LogHelper.WriteLog("程序监听系统按键异常");
            }
            try
            {
                //1.MEF初始化
                MefConfig.Init();

                //2.数据库初始化连接
                ConfigInit.InitConfig();

                //3.系统参数配置初始化
                ConfigManager configManager = MefConfig.TryResolve<ConfigManager>();
                configManager.Init();

                //4.任务启动
                QuartzHelper.InitScheduler();
                QuartzHelper.StartScheduler();


                //5.加载SQL信息到缓存中
                XmlCommandManager.LoadCommnads(SysConfig.XmlCommandFolder);

                //测试dapper orm框架
                //DapperDemoService.Test();

                //启动站点
                using (NancyHost host = Startup.Start(SysConfig.WebPort))
                {
                    //调用系统默认的浏览器   
                    string url = string.Format("http://127.0.0.1:{0}", SysConfig.WebPort);
                    Process.Start(url);
                    System.Console.WriteLine("系统已启动，当前监听站点地址:{0}", url);
                    //4.消息队列启动
                    RabbitMQClient.InitClient();

                    //5.系统命令初始化
                    CommandHelp.Init();
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex);
            }
            System.Console.Read();
        }


    }
}
