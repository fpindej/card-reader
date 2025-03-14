#include <WiFi.h>
#include "WiFiManager.h"

const char* WiFiManager::ssid = "<WIFI_SSID>";
const char* WiFiManager::password = "<WIFI_PASSWORD>";
unsigned long WiFiManager::lastReconnectAttempt = 0;

void WiFiManager::connect() 
{
    Serial.print("Connecting to ");
    Serial.print(ssid);
    WiFi.begin(ssid, password);

    // Wait for connection with timeout
    unsigned long startAttempt = millis();
    while (WiFi.status() != WL_CONNECTED && millis() - startAttempt < 10000) // 10 second timeout
    {
        delay(500);
        Serial.print(".");
    }
    
    if (WiFi.status() == WL_CONNECTED) {
        Serial.println("\nWiFi connected");
        Serial.print("IP address: ");
        Serial.println(WiFi.localIP());
    } else {
        Serial.println("\nWiFi connection failed");
    }
}

bool WiFiManager::isConnected() {
    return WiFi.status() == WL_CONNECTED;
}

void WiFiManager::checkAndReconnect() 
{
    unsigned long currentMillis = millis();
    
    // If WiFi is disconnected and it's time to try reconnecting
    if (!isConnected() && (currentMillis - lastReconnectAttempt >= RECONNECT_INTERVAL)) 
    {
        Serial.println("WiFi disconnected, attempting to reconnect...");
        WiFi.disconnect();
        WiFi.begin(ssid, password);
        lastReconnectAttempt = currentMillis;
    }
    
    // If we just got connected, print the IP address
    static bool wasConnected = false;
    bool isNowConnected = isConnected();
    
    if (!wasConnected && isNowConnected) {
        Serial.println("WiFi reconnected successfully");
        Serial.print("IP address: ");
        Serial.println(WiFi.localIP());
    }
    
    wasConnected = isNowConnected;
}
