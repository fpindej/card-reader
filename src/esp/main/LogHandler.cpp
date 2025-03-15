#include <ArduinoJson.h>
#include "LogHandler.h"
#include <HTTPClient.h>
#include <WiFi.h>

const char* LogHandler::LOG_ENDPOINT = "https://gym.pindej.cz/api/AccessLog/log";


void LogHandler::logAccess(const String& cardNumber, bool isSuccessful) {
    if (WiFi.status() != WL_CONNECTED) {
        Serial.println("Error: WiFi not connected");
        return;
    }

    HTTPClient http;
    http.begin(LOG_ENDPOINT);
    http.addHeader("Content-Type", "application/json");

    JsonDocument doc;
    doc["cardNumber"] = cardNumber;
    doc["isSuccessful"] = isSuccessful;

    String jsonPayload;
    serializeJson(doc, jsonPayload);

    int httpResponseCode = http.POST(jsonPayload);

    if (httpResponseCode > 0) {
        Serial.printf("Access log sent. Response code: %d\n", httpResponseCode);
    } else {
        Serial.printf("Error sending access log. Error: %s\n", http.errorToString(httpResponseCode).c_str());
    }

    http.end();
}