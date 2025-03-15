#include <ArduinoJson.h>
#include "LogHandler.h"
#include <HTTPClient.h>
#include <WiFi.h>
#include "TimeManager.h"
#include "WiFiManager.h"

const char* LogHandler::LOG_ENDPOINT = "https://gym.pindej.cz/api/AccessLog/log";
const char* LogHandler::BATCH_LOG_ENDPOINT = "https://gym.pindej.cz/api/AccessLog/logbatch";
const unsigned long LogHandler::PROCESS_INTERVAL = 60000; // 1 minute
const size_t LogHandler::MAX_QUEUE_SIZE = 100;
const size_t LogHandler::BATCH_SIZE = 10;

std::vector<LogEntry> LogHandler::logQueue;

void LogHandler::init() {
    logQueue.reserve(MAX_QUEUE_SIZE);
}

bool LogHandler::sendSingleLog(const LogEntry& entry) {
    if (!WiFiManager::isConnected()) {
        Serial.println("Cannot send log: WiFi not connected");
        return false;
    }

    HTTPClient http;
    http.begin(LOG_ENDPOINT);
    http.addHeader("Content-Type", "application/json");

    JsonDocument doc;
    doc["cardNumber"] = entry.cardNumber;
    doc["isSuccessful"] = entry.isSuccessful;
    doc["timestamp"] = entry.timestamp;

    String jsonPayload;
    serializeJson(doc, jsonPayload);

    int httpResponseCode = http.POST(jsonPayload);
    bool success = false;

    if (httpResponseCode > 0) {
        Serial.printf("Access log sent. Response code: %d\n", httpResponseCode);
        success = (httpResponseCode == 200 || httpResponseCode == 201);
    } else {
        Serial.printf("Error sending access log. Error: %s\n", http.errorToString(httpResponseCode).c_str());
    }

    http.end();
    return success;
}

bool LogHandler::sendBatchLogs(const std::vector<LogEntry>& entries) {
    if (!WiFiManager::isConnected()) {
        Serial.println("Cannot send batch: WiFi not connected");
        return false;
    }

    HTTPClient http;
    http.begin(BATCH_LOG_ENDPOINT);
    http.addHeader("Content-Type", "application/json");

    JsonDocument doc;
    JsonArray logsArray = doc.to<JsonArray>();

    for (const auto& entry : entries) {
        JsonObject logEntry = logsArray.createNestedObject();
        logEntry["cardNumber"] = entry.cardNumber;
        logEntry["isSuccessful"] = entry.isSuccessful;
        logEntry["timestamp"] = entry.timestamp;
    }

    String jsonPayload;
    serializeJson(doc, jsonPayload);

    int httpResponseCode = http.POST(jsonPayload);
    bool success = false;

    if (httpResponseCode > 0) {
        Serial.printf("Batch log sent (%d entries). Response code: %d\n", entries.size(), httpResponseCode);
        success = (httpResponseCode == 200 || httpResponseCode == 201);
    } else {
        Serial.printf("Error sending batch log. Error: %s\n", http.errorToString(httpResponseCode).c_str());
    }

    http.end();
    return success;
}

void LogHandler::logAccess(const String& cardNumber, bool isSuccessful) {
    LogEntry entry = {
        .cardNumber = cardNumber,
        .isSuccessful = isSuccessful,
        .timestamp = TimeManager::getCurrentUTCTime()
    };

    // Try to send immediately only if we're connected
    if (WiFiManager::isConnected() && sendSingleLog(entry)) {
        return; // Successfully sent immediately
    }

    // If sending failed or we're offline, queue the entry
    if (logQueue.size() < MAX_QUEUE_SIZE) {
        logQueue.push_back(entry);
        Serial.printf("Log entry queued. Queue size: %d\n", logQueue.size());
    } else {
        Serial.println("Warning: Log queue is full, entry discarded");
    }
}

void LogHandler::processQueue() {
    if (logQueue.empty()) {
        return;
    }

    // Only process if we have WiFi connection
    if (!WiFiManager::isConnected()) {
        Serial.println("Queue processing skipped: No WiFi connection");
        return;
    }

    Serial.printf("Processing queue. Current size: %d\n", logQueue.size());

    // Process queue in batches
    while (!logQueue.empty() && WiFiManager::isConnected()) {
        // Determine batch size for this iteration
        size_t currentBatchSize = min(BATCH_SIZE, logQueue.size());
        std::vector<LogEntry> batch(logQueue.begin(), logQueue.begin() + currentBatchSize);

        if (sendBatchLogs(batch)) {
            // Remove sent entries from queue
            logQueue.erase(logQueue.begin(), logQueue.begin() + currentBatchSize);
            Serial.printf("Batch sent successfully. Remaining queue size: %d\n", logQueue.size());
        } else {
            // If sending failed, break and try again later
            Serial.println("Batch send failed, will retry later");
            break;
        }
    }
}

void LogHandler::processQueuedLogs() {
    if (WiFiManager::isConnected()) {
        processQueue();
    }
}