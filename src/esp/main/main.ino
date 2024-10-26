#include "WiFiManager.h"
#include "UserStore.h"
#include "RFIDHandler.h"

void setup() 
{
    Serial.begin(9600);
    WiFiManager::connect();
    RFIDHandler::init();
    UserStore::fetchUsersFromServer();
}

void loop() 
{
    UserStore::updateValidUsers();
    RFIDHandler::checkCard();
}
