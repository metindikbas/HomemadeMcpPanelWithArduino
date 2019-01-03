// EncoderInput.h

#ifndef _ENCODERINPUT_h
#define _ENCODERINPUT_h

#if defined(ARDUINO) && ARDUINO >= 100
#include "arduino.h"
#else
#include "WProgram.h"
#endif

#include <RotaryEncoder.h>
#include "EnumEncoderChangeType.h"

class EncoderInput
{
  private:
	uint8_t _id;
	uint8_t _pin1;
	uint8_t _pin2;
	int _data;
	RotaryEncoder *_encoder;

	void update();

  public:
	EncoderInput();
	EncoderInput(uint8_t id, uint8_t pin1, uint8_t pin2);
	uint8_t updateData();
	uint8_t getId();
	~EncoderInput();
};

#endif
