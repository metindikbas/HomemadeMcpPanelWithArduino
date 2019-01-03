using CommandMessenger;
using MDHomeCockpit.ListenerService.Enums;
using MDHomeCockpit.ListenerService.IoObjects.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDHomeCockpit.ListenerService.IoObjects
{
    public class Mux16LedOutput : BaseIoObject
    {
        private int _id { get; set; }
        private byte[] _data { get; set; }

        public Mux16LedOutput(CmdMessenger cmdMessenger, int id) : base(cmdMessenger)
        {
            _id = id;
            _data = new byte[16];
        }

        public int GetId()
        {
            return _id;
        }

        public void Update(int index, int data)
        {
            if (_data[index] == data)
            {
                return;
            }
            _data[index] = (byte)data;
            // Prepare command
            var command = new SendCommand((int)EnumOperationTypes.MuxLedOutput);
            command.AddArgument(_id);
            command.AddArgument(index);
            command.AddArgument(_data[index]);
            ReceivedCommand resultCommand = _cmdMessenger.SendCommand(command);
            if (!resultCommand.Ok)
            {
                // Log
            }
        }
    }
}
