/*
Name:		HomeCockpitV2.ino
Created:	8/25/2018 3:54:43 PM
Author:	MetinDikbas
*/

#include "EncoderInput.h"
#include "ToggleInput.h"
#include "ButtonInput.h"
#include "LedOutput.h"
#include <LedControl.h>
#include <EEPROM.h>
#include <CmdMessenger.h>

#include "Settings.h"
#include "MessageTypes.h"
#include "DisplayDataTypes.h"

CmdMessenger _cmdMessenger = CmdMessenger(Serial);

LedOutput ledOutputs[]
{
	LedOutput(0, 23), // EVT_MCP_FD_SWITCH_L
	LedOutput(1, 25), // EVT_MCP_AT_ARM_SWITCH
	LedOutput(2, 27), // EVT_MCP_N1_SWITCH
	LedOutput(3, 29), // EVT_MCP_SPEED_SWITCH
	LedOutput(4, 31), // EVT_MCP_LVL_CHG_SWITCH
	LedOutput(5, 33), // EVT_MCP_HDG_SEL_SWITCH
	LedOutput(6, 35), // EVT_MCP_APP_SWITCH
	LedOutput(7, 37), // EVT_MCP_VOR_LOC_SWITCH
	LedOutput(8, 39), // EVT_MCP_LNAV_SWITCH
	LedOutput(9, 41), // EVT_MCP_VNAV_SWITCH
	LedOutput(10, 53), // EVT_MCP_ALT_HOLD_SWITCH
	LedOutput(11, 45), // EVT_MCP_VS_SWITCH
	LedOutput(12, 46), // EVT_MCP_DISENGAGE_BAR
	LedOutput(13, 49), // EVT_MCP_CMD_B_SWITCH
	LedOutput(14, 51), // EVT_MCP_CMD_A_SWITCH
};

LedControl displayOutputs[]{
	LedControl(2, 3, 4, 2),
	LedControl(5, 6, 7, 2)
};

EncoderInput encoderInputs[]{
	EncoderInput(0, A2, A3),
	EncoderInput(1, A0, A1),
	EncoderInput(2, 9, 8),
	EncoderInput(3, 11, 10),
	EncoderInput(4, 13, 12)
};

ButtonInput buttonInputs[]{
	ButtonInput(0, 26), // EVT_MCP_N1_SWITCH
	ButtonInput(1, 28), // EVT_MCP_SPEED_SWITCH
	ButtonInput(2, 30), // EVT_MCP_LVL_CHG_SWITCH
	ButtonInput(3, 32), // EVT_MCP_HDG_SEL_SWITCH
	ButtonInput(4, 34), // EVT_MCP_APP_SWITCH
	ButtonInput(5, 36), // EVT_MCP_VOR_LOC_SWITCH
	ButtonInput(6, 38), // EVT_MCP_LNAV_SWITCH
	ButtonInput(7, 40), // EVT_MCP_VNAV_SWITCH
	ButtonInput(8, 52), // EVT_MCP_ALT_HOLD_SWITCH
	ButtonInput(9, 44), // EVT_MCP_VS_SWITCH
	ButtonInput(10, 48), // EVT_MCP_CMD_B_SWITCH
	ButtonInput(11, 50), // EVT_MCP_CMD_A_SWITCH
};

ToggleInput toggleInputs[]{
	ToggleInput(0, 22), // EVT_MCP_FD_SWITCH_L
	ToggleInput(1, 24), // EVT_MCP_AT_ARM_SWITCH
	ToggleInput(2, 47), // EVT_MCP_DISENGAGE_BAR
};

void setup() {
	Serial.begin(115200);

	// LEFT DISPLAYS
	displayOutputs[0].shutdown(0, false);
	displayOutputs[0].setIntensity(0, 10);
	displayOutputs[0].clearDisplay(0);
	displayOutputs[0].shutdown(1, false);
	displayOutputs[0].setIntensity(1, 10);
	displayOutputs[0].clearDisplay(1);

	// RIGHT DISPLAYS
	displayOutputs[1].shutdown(0, false);
	displayOutputs[1].setIntensity(0, 10);
	displayOutputs[1].clearDisplay(0);
	displayOutputs[1].shutdown(1, false);
	displayOutputs[1].setIntensity(1, 10);
	displayOutputs[1].clearDisplay(1);

	_cmdMessenger.printLfCr();

	attachCommandCallbacks();

	_cmdMessenger.sendCmd(opStatus, "Arduino has started!");
}

