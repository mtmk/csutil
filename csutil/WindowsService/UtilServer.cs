using System;
using System.Threading;

namespace csutil.WindowsService
{
    public class UtilServer
    {
        private const string Tag = "UtilServer";
        private readonly ISimpleLogger _logger;
        private readonly ManualResetEvent _exitHandle;
        private Thread _worker;

        public UtilServer(ISimpleLogger logger, ManualResetEvent exitHandle)
        {
            _logger = logger;
            _exitHandle = exitHandle;
        }

        public void Start()
        {
            _worker = new Thread(() =>
                                     {
                                         _logger.Log(Tag, "Started worker");
                                         while (true)
                                         {
                                             _logger.Log(Tag, DateTime.Now.ToString("o"));
                                             var waitAny = WaitHandle.WaitAny(new[] { _exitHandle }, 1000);
                                             if (waitAny == WaitHandle.WaitTimeout) continue;
                                             _logger.Log(Tag, "Exiting worker");
                                             return;
                                         }
                                     });
            _worker.Start();
        }

        public void Stop()
        {
            _exitHandle.Set();
            _worker.Join();
        }
    }
}