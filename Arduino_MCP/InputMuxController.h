// MuxController.h

#ifndef _MUXCONTROLLER_h
#define _MUXCONTROLLER_h

#if defined(ARDUINO) && ARDUINO >= 100
	#include "arduino.h"
#else
	#include "WProgram.h"
#endif

class InputMuxController {
private:
	uint8_t _id;
	uint8_t _pinS0;
	uint8_t _pinS1;
	uint8_t _pinS2;
	uint8_t _pinZ;
	uint8_t _data[8];
	uint8_t _firstInputPin;

public:
	InputMuxController();
	InputMuxController(uint8_t id, uint8_t pinS0, uint8_t pinS1, uint8_t pinS2, uint8_t pinZ, uint8_t firstInputPin);
	void readMux();
	uint8_t getId();
	uint8_t *getData();
	~InputMuxController();
};

#endif

