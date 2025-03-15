#include <Arduino.h>
#include <ArduinoJson.h>
#include <MFRC522.h>

#include "WiFiManager.h"
#include "CardHandler.h"
#include "RFIDHandler.h"
#include "RelayHandler.h"
#include "RemoteHandler.h"
#include "LogHandler.h"

void setup() 
{
    Serial.begin(9600);
    WiFiManager::connect();
    RFIDHandler::init();
    RelayHandler::init();
    CardHandler::fetchCardsFromServer();
    RemoteHandler::init();
    LogHandler::init();
}

void loop() 
{
    WiFiManager::checkAndReconnect();
    CardHandler::updateActiveCards();
    RFIDHandler::checkCard();
    RemoteHandler::handleClient();
}