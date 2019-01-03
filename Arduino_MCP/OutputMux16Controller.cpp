#include "OutputMux16Controller.h"

OutputMux16Controller::OutputMux16Controller()
{
}

OutputMux16Controller::OutputMux16Controller(uint8_t id, uint8_t pinST, uint8_t pinSH, uint8_t pinDS)
{
	OutputMux16Controller::_id = id;
	OutputMux16Controller::_pinST = pinST;
	OutputMux16Controller::_pinSH = pinSH;
	OutputMux16Controller::_pinDS = pinDS;

	pinMode(OutputMux16Controller::_pinST, OUTPUT);
	pinMode(OutputMux16Controller::_pinSH, OUTPUT);
	pinMode(OutputMux16Controller::_pinDS, OUTPUT);
}

void OutputMux16Controller::updateMux(uint8_t index, uint8_t value)
{
	if (OutputMux16Controller::_data[index] != value) {
		OutputMux16Controller::_data[index] = value;

		for (size_t i = 0; i < 16; i++)
		{
			digitalWrite(OutputMux16Controller::_pinST, LOW);
			shiftOut(OutputMux16Controller::_pinDS, OutputMux16Controller::_pinSH, MSBFIRST, 255);
			digitalWrite(OutputMux16Controller::_pinST, HIGH);
		}
	}
}

OutputMux16Controller::~OutputMux16Controller()
{
}
