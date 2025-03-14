#include "WiFiManager.h"
#include "CardHandler.h"
#include "RFIDHandler.h"
#include "RelayHandler.h"

void setup() 
{
    Serial.begin(9600);
    WiFiManager::connect();
    RFIDHandler::init();
    RelayHandler::init();
    CardHandler::fetchCardsFromServer();
}

void loop() 
{
    WiFiManager::checkAndReconnect();
    CardHandler::updateActiveCards();
    RFIDHandler::checkCard();
}
