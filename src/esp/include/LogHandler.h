#ifndef LOG_HANDLER_H
#define LOG_HANDLER_H

#include <Arduino.h>
#include <vector>

struct LogEntry {
    String cardNumber;
    bool isSuccessful;
    String timestamp;
};

class LogHandler {
public:
    static void init();
    static void logAccess(const String& cardNumber, bool isSuccessful);
    static void processQueuedLogs();
    
private:
    static const char* LOG_ENDPOINT;
    static const char* BATCH_LOG_ENDPOINT;
    static const unsigned long PROCESS_INTERVAL; // 1 minute in milliseconds
    static const size_t MAX_QUEUE_SIZE; // Maximum number of entries to store
    static const size_t BATCH_SIZE; // Number of entries to send in one batch
    
    static std::vector<LogEntry> logQueue;
    
    static bool sendSingleLog(const LogEntry& entry);
    static bool sendBatchLogs(const std::vector<LogEntry>& entries);
    static void processQueue();
};

#endif