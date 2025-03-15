#include "RelayHandler.h"
#include <Arduino.h>

#define RELAY_PIN 26

unsigned long RelayHandler::relayStartTime = 0;
bool RelayHandler::relayActive = false;

void RelayHandler::init() 
{
    pinMode(RELAY_PIN, OUTPUT);
    digitalWrite(RELAY_PIN, LOW);
    Serial.println("Relay initialized.");
}

void RelayHandler::checkAndUpdateRelay() 
{
    if (relayActive && (millis() - relayStartTime >= RELAY_ACTIVE_DURATION))
    {
        digitalWrite(RELAY_PIN, LOW);
        relayActive = false;
        Serial.println("Relay turned OFF.");
    }
}

void RelayHandler::activateRelay() 
{
    digitalWrite(RELAY_PIN, HIGH);
    relayStartTime = millis();
    relayActive = true;
    Serial.println("Relay turned ON.");
}
