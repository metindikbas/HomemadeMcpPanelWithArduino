using System;
using System.Linq;
using CommandMessenger;
using log4net;
using MDHomeCockpit.ListenerService.IoObjects.Base;
using MDHomeCockpit.ListenerService.IoObjects.Enums;
using MDHomeCockpit.ListenerService.Listener;

namespace MDHomeCockpit.ListenerService.IoObjects
{
    public class DisplayOutput : BaseIoObject
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private byte _id { get; set; }
        private EnumDisplayDataTypes _dataType { get; set; }
        private string _data { get; set; } = "-111";
        private int _length { get; set; }
        private bool _fillBlankSpace { get; set; }

        public DisplayOutput(CmdMessenger cmdMessenger, byte id, EnumDisplayDataTypes dataType, int length, bool fillBlankSpace) : base(cmdMessenger, EnumIoObjectType.DisplayOutput)
        {
            _id = id;
            _length = length;
            _fillBlankSpace = fillBlankSpace;
            _dataType = dataType;
        }

        public byte GetId()
        {
            return _id;
        }

        public void Update(int data)
        {
            string dataString = "";
            if (data == -9999)
            {
                for (int i = 0; i < _length; i++)
                {
                    dataString += " ";
                }
            }
            else
            {
                dataString = CalculateData(data);
            }

            if (_data == dataString)
            {
                return;
            }
            _data = dataString;
            // Prepare command
            var command = new SendCommand((byte)EnumOperationTypes.DisplayOutput);
            command.AddArgument(_id);
            command.AddArgument((byte)_dataType);
            command.AddArgument(dataString);
            var result = _cmdMessenger.SendCommand(command);
            Log.Info("Sending display update. Data Type: " + _dataType.ToString() + ", Value: " + _data + ", Result: " + result.Ok);
        }

        private string CalculateData(int rawData)
        {
            if (rawData == 0 && _fillBlankSpace)
            {
                switch (_length)
                {
                    case 3:
                        return "000";
                    case 5:
                        return " 0000"; // PMDG 737
                }
            }
            bool isNegative = rawData < 0;
            rawData = Math.Abs(rawData);
            string dataString = rawData.ToString();
            int length = _length;
            if (!isNegative)
            {
                while (dataString.Length < length)
                {
                    string temp = (_fillBlankSpace ? " " : "0") + dataString;
                    dataString = temp;
                }
            }

            if (isNegative)
            {
                dataString = "-" + dataString;
                while (dataString.Length < length)
                {
                    string temp = (_fillBlankSpace ? " " : "0") + dataString;
                    dataString = temp;
                }
            }

            return dataString;
        }
    }
}