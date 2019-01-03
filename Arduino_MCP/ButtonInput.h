// ButtonInput.h

#ifndef _BUTTONINPUT_h
#define _BUTTONINPUT_h

#if defined(ARDUINO) && ARDUINO >= 100
#include "arduino.h"
#else
#include "WProgram.h"
#endif

class ButtonInput
{
  private:
	uint8_t _id;
	uint8_t _pin;
	uint8_t _data;
	long _readTime = 0;
	const long _debounceInterval = 1000;

	void update();

  public:
	ButtonInput();
	ButtonInput(uint8_t id, uint8_t pin);
	bool updateData();
	uint8_t getData();
	uint8_t getId();
	~ButtonInput();
};

#endif
