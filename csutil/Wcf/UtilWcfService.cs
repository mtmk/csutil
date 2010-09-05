using System;
using System.IO;
using System.ServiceModel;
using System.Threading;

namespace csutil.Wcf
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class UtilWcfService : IUtilWcfService
    {
        public UtilWcfService()
        {
            Console.WriteLine("[{0}] CTOR", Thread.CurrentThread.ManagedThreadId);
        }

        public string Echo(string name)
        {
            Console.WriteLine("[{0}] ECHO", Thread.CurrentThread.ManagedThreadId);
            Console.WriteLine("Server : Received : {0}", name);
            Thread.Sleep(5000);
            return name;
        }

        public void SendStream(Stream s)
        {
            int c = 0;
            while (c != -1)
            {
                c = s.ReadByte();
                Console.Write("{0} ", c);
            }
        }

        public static void Run(WaitHandle exitHandle)
        {
            var serviceHost = new ServiceHost(new UtilWcfService());
            serviceHost.AddServiceEndpoint(typeof (IUtilWcfService), new NetTcpBinding(SecurityMode.None){TransferMode = TransferMode.Streamed}, "net.tcp://localhost:54321");

            Console.WriteLine("WCF SERVER: Open");
            serviceHost.Open();
            Console.WriteLine("WCF SERVER: Wait");
            exitHandle.WaitOne();
            Console.WriteLine("WCF SERVER: Close");
            serviceHost.Close();
            Console.WriteLine("WCF SERVER: Done");
        }
    }

    [ServiceContract]
    public interface IUtilWcfService
    {
        [OperationContract]
        string Echo(string name);
        [OperationContract]
        void SendStream(Stream name);
    }
}