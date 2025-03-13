#ifndef RELAY_HANDLER_H
#define RELAY_HANDLER_H

#include <Arduino.h>

class RelayHandler {
public:
    static void init();
    static void turnOn();
    static void turnOff();
    static void update();

private:
    static unsigned long relayStartTime;
    static bool relayActive;
};

#endif
