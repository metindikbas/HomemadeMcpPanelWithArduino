using System;
using System.Threading.Tasks;
using log4net;
using MDHomeCockpit.ListenerService.Listener;

namespace MDHomeCockpit.ListenerService
{
    class Program
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static ListenerServer _listenerServer;

        static void Main(string[] args)
        {
            Log.Info("Application is running.");
            _listenerServer = new ListenerServer("COM6", 115200, TimeSpan.FromMilliseconds(100));
            Task.Factory.StartNew(() => { _listenerServer.Run(); });

            Console.Read();
            Log.Info("Application is closed.");
        }
    }
}
