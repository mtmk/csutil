using System;
using System.IO;
using System.ServiceProcess;
using System.Threading;
using csutil.WindowsService;

namespace csutil.cmd
{
    class Program
    {
        static Program()
        {
            var logFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "log4net.config");
            log4net.Config.XmlConfigurator.Configure(new FileInfo(logFile));
        }

        static void Main(string[] args)
        {
            var exitHandle = new ManualResetEvent(false);
            var logger = new Log4NetLogger();
            var utilServer = new UtilServer(logger, exitHandle);
            var windowsService = new UtilsWindowsService(utilServer, logger);

            if (args.Length == 0)
            {
                ServiceBase.Run(windowsService);
            }
            else switch (args[0])
                {
                    case "console":
                        utilServer.Start();
                        Console.CancelKeyPress += (s, e) =>
                                                      {
                                                          e.Cancel = true;
                                                          utilServer.Stop();
                                                      };
                        break;
                    case "install":
                        if (!windowsService.IsInstalled())
                            windowsService.Install();
                        break;
                    case "uninstall":
                        if (windowsService.IsInstalled())
                            windowsService.Uninstall();
                        break;
                }
        }

    }
}
