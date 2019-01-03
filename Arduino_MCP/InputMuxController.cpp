// 
// 
// 

#include "InputMuxController.h"

InputMuxController::InputMuxController()
{
}

InputMuxController::InputMuxController(uint8_t id, uint8_t pinS0, uint8_t pinS1, uint8_t pinS2, uint8_t pinZ, uint8_t firstInputPin)
{
	InputMuxController::_id = id;
	InputMuxController::_pinS0 = pinS0;
	InputMuxController::_pinS1 = pinS1;
	InputMuxController::_pinS2 = pinS2;
	InputMuxController::_pinZ = pinZ;
	InputMuxController::_firstInputPin = firstInputPin;

	pinMode(InputMuxController::_pinS0, OUTPUT);
	pinMode(InputMuxController::_pinS1, OUTPUT);
	pinMode(InputMuxController::_pinS2, OUTPUT);
}

void InputMuxController::readMux()
{
	for (size_t i = InputMuxController::_firstInputPin; i < InputMuxController::_firstInputPin + 8; i++) {
		digitalWrite(InputMuxController::_pinS0, HIGH && (i & B00000001));
		digitalWrite(InputMuxController::_pinS1, HIGH && (i & B00000010));
		digitalWrite(InputMuxController::_pinS2, HIGH && (i & B00000100));
		InputMuxController::_data[i % 8] = analogRead(InputMuxController::_pinZ) > 1020 ? 1 : 0;
	}
}

uint8_t InputMuxController::getId()
{
	return InputMuxController::_id;
}

uint8_t * InputMuxController::getData()
{
	return InputMuxController::_data;
}

InputMuxController::~InputMuxController()
{
}
