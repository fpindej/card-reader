#include <WiFi.h>
#include "WiFiManager.h"

const char* WiFiManager::ssid = "<WIFI_SSID>";
const char* WiFiManager::password = "<WIFI_PASSWORD>";

void WiFiManager::connect() 
{
    Serial.print("Connecting to ");
    Serial.print(ssid);
    WiFi.begin(ssid, password);

    while (WiFi.status() != WL_CONNECTED) 
    {
        delay(500);
        Serial.print(".");
    }
    Serial.println("\nWiFi connected");
}

bool WiFiManager::isConnected() {
    return WiFi.status() == WL_CONNECTED;
}

void WiFiManager::reconnectIfNeeded() 
{
    if (WiFi.status() != WL_CONNECTED) 
    {
        connect();
    }
}