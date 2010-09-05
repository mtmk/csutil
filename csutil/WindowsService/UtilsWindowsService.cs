using System.Collections;
using System.Configuration.Install;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;

namespace csutil.WindowsService
{
    public sealed class UtilsWindowsService : ServiceBase
    {
        private const string Tag = "UtilsWindowsService";
        private readonly UtilServer _server;
        private readonly ISimpleLogger _logger;

        public UtilsWindowsService(UtilServer server, ISimpleLogger logger)
        {
            _server = server;
            _logger = logger;
            EventLog.Log = "Application";
            ServiceName = "Example Service";
            CanStop = true;
        }

        protected override void OnStart(string[] args)
        {
            base.OnStart(args);
            _logger.Log(Tag, "Starting service {0}", ServiceName);
            _server.Start();
        }

        protected override void OnStop()
        {
            base.OnStop();
            _logger.Log(Tag, "Stopping service {0}", ServiceName);
            _server.Stop();
        }

        public bool IsInstalled()
        {
            return ServiceController.GetServices().Any(service => service.ServiceName == ServiceName);
        }

        public void Install()
        {
            _logger.Log(Tag, "Installing service {0}", ServiceName);
            using (var ti = new TransactedInstaller())
            {
                SetInstallers(ti);
                ti.Install(new Hashtable());
            }
        }

        public void Uninstall()
        {
            _logger.Log(Tag, "Installing service {0}", ServiceName);
            using (var ti = new TransactedInstaller())
            {
                SetInstallers(ti);
                ti.Uninstall(null);
            }
        }

        private void SetInstallers(Installer ti)
        {
            ti.Installers.Add(new ServiceProcessInstaller { Account = ServiceAccount.LocalSystem });
            ti.Installers.Add(new ServiceInstaller { Description = ServiceName, ServiceName = ServiceName, StartType = ServiceStartMode.Automatic });
            ti.Context = new InstallContext(null, new[] { "/assemblypath=" + Process.GetCurrentProcess().MainModule.FileName });
        }
    }
}