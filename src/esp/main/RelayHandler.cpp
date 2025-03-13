#include <Arduino.h>

#define RELAY_PIN 26

class RelayHandler {
public:
    static void init() {
        pinMode(RELAY_PIN, OUTPUT);
        digitalWrite(RELAY_PIN, LOW);
    }

    static void turnOn() {
        digitalWrite(RELAY_PIN, HIGH);
        relayStartTime = millis();
        relayActive = true;
    }

    static void turnOff() {
        digitalWrite(RELAY_PIN, LOW);
        relayActive = false;
    }

    static void update() {
        if (relayActive && (millis() - relayStartTime >= 2000)) {
            turnOff();
        }
    }

private:
    static unsigned long relayStartTime;
    static bool relayActive;
};

unsigned long RelayHandler::relayStartTime = 0;
bool RelayHandler::relayActive = false;
