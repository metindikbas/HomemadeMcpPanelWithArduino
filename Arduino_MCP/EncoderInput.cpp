// 
// 
// 

#include "EncoderInput.h"

EncoderInput::EncoderInput()
{
}

EncoderInput::EncoderInput(uint8_t id, uint8_t pin1, uint8_t pin2)
{
	EncoderInput::_id = id;
	EncoderInput::_pin1 = pin1;
	EncoderInput::_pin2 = pin2;
	EncoderInput::_encoder = new RotaryEncoder(pin1, pin2);
}

uint8_t EncoderInput::updateData()
{
	EncoderInput::_encoder->tick();
	uint8_t returnData = 0;

	int newPosition = EncoderInput::_encoder->getPosition();
	if (newPosition == EncoderInput::_data) {
		returnData = ctNoChange;
	}
	else if (newPosition < EncoderInput::_data) {
		returnData = ctDecrease;
	}
	else if (newPosition > EncoderInput::_data) {
		returnData = ctIncrease;
	}
	EncoderInput::_data = newPosition;
	return returnData;
}

uint8_t EncoderInput::getId()
{
	return EncoderInput::_id;
}

EncoderInput::~EncoderInput()
{
}
