using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AduinoConnect.Pmdg;
using CommandMessenger;
using CommandMessenger.Queue;
using CommandMessenger.Transport.Serial;
using FSUIPC;
using log4net;
using MDHomeCockpit.ListenerService.Enums;
using MDHomeCockpit.ListenerService.FsuipcData.Pmdg737;
using MDHomeCockpit.ListenerService.IoObjects;

namespace MDHomeCockpit.ListenerService.Listener
{
    public class ListenerServer
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly TimeSpan _simDataCollectionPeriod;
        private readonly Timer _fsuipcConnectTimer;
        private readonly Timer _simDataCollectorTimer;
        private readonly SerialTransport _serialTransport;
        private readonly CmdMessenger _cmdMessenger;

        private readonly LedOutput[] _ledOutputs;
        private readonly DisplayOutput[] _displayOutputs;
        // EVT_MCP_FD_SWITCH_L
        // EVT_MCP_AT_ARM_SWITCH
        // EVT_MCP_N1_SWITCH
        // EVT_MCP_SPEED_SWITCH
        // EVT_MCP_LVL_CHG_SWITCH
        // EVT_MCP_HDG_SEL_SWITCH
        // EVT_MCP_APP_SWITCH"
        // EVT_MCP_VOR_LOC_SWITCH
        // EVT_MCP_LNAV_SWITCH
        // EVT_MCP_VNAV_SWITCH
        // EVT_MCP_ALT_HOLD_SWITCH
        // EVT_MCP_VS_SWITCH
        // EVT_MCP_DISENGAGE_BAR
        // EVT_MCP_CMD_B_SWITCH
        // EVT_MCP_CMD_A_SWITCH
        private readonly Mux16LedOutput mux16LedOutput1;

        private bool _isFsuipcConnected;
        private bool _isArduinoConnected;

        private Offset<uint> controlParam = new Offset<uint>("sendControl", 0x3114, true);
        private Offset<uint> sendControl = new Offset<uint>("sendControl", 0x3110, true);

        public ListenerServer(string portName, int baudRate, TimeSpan dataRefreshInterval)
        {
            String operationMode = ConfigurationManager.AppSettings["OperationMode"];

            _simDataCollectionPeriod = dataRefreshInterval;
            _fsuipcConnectTimer = new Timer(FsuipcConnectTimerCallback, null, -1, -1);
            _simDataCollectorTimer = new Timer(SimDataCollectorCallback, null, -1, -1);

            _ledOutputs = new LedOutput[15];
            
            _serialTransport = new SerialTransport
            {
                CurrentSerialSettings = { PortName = portName, BaudRate = baudRate, DtrEnable = false }
            };

            _cmdMessenger = new CmdMessenger(_serialTransport, BoardType.Bit16);

            _cmdMessenger.NewLineReceived += _cmdMessenger_NewLineReceived;

            AttachCommandCallBacks();

            while (!_isArduinoConnected)
            {
                _isArduinoConnected = _cmdMessenger.Connect();
                Thread.Sleep(1000);
            }

            // Populate io objects
            // LEDS
            _ledOutputs = new LedOutput[]
            {
                new LedOutput(_cmdMessenger, 0),    // EVT_MCP_FD_SWITCH_L
                new LedOutput(_cmdMessenger, 1),    // EVT_MCP_AT_ARM_SWITCH
                new LedOutput(_cmdMessenger, 2),    // EVT_MCP_N1_SWITCH
                new LedOutput(_cmdMessenger, 3),    // EVT_MCP_SPEED_SWITCH
                new LedOutput(_cmdMessenger, 4),    // EVT_MCP_LVL_CHG_SWITCH
                new LedOutput(_cmdMessenger, 5),    // EVT_MCP_HDG_SEL_SWITCH
                new LedOutput(_cmdMessenger, 6),    // EVT_MCP_APP_SWITCH
                new LedOutput(_cmdMessenger, 7),    // EVT_MCP_VOR_LOC_SWITCH
                new LedOutput(_cmdMessenger, 8),    // EVT_MCP_LNAV_SWITCH
                new LedOutput(_cmdMessenger, 9),    // EVT_MCP_VNAV_SWITCH
                new LedOutput(_cmdMessenger, 10),    // EVT_MCP_ALT_HOLD_SWITCH
                new LedOutput(_cmdMessenger, 11),    // EVT_MCP_VS_SWITCH
                new LedOutput(_cmdMessenger, 12),    // EVT_MCP_DISENGAGE_BAR
                new LedOutput(_cmdMessenger, 13),    // EVT_MCP_CMD_B_SWITCH
                new LedOutput(_cmdMessenger, 14),    // EVT_MCP_CMD_A_SWITCH
            };
            // 7 SEGMENT DISPLAYS
            _displayOutputs = new DisplayOutput[]
            {
                new DisplayOutput(_cmdMessenger, 0, EnumDisplayDataTypes.Course, 3, false), // Course
                new DisplayOutput(_cmdMessenger, 0, EnumDisplayDataTypes.Ias, 3, false), // Speed
                new DisplayOutput(_cmdMessenger, 0, EnumDisplayDataTypes.Heading, 3, false), // Heading
                new DisplayOutput(_cmdMessenger, 1, EnumDisplayDataTypes.Altitude, 5, false), // Altitude
                new DisplayOutput(_cmdMessenger, 1, EnumDisplayDataTypes.VertSpeed, 5, true), // VertSpeed
            };

            mux16LedOutput1 = new Mux16LedOutput(_cmdMessenger, 0);

            Log.Info("Cmd messenger successfully connected.");
        }