void loop() {
	_cmdMessenger.feedinSerialData();

	for (size_t i = 0; i < 12; i++)
	{
		if (buttonInputs[i].updateData()) {
			_cmdMessenger.sendCmdStart(opButtonInput);
			_cmdMessenger.sendCmdArg(buttonInputs[i].getId());
			_cmdMessenger.sendCmdEnd();
		}
	}

	for (size_t i = 0; i < 3; i++)
	{
		if (toggleInputs[i].updateData()) {
			_cmdMessenger.sendCmdStart(opToggleInput);
			_cmdMessenger.sendCmdArg(toggleInputs[i].getId());
			_cmdMessenger.sendCmdEnd();
		}
	}

	for (size_t i = 0; i < 5; i++)
	{
		uint8_t result = encoderInputs[i].updateData();
		if (result == 1 || result == 2) {
			_cmdMessenger.sendCmdStart(opEncoderInput);
			_cmdMessenger.sendCmdArg(encoderInputs[i].getId());
			_cmdMessenger.sendCmdArg(result);
			_cmdMessenger.sendCmdEnd();
		}
	}
}

void attachCommandCallbacks()
{
	// Attach callback methods
	_cmdMessenger.attach(OnUnknownCommandReceived);
	_cmdMessenger.attach(opLedOutput, OnLedOutputReceived);
	_cmdMessenger.attach(opDisplayOutput, OnDisplayOutputReceived);
}

void OnUnknownCommandReceived() {
	_cmdMessenger.sendCmdStart(opStatus);
	_cmdMessenger.sendCmdArg("Unknown command received!");
	_cmdMessenger.sendCmdEnd();
}

void OnLedOutputReceived() {
	int id = _cmdMessenger.readInt32Arg();
	uint8_t data = _cmdMessenger.readInt16Arg();
	// Update data
	ledOutputs[id].updateData(data);
}

void OnDisplayOutputReceived() {
	uint8_t id = _cmdMessenger.readInt16Arg();
	uint8_t displayDataType = _cmdMessenger.readInt16Arg();
	String data = _cmdMessenger.readStringArg();

	if (displayDataType == dtCourse) {
		displayOutputs[id].setChar(0, 7, data.charAt(0), false);
		displayOutputs[id].setChar(0, 6, data.charAt(1), false);
		displayOutputs[id].setChar(0, 5, data.charAt(2), false);
	}
	else if (displayDataType == dtIas) {
		displayOutputs[id].setChar(0, 2, data.charAt(0), false);
		displayOutputs[id].setChar(0, 1, data.charAt(1), false);
		displayOutputs[id].setChar(0, 0, data.charAt(2), false);
	}
	else if (displayDataType == dtHeading) {
		displayOutputs[id].setChar(1, 2, data.charAt(0), false);
		displayOutputs[id].setChar(1, 1, data.charAt(1), false);
		displayOutputs[id].setChar(1, 0, data.charAt(2), false);
	}
	else if (displayDataType == dtAltitude) {
		displayOutputs[id].setChar(0, 4, data.charAt(0), false);
		displayOutputs[id].setChar(0, 3, data.charAt(1), false);
		displayOutputs[id].setChar(0, 2, data.charAt(2), false);
		displayOutputs[id].setChar(0, 1, data.charAt(3), false);
		displayOutputs[id].setChar(0, 0, data.charAt(4), false);
	}
	else if (displayDataType == dtVertSpeed) {
		displayOutputs[id].setChar(1, 4, data.charAt(0), false);
		displayOutputs[id].setChar(1, 3, data.charAt(1), false);
		displayOutputs[id].setChar(1, 2, data.charAt(2), false);
		displayOutputs[id].setChar(1, 1, data.charAt(3), false);
		displayOutputs[id].setChar(1, 0, data.charAt(4), false);
	}
}
