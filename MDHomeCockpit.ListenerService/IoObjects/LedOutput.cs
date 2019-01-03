using CommandMessenger;
using MDHomeCockpit.ListenerService.Enums;
using MDHomeCockpit.ListenerService.IoObjects.Base;

namespace MDHomeCockpit.ListenerService.IoObjects
{
    public class LedOutput : BaseIoObject
    {
        private int _id { get; set; }
        private byte _data { get; set; }

        public LedOutput(CmdMessenger cmdMessenger, int id) : base(cmdMessenger)
        {
            _id = id;
        }

        public int GetId()
        {
            return _id;
        }

        public void Update(int data)
        {
            if (_data == data)
            {
                return;
            }
            _data = (byte)data;
            // Prepare command
            var command = new SendCommand((int)EnumOperationTypes.LedOutput);
            command.AddArgument(_id);
            command.AddArgument(_data);
            ReceivedCommand resultCommand = _cmdMessenger.SendCommand(command);
            if (!resultCommand.Ok)
            {
                // Log
            }
        }
    }
}