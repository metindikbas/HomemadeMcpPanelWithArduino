// 
// 
// 

#include "ToggleInput.h"

ToggleInput::ToggleInput()
{
}

ToggleInput::ToggleInput(uint8_t id, uint8_t pin)
{
	ToggleInput::_id = id;
	ToggleInput::_pin = pin;
	pinMode(ToggleInput::_pin, INPUT_PULLUP);
	digitalWrite(ToggleInput::_pin, HIGH);
}

bool ToggleInput::updateData()
{
	int buttonState = digitalRead(ToggleInput::_pin);
	if ((buttonState != ToggleInput::_data)) {
		ToggleInput::_data = buttonState;
		return true;
	}
	return false;
}

uint8_t ToggleInput::getData()
{
	return ToggleInput::_data;
}

uint8_t ToggleInput::getId()
{
	return ToggleInput::_id;
}

ToggleInput::~ToggleInput()
{
}
