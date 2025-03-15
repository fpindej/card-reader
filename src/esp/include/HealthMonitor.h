#ifndef HEALTH_MONITOR_H
#define HEALTH_MONITOR_H

#include <Arduino.h>
#include <HTTPClient.h>
#include <ArduinoJson.h>

struct SystemMetrics {
    uint32_t freeHeap;
    uint32_t maxAllocHeap;
    uint32_t minFreeHeap;
    uint32_t uptime;
    size_t freeSketchSpace;
};

class HealthMonitor {
public:
    static void collectMetrics();
    static void reportMetrics();

private:
    static const char* _endpoint;
    static SystemMetrics _lastMetrics;
    static unsigned long _lastCheck;
    static const unsigned long CHECK_INTERVAL = 300000; // 5 minutes
};

#endif
