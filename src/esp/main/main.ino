#include "WiFiManager.h"
#include "CardHandler.h"
#include "RFIDHandler.h"

void setup() 
{
    Serial.begin(9600);
    WiFiManager::connect();
    RFIDHandler::init();
    CardHandler::fetchCardsFromServer();
}

void loop() 
{
    CardHandler::updateActiveCards();
    RFIDHandler::checkCard();
}
