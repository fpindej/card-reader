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

    // Construct the JSON payload
    String jsonPayload = "{\"cardNumber\":\"" + cardNumber + "\",\"isSuccessful\":" + (isSuccessful ? "true" : "false") + "}";

    int httpResponseCode = http.POST(jsonPayload);

    if (httpResponseCode > 0) {
        Serial.printf("Access log sent. Response code: %d\n", httpResponseCode);
    } else {
        Serial.printf("Error sending access log. Error: %s\n", http.errorToString(httpResponseCode).c_str());
    }

    http.end();
}