        private void _cmdMessenger_NewLineReceived(object sender, CommandEventArgs e)
        {
            Log.Info("new line received -> " + e.Command.CommandString());
        }

        public void Run()
        {
            _fsuipcConnectTimer.Change(0, (int)TimeSpan.FromSeconds(5).TotalMilliseconds);
            _simDataCollectorTimer.Change(0, 100);
        }

        private void SimDataCollectorCallback(object state)
        {
            try
            {
                if (!_isFsuipcConnected)
                {
                    return;
                }

                // Fetch new data
                FSUIPCConnection.Process("MCP");

                // Update
                // LEDS
                mux16LedOutput1.Update(0, Pmdg737Offsets.MCP_FDSw.Value);
                mux16LedOutput1.Update(1, Pmdg737Offsets.MCP_ATArmSw.Value);
                mux16LedOutput1.Update(2, Pmdg737Offsets.MCP_annunN1.Value);
                mux16LedOutput1.Update(3, Pmdg737Offsets.MCP_annunSPEED.Value);
                mux16LedOutput1.Update(4, Pmdg737Offsets.MCP_annunLVL_CHG.Value);
                mux16LedOutput1.Update(5, Pmdg737Offsets.MCP_annunHDG_SEL.Value);
                mux16LedOutput1.Update(6, Pmdg737Offsets.MCP_annunAPP.Value);
                mux16LedOutput1.Update(7, Pmdg737Offsets.MCP_annunVOR_LOC.Value);
                mux16LedOutput1.Update(8, Pmdg737Offsets.MCP_annunLNAV.Value);
                mux16LedOutput1.Update(9, Pmdg737Offsets.MCP_annunVNAV.Value);
                mux16LedOutput1.Update(10, Pmdg737Offsets.MCP_annunALT_HOLD.Value);
                mux16LedOutput1.Update(11, Pmdg737Offsets.MCP_annunVS.Value);
                mux16LedOutput1.Update(12, Pmdg737Offsets.MCP_DisengageBar.Value);
                mux16LedOutput1.Update(13, Pmdg737Offsets.MCP_annunCMD_B.Value);
                mux16LedOutput1.Update(14, Pmdg737Offsets.MCP_annunCMD_A.Value);

                // _ledOutputs[0].Update(Pmdg737Offsets.MCP_FDSw.Value);
                // _ledOutputs[1].Update(Pmdg737Offsets.MCP_ATArmSw.Value);
                // _ledOutputs[2].Update(Pmdg737Offsets.MCP_annunN1.Value);
                // _ledOutputs[3].Update(Pmdg737Offsets.MCP_annunSPEED.Value);
                // _ledOutputs[4].Update(Pmdg737Offsets.MCP_annunLVL_CHG.Value);
                // _ledOutputs[5].Update(Pmdg737Offsets.MCP_annunHDG_SEL.Value);
                // _ledOutputs[6].Update(Pmdg737Offsets.MCP_annunAPP.Value);
                // _ledOutputs[7].Update(Pmdg737Offsets.MCP_annunVOR_LOC.Value);
                // _ledOutputs[8].Update(Pmdg737Offsets.MCP_annunLNAV.Value);
                // _ledOutputs[9].Update(Pmdg737Offsets.MCP_annunVNAV.Value);
                // _ledOutputs[10].Update(Pmdg737Offsets.MCP_annunALT_HOLD.Value);
                // _ledOutputs[11].Update(Pmdg737Offsets.MCP_annunVS.Value);
                // _ledOutputs[12].Update(Pmdg737Offsets.MCP_DisengageBar.Value);
                // _ledOutputs[13].Update(Pmdg737Offsets.MCP_annunCMD_B.Value);
                // _ledOutputs[14].Update(Pmdg737Offsets.MCP_annunCMD_A.Value);

                // 7 SEGMENT DISPLAYS
                _displayOutputs[0].Update(Pmdg737Offsets.MCP_Course.Value);
                if (Pmdg737Offsets.MCP_IASBlank.Value != 1)
                {
                    _displayOutputs[1].Update((int)Pmdg737Offsets.MCP_IASMach.Value);
                }
                else
                {
                    _displayOutputs[1].Update(-9999);
                }
                _displayOutputs[2].Update(Pmdg737Offsets.MCP_Heading.Value);
                _displayOutputs[3].Update(Pmdg737Offsets.MCP_Altitude.Value);
                if (Pmdg737Offsets.MCP_VertSpeedBlank.Value != 1)
                {
                    _displayOutputs[4].Update(Pmdg737Offsets.MCP_VertSpeed.Value);
                }
                else
                {
                    _displayOutputs[4].Update(-9999);
                }

                // Update
                //Log.Info(" DATA -> " + "MCP_HEADING: " + Pmdg737Offsets.MCP_Heading.Value);
            }
            catch (Exception ex)
            {
                Log.Error("Could not fetch new data from simulator!", ex);
            }
        }

