using FSUIPC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace XplaneConnectionTest
{
    class Program
    {
        static void Main(string[] args)
        {
            FSUIPCConnection.Open();

            Offset<long> playerLatitude = new Offset<long>(0x0560);

            Offset<long> playerLongitude = new Offset<long>(0x0568);

            Offset<int> autoThrottle = new Offset<int>(0x0810);

            Offset<int> flightDirector = new Offset<int>(0x2EE0);

            while (true)
            {
                FSUIPCConnection.Process();
                FsLongitude lon = new FsLongitude(playerLongitude.Value);
                FsLatitude lat = new FsLatitude(playerLatitude.Value);

                var atSwitch = autoThrottle.Value;

                var fdSwitch = flightDirector.Value;

                Thread.Sleep(5000);
            }
        }
    }
}
