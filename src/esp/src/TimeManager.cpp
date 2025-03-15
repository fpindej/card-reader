#include "TimeManager.h"
#include <time.h>
#include <Arduino.h>

bool TimeManager::timeInitialized = false;
const char* TimeManager::NTP_SERVER_1 = "pool.ntp.org";
const char* TimeManager::NTP_SERVER_2 = "time.nist.gov";

void TimeManager::initialize() {
    configTime(GMT_OFFSET_SEC, DAYLIGHT_OFFSET_SEC, NTP_SERVER_1, NTP_SERVER_2);
    
    Serial.println("Waiting for NTP time sync...");
    time_t now = time(nullptr);
    int attempts = 0;
    const int MAX_ATTEMPTS = 20; // 10 seconds maximum

    while (now < 8 * 3600 * 2 && attempts < MAX_ATTEMPTS) {
        delay(500);
        Serial.print(".");
        now = time(nullptr);
        attempts++;
    }
    
    if (attempts < MAX_ATTEMPTS) {
        Serial.println("\nTime synchronized successfully!");
        timeInitialized = true;
    } else {
        Serial.println("\nFailed to synchronize time!");
        timeInitialized = false;
    }
}

bool TimeManager::isTimeSet() {
    return timeInitialized;
}

void TimeManager::sync() {
    initialize();
}

String TimeManager::getCurrentUTCTime() {
    struct tm timeinfo;
    if (!getLocalTime(&timeinfo)) {
        return "Failed to obtain time";
    }
    
    char timeStringBuff[30];
    strftime(timeStringBuff, sizeof(timeStringBuff), "%Y-%m-%dT%H:%M:%S", &timeinfo);
    
    // Add milliseconds and Z suffix
    unsigned long milliseconds = millis() % 1000;
    char fullTimeString[35];
    snprintf(fullTimeString, sizeof(fullTimeString), "%s.%03luZ", timeStringBuff, milliseconds);
    
    return String(fullTimeString);
}
