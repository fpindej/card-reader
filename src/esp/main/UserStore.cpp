#include <HTTPClient.h>
#include "UserStore.h"
#include "WiFiManager.h"

#define MAX_USERS 100                 // Max number of users to store
#define USER_ID_LENGTH 10
#define UPDATE_INTERVAL 1800000       // 30 minutes update interval for valid users

String validUsers[MAX_USERS];         // Array to store valid users
int userCount = 0;                    // Current number of valid users

const char* UserStore::endpointGetValidUsers = "https://gym.pindej.cz/api/rfidcard/getallactive";

void UserStore::updateValidUsers() 
{
    static unsigned long lastUpdateTime = 0;
    unsigned long currentMillis = millis();

    // Update every <UPDATE_INTERVAL> or on card verification
    if (currentMillis - lastUpdateTime > UPDATE_INTERVAL) 
    {
        fetchUsersFromServer();
        lastUpdateTime = currentMillis;
    }
}

void UserStore::fetchUsersFromServer() 
{
    if (!WiFiManager::isConnected()) 
    {
        Serial.println("WiFi not connected, cannot update user list.");
        return;
    }

    HTTPClient http;
    http.begin(endpointGetValidUsers);
    int httpResponseCode = http.GET();

    if (httpResponseCode != 200) 
    {
      Serial.println("Failed to update user list from server.");
      return;
    }

    String response = http.getString();
    http.end();

    // Check if the response is an empty array
    if (response == "[]") 
    {
        Serial.println("No valid users found in the response.");
        userCount = 0;
        return;
    }

    // Clear previous users
    userCount = 0;
    parseAndSaveUserList(response);
    Serial.println("User list updated.");
}

bool UserStore::isUserValid(const String& rfidId) 
{
    for (int i = 0; i < userCount; i++) 
    {
        if (validUsers[i] == rfidId) 
        {
            return true;
        }
    }
    return false;
}

void UserStore::parseAndSaveUserList(const String& response) 
{
    int startIndex = 0;
    while ((startIndex = response.indexOf('"', startIndex)) != -1) 
    {
        int endIndex = response.indexOf('"', startIndex + 1);
        if (endIndex == -1) break; // No more users
        String user = response.substring(startIndex + 1, endIndex);
        if (userCount < MAX_USERS) 
        {
            validUsers[userCount++] = user;
        }
        startIndex = endIndex + 1;
    }
}
