// OutputMuxController.h

#ifndef _OUTPUTMUX16CONTROLLER_h
#define _OUTPUTMUX16CONTROLLER_h

#if defined(ARDUINO) && ARDUINO >= 100
	#include "arduino.h"
#else
	#include "WProgram.h"
#endif

class OutputMux16Controller {
private:
	uint8_t _id;
	uint8_t _pinST;
	uint8_t _pinSH;
	uint8_t _pinDS;
	uint8_t _data[16];

public:
	OutputMux16Controller();
	OutputMux16Controller(uint8_t id, uint8_t pinST, uint8_t pinSH, uint8_t pinDS);
	void updateMux(uint8_t index, uint8_t value);
	~OutputMux16Controller();
};

#endif

