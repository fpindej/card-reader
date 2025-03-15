#include "HealthMonitor.h"
#include <.env.h>

const char* HealthMonitor::_endpoint = HEALTH_ENDPOINT;

SystemMetrics HealthMonitor::_lastMetrics;
unsigned long HealthMonitor::_lastCheck = 0;

void HealthMonitor::collectMetrics() {
    _lastMetrics.freeHeap = ESP.getFreeHeap();
    _lastMetrics.maxAllocHeap = ESP.getMaxAllocHeap();
    _lastMetrics.minFreeHeap = ESP.getMinFreeHeap();
    _lastMetrics.uptime = millis() / 1000; // Convert to seconds
    _lastMetrics.freeSketchSpace = ESP.getFreeSketchSpace();
}

void HealthMonitor::reportMetrics() {
    if (millis() - _lastCheck < CHECK_INTERVAL) {
        return;
    }

    collectMetrics();
    
    StaticJsonDocument<200> doc;
    doc["freeHeap"] = _lastMetrics.freeHeap;
    doc["maxAllocHeap"] = _lastMetrics.maxAllocHeap;
    doc["minFreeHeap"] = _lastMetrics.minFreeHeap;
    doc["uptime"] = _lastMetrics.uptime;
    doc["freeSketchSpace"] = _lastMetrics.freeSketchSpace;

    String jsonString;
    serializeJson(doc, jsonString);

    HTTPClient http;
    http.begin(_endpoint);
    http.addHeader("Content-Type", "application/json");
    
    int httpCode = http.POST(jsonString);
    http.end();

    _lastCheck = millis();
}
