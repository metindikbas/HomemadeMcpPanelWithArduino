// 
// 
// 

#include "LedOutput.h"

LedOutput::LedOutput()
{
}

LedOutput::LedOutput(uint8_t id, uint8_t pin)
{
	LedOutput::_id = id;
	LedOutput::_pin = pin;
	// Set pin mode
	pinMode(LedOutput::_pin, OUTPUT);
}

void LedOutput::updateData(uint8_t data)
{
	if (LedOutput::_data == data) {
		return;
	}

	LedOutput::_data = data;

	// Update io object
	LedOutput::update();
}

uint8_t LedOutput::getData()
{
	return LedOutput::_data;
}

void LedOutput::update()
{
	digitalWrite(LedOutput::_pin, (LedOutput::_data == 1) ? (HIGH) : (LOW));
}

LedOutput::~LedOutput()
{
}