        private void FsuipcConnectTimerCallback(object state)
        {
            if (!_isFsuipcConnected)
            {
                try
                {
                    FSUIPCConnection.Open();
                    _isFsuipcConnected = true;
                    Log.Info("FSUIPC connection is opened.");
                }
                catch (Exception ex)
                {
                    _isFsuipcConnected = false;
                    Log.Error("FSUIPC connection could not be made!", ex);
                }
            }
        }

        private void AttachCommandCallBacks()
        {
            _cmdMessenger.Attach((int)EnumOperationTypes.ButtonInput, ButtonInputReceived);
            _cmdMessenger.Attach((int)EnumOperationTypes.EncoderInput, EncoderInputReceived);
            _cmdMessenger.Attach((int)EnumOperationTypes.ToggleInput, ToggleInputReceived);
            _cmdMessenger.Attach((int)EnumOperationTypes.Status, StatusReceived);
            _cmdMessenger.Attach((int)EnumOperationTypes.MuxToggleInput, MuxToggleInputReceived);
        }

        private void StatusReceived(ReceivedCommand receivedcommand)
        {
            Log.Info(receivedcommand.ReadStringArg());
        }

        private void ToggleInputReceived(ReceivedCommand receivedcommand)
        {
            short id = receivedcommand.ReadInt16Arg();
            switch (id)
            {
                case 0:
                    SendControlToFS(Pmdg737Events.EVT_MCP_FD_SWITCH_L, PmdgParameters.MOUSE_FLAG_LEFTSINGLE);
                    SendControlToFS(Pmdg737Events.EVT_MCP_FD_SWITCH_R, PmdgParameters.MOUSE_FLAG_LEFTSINGLE);
                    break;
                case 1:
                    SendControlToFS(Pmdg737Events.EVT_MCP_AT_ARM_SWITCH, PmdgParameters.MOUSE_FLAG_LEFTSINGLE);
                    break;
                case 2:
                    SendControlToFS(Pmdg737Events.EVT_MCP_DISENGAGE_BAR, PmdgParameters.MOUSE_FLAG_LEFTSINGLE);
                    break;
            }
        }

