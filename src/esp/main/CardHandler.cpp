#include <HTTPClient.h>
#include "CardHandler.h"
#include "WiFiManager.h"

#define MAX_CARDS 100                 // Max number of active cards to store
#define CARD_ID_LENGTH 10
#define UPDATE_INTERVAL 1800000       // 30 minutes update interval for active cards

String activeCards[MAX_CARDS];
int cardCount = 0;

const char* CardHandler::endpointGetActiveCards = "https://gym.pindej.cz/api/rfidcard/getallactive";

void CardHandler::updateActiveCards() 
{
    static unsigned long lastUpdateTime = 0;
    unsigned long currentMillis = millis();

    // Update every <UPDATE_INTERVAL> or on card verification
    if (currentMillis - lastUpdateTime > UPDATE_INTERVAL) 
    {
        fetchCardsFromServer();
        lastUpdateTime = currentMillis;
    }
}

void CardHandler::fetchCardsFromServer() 
{
    if (!WiFiManager::isConnected()) 
    {
        Serial.println("WiFi not connected, cannot update active cards.");
        return;
    }

    HTTPClient http;
    http.begin(endpointGetActiveCards);
    int httpResponseCode = http.GET();

    if (httpResponseCode != 200) 
    {
      Serial.println("Failed to update active cards from server.");
      return;
    }

    String response = http.getString();
    http.end();

    // Check if the response is an empty array
    if (response == "[]") 
    {
        Serial.println("No active cards found in the response.");
        cardCount = 0;
        return;
    }

    // Clear previous cards
    cardCount = 0;
    parseAndSaveCardList(response);
    Serial.println("Active cards updated.");
}

bool CardHandler::isCardActive(const String& rfidId) 
{
    for (int i = 0; i < cardCount; i++) 
    {
        if (activeCards[i] == rfidId) 
        {
            return true;
        }
    }
    return false;
}

void CardHandler::parseAndSaveCardList(const String& response) 
{
    int startIndex = 0;
    while ((startIndex = response.indexOf('"', startIndex)) != -1) 
    {
        int endIndex = response.indexOf('"', startIndex + 1);
        if (endIndex == -1) break; // No more active cards
        String card = response.substring(startIndex + 1, endIndex);
        if (cardCount < MAX_CARDS) 
        {
            activeCards[cardCount++] = card;
        }
        startIndex = endIndex + 1;
    }
}
