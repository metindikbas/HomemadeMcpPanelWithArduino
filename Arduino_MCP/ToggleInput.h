// ToggleInput.h

#ifndef _TOGGLEINPUT_h
#define _TOGGLEINPUT_h

#if defined(ARDUINO) && ARDUINO >= 100
	#include "arduino.h"
#else
	#include "WProgram.h"
#endif

class ToggleInput
{
private:
	uint8_t _id;
	uint8_t _pin;
	uint8_t _data = HIGH;
public:
	ToggleInput();
	ToggleInput(uint8_t id, uint8_t pin);
	bool updateData();
	uint8_t getData();
	uint8_t getId();
	~ToggleInput();
};

#endif

