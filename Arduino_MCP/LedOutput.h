// LedObject.h

#ifndef _LEDOUTPUT_h
#define _LEDOUTPUT_h

#if defined(ARDUINO) && ARDUINO >= 100
	#include "arduino.h"
#else
	#include "WProgram.h"
#endif

#include "Settings.h"

class LedOutput {
private:
	uint8_t _id;
	uint8_t _pin;
	uint8_t _data;

	void update();
public:
	LedOutput();
	LedOutput(uint8_t id, uint8_t pin);
	void updateData(uint8_t data);
	uint8_t getData();
	~LedOutput();
};

#endif

