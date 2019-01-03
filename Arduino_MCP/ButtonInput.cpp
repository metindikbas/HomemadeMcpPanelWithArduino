//
//
//

#include "ButtonInput.h"

ButtonInput::ButtonInput()
{
}

ButtonInput::ButtonInput(uint8_t id, uint8_t pin)
{
	ButtonInput::_id = id;
	ButtonInput::_pin = pin;
	pinMode(ButtonInput::_pin, INPUT_PULLUP);
	digitalWrite(ButtonInput::_pin, HIGH);
}

bool ButtonInput::updateData()
{
	if ((millis() - ButtonInput::_readTime) > _debounceInterval)
	{
		int buttonState = digitalRead(ButtonInput::_pin);
		if ((buttonState == LOW))
		{
			ButtonInput::_readTime = millis();
			return true;
		}
	}
	return false;
}

uint8_t ButtonInput::getData()
{
	return ButtonInput::_data;
}

uint8_t ButtonInput::getId()
{
	return ButtonInput::_id;
}

ButtonInput::~ButtonInput()
{
}