        private void MuxToggleInputReceived(ReceivedCommand receivedcommand)
        {
            short id = receivedcommand.ReadInt16Arg();
            short value = receivedcommand.ReadInt16Arg();
            switch (id)
            {
                case 0:
                    if (value == 1)
                    {
                        SendControlToFS(Pmdg737Events.EVT_OH_LIGHTS_IGN_SEL, PmdgParameters.MOUSE_FLAG_LEFTSINGLE);
                    }
                    else
                    {
                        SendControlToFS(Pmdg737Events.EVT_OH_LIGHTS_IGN_SEL, PmdgParameters.MOUSE_FLAG_RIGHTSINGLE);
                    }
                    break;
                case 1:
                    if (value == 0)
                    {
                        SendControlToFS(Pmdg737Events.EVT_OH_LIGHTS_LOGO, PmdgParameters.MOUSE_FLAG_LEFTSINGLE);
                    }
                    else
                    {
                        SendControlToFS(Pmdg737Events.EVT_OH_LIGHTS_LOGO, PmdgParameters.MOUSE_FLAG_RIGHTSINGLE);
                    }
                    break;
                case 2:
                    if (value == 0)
                    {
                        SendControlToFS(Pmdg737Events.EVT_OH_LIGHTS_POS_STROBE, PmdgParameters.MOUSE_FLAG_LEFTSINGLE);
                        SendControlToFS(Pmdg737Events.EVT_OH_LIGHTS_POS_STROBE, PmdgParameters.MOUSE_FLAG_LEFTSINGLE);
                    }
                    else
                    {
                        SendControlToFS(Pmdg737Events.EVT_OH_LIGHTS_POS_STROBE, PmdgParameters.MOUSE_FLAG_LEFTSINGLE);
                        SendControlToFS(Pmdg737Events.EVT_OH_LIGHTS_POS_STROBE, PmdgParameters.MOUSE_FLAG_LEFTSINGLE);
                        SendControlToFS(Pmdg737Events.EVT_OH_LIGHTS_POS_STROBE, PmdgParameters.MOUSE_FLAG_RIGHTSINGLE);
                    }
                    break;
                case 3:
                    if (value == 1)
                    {
                        SendControlToFS(Pmdg737Events.EVT_OH_LIGHTS_IGN_SEL, PmdgParameters.MOUSE_FLAG_RIGHTSINGLE);
                    }
                    else
                    {
                        SendControlToFS(Pmdg737Events.EVT_OH_LIGHTS_IGN_SEL, PmdgParameters.MOUSE_FLAG_LEFTSINGLE);
                    }
                    break;
                case 4:
                    if (value == 0)
                    {
                        SendControlToFS(Pmdg737Events.EVT_OH_LIGHTS_WHEEL_WELL, PmdgParameters.MOUSE_FLAG_LEFTSINGLE);
                    }
                    else
                    {
                        SendControlToFS(Pmdg737Events.EVT_OH_LIGHTS_WHEEL_WELL, PmdgParameters.MOUSE_FLAG_RIGHTSINGLE);
                    }
                    break;
                case 5:
                    if (value == 0)
                    {
                        SendControlToFS(Pmdg737Events.EVT_OH_LIGHTS_POS_STROBE, PmdgParameters.MOUSE_FLAG_RIGHTSINGLE);
                        SendControlToFS(Pmdg737Events.EVT_OH_LIGHTS_POS_STROBE, PmdgParameters.MOUSE_FLAG_RIGHTSINGLE);
                    }
                    else
                    {
                        SendControlToFS(Pmdg737Events.EVT_OH_LIGHTS_POS_STROBE, PmdgParameters.MOUSE_FLAG_RIGHTSINGLE);
                        SendControlToFS(Pmdg737Events.EVT_OH_LIGHTS_POS_STROBE, PmdgParameters.MOUSE_FLAG_RIGHTSINGLE);
                        SendControlToFS(Pmdg737Events.EVT_OH_LIGHTS_POS_STROBE, PmdgParameters.MOUSE_FLAG_LEFTSINGLE);
                    }
                    break;
                case 6:
                    if (value == 0)
                    {
                        SendControlToFS(Pmdg737Events.EVT_OH_LIGHTS_WING, PmdgParameters.MOUSE_FLAG_LEFTSINGLE);
                    }
                    else
                    {
                        SendControlToFS(Pmdg737Events.EVT_OH_LIGHTS_WING, PmdgParameters.MOUSE_FLAG_RIGHTSINGLE);
                    }
                    break;
                case 7:
                    if (value == 0)
                    {
                        SendControlToFS(Pmdg737Events.EVT_OH_LIGHTS_ANT_COL, PmdgParameters.MOUSE_FLAG_LEFTSINGLE);
                    }
                    else
                    {
                        SendControlToFS(Pmdg737Events.EVT_OH_LIGHTS_ANT_COL, PmdgParameters.MOUSE_FLAG_RIGHTSINGLE);
                    }
                    break;
                case 8:
                    if (value == 0)
                    {
                        SendControlToFS(Pmdg737Events.EVT_OH_LIGHTS_L_FIXED, PmdgParameters.MOUSE_FLAG_LEFTSINGLE);
                    }
                    else
                    {
                        SendControlToFS(Pmdg737Events.EVT_OH_LIGHTS_L_FIXED, PmdgParameters.MOUSE_FLAG_RIGHTSINGLE);
                    }
                    break;
                case 9:
                    if (value == 0)
                    {
                        SendControlToFS(Pmdg737Events.EVT_OH_LIGHTS_R_FIXED, PmdgParameters.MOUSE_FLAG_LEFTSINGLE);
                    }
                    else
                    {
                        SendControlToFS(Pmdg737Events.EVT_OH_LIGHTS_R_FIXED, PmdgParameters.MOUSE_FLAG_RIGHTSINGLE);
                    }
                    break;
                case 10:
                    if (value == 0)
                    {
                        SendControlToFS(Pmdg737Events.EVT_OH_LIGHTS_L_TURNOFF, PmdgParameters.MOUSE_FLAG_LEFTSINGLE);
                    }
                    else
                    {
                        SendControlToFS(Pmdg737Events.EVT_OH_LIGHTS_L_TURNOFF, PmdgParameters.MOUSE_FLAG_RIGHTSINGLE);
                    }
                    break;
                case 11:
                    if (value == 0)
                    {
                        SendControlToFS(Pmdg737Events.EVT_OH_LIGHTS_R_RETRACT, PmdgParameters.MOUSE_FLAG_LEFTSINGLE);
                        SendControlToFS(Pmdg737Events.EVT_OH_LIGHTS_L_RETRACT, PmdgParameters.MOUSE_FLAG_LEFTSINGLE);
                    }
                    else
                    {
                        SendControlToFS(Pmdg737Events.EVT_OH_LIGHTS_R_RETRACT, PmdgParameters.MOUSE_FLAG_RIGHTSINGLE);
                        SendControlToFS(Pmdg737Events.EVT_OH_LIGHTS_L_RETRACT, PmdgParameters.MOUSE_FLAG_RIGHTSINGLE);
                    }
                    break;
                case 12:
                    if (value == 1)
                    {
                        SendControlToFS(Pmdg737Events.EVT_OH_LIGHTS_APU_START, PmdgParameters.MOUSE_FLAG_RIGHTSINGLE);
                        SendControlToFS(Pmdg737Events.EVT_OH_LIGHTS_APU_START, PmdgParameters.MOUSE_FLAG_RIGHTSINGLE);
                    }
                    else
                    {
                        SendControlToFS(Pmdg737Events.EVT_OH_LIGHTS_APU_START, PmdgParameters.MOUSE_FLAG_LEFTSINGLE);
                    }
                    break;
                case 13:
                    if (value == 0)
                    {
                        SendControlToFS(Pmdg737Events.EVT_OH_LIGHTS_R_TURNOFF, PmdgParameters.MOUSE_FLAG_LEFTSINGLE);
                    }
                    else
                    {
                        SendControlToFS(Pmdg737Events.EVT_OH_LIGHTS_R_TURNOFF, PmdgParameters.MOUSE_FLAG_RIGHTSINGLE);
                    }
                    break;
                case 14:
                    SendControlToFS(Pmdg737Events.EVT_OH_LIGHTS_APU_START, PmdgParameters.MOUSE_FLAG_LEFTSINGLE);
                    SendControlToFS(Pmdg737Events.EVT_OH_LIGHTS_APU_START, PmdgParameters.MOUSE_FLAG_LEFTSINGLE);
                    break;
                case 15:
                    if (value == 0)
                    {
                        SendControlToFS(Pmdg737Events.EVT_OH_LIGHTS_TAXI, PmdgParameters.MOUSE_FLAG_LEFTSINGLE);
                    }
                    else
                    {
                        SendControlToFS(Pmdg737Events.EVT_OH_LIGHTS_TAXI, PmdgParameters.MOUSE_FLAG_RIGHTSINGLE);
                    }
                    break;
            }
        }

        private void EncoderInputReceived(ReceivedCommand receivedcommand)
        {
            short id = receivedcommand.ReadInt16Arg();
            short changeType = receivedcommand.ReadInt16Arg();
            uint parameter = 0;
            if (changeType == 1)
            {
                // Decrease
                parameter = PmdgParameters.MOUSE_FLAG_WHEEL_DOWN;
            }
            else if (changeType == 2)
            {
                parameter = PmdgParameters.MOUSE_FLAG_WHEEL_UP;
            }
            switch (id)
            {
                case 0:
                    SendControlToFS(Pmdg737Events.EVT_MCP_COURSE_SELECTOR_L, parameter);
                    break;
                case 1:
                    SendControlToFS(Pmdg737Events.EVT_MCP_SPEED_SELECTOR, parameter);
                    break;
                case 2:
                    SendControlToFS(Pmdg737Events.EVT_MCP_HEADING_SELECTOR, parameter);
                    break;
                case 3:
                    SendControlToFS(Pmdg737Events.EVT_MCP_ALTITUDE_SELECTOR, parameter);
                    break;
                case 4:
                    SendControlToFS(Pmdg737Events.EVT_MCP_VS_SELECTOR, parameter);
                    break;
            }
        }

        private void ButtonInputReceived(ReceivedCommand receivedcommand)
        {
            short id = receivedcommand.ReadInt16Arg();
            switch (id)
            {
                case 0:
                    SendControlToFS(Pmdg737Events.EVT_MCP_N1_SWITCH, PmdgParameters.MOUSE_FLAG_LEFTSINGLE);
                    break;
                case 1:
                    SendControlToFS(Pmdg737Events.EVT_MCP_SPEED_SWITCH, PmdgParameters.MOUSE_FLAG_LEFTSINGLE);
                    break;
                case 2:
                    SendControlToFS(Pmdg737Events.EVT_MCP_LVL_CHG_SWITCH, PmdgParameters.MOUSE_FLAG_LEFTSINGLE);
                    break;
                case 3:
                    SendControlToFS(Pmdg737Events.EVT_MCP_HDG_SEL_SWITCH, PmdgParameters.MOUSE_FLAG_LEFTSINGLE);
                    break;
                case 4:
                    SendControlToFS(Pmdg737Events.EVT_MCP_APP_SWITCH, PmdgParameters.MOUSE_FLAG_LEFTSINGLE);
                    break;
                case 5:
                    SendControlToFS(Pmdg737Events.EVT_MCP_VOR_LOC_SWITCH, PmdgParameters.MOUSE_FLAG_LEFTSINGLE);
                    break;
                case 6:
                    SendControlToFS(Pmdg737Events.EVT_MCP_LNAV_SWITCH, PmdgParameters.MOUSE_FLAG_LEFTSINGLE);
                    break;
                case 7:
                    SendControlToFS(Pmdg737Events.EVT_MCP_VNAV_SWITCH, PmdgParameters.MOUSE_FLAG_LEFTSINGLE);
                    break;
                case 8:
                    SendControlToFS(Pmdg737Events.EVT_MCP_ALT_HOLD_SWITCH, PmdgParameters.MOUSE_FLAG_LEFTSINGLE);
                    break;
                case 9:
                    SendControlToFS(Pmdg737Events.EVT_MCP_VS_SWITCH, PmdgParameters.MOUSE_FLAG_LEFTSINGLE);
                    break;
                case 10:
                    SendControlToFS(Pmdg737Events.EVT_MCP_CMD_B_SWITCH, PmdgParameters.MOUSE_FLAG_LEFTSINGLE);
                    break;
                case 11:
                    SendControlToFS(Pmdg737Events.EVT_MCP_CMD_A_SWITCH, PmdgParameters.MOUSE_FLAG_LEFTSINGLE);
                    break;
            }
        }

        private void SendControlToFS(uint controlNumber, uint parameterValue)
        {
            if (!_isArduinoConnected)
            {
                return;
            }
            sendControl.Value = controlNumber;
            controlParam.Value = parameterValue;
            FSUIPCConnection.Process("sendControl");
        }
    }
}